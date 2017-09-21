using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Common.Logging;
using Ionic.Zlib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WotDossier.Applications.Parser;
using WotDossier.Common;
using WotDossier.Common.Extensions;
using WotDossier.Domain.Replay;

namespace WotDossier.Applications
{
    public static class ReplayFileHelper
    {
private const string REPLAY_DATABLOCK_2 = "datablock_2";

        private static readonly ILog _log = LogManager.GetLogger("ReplayFileHelper");
        private static readonly Version _jsonFormatedReplayMinVersion = new Version("0.8.11.0");
        private static readonly Version _prevMinVersion = new Version("0.9.8.0");
        
        public static Replay ParseReplay(FileInfo phisicalFile, Version clientVersion, bool readAdvancedData)
        {
            //if (clientVersion < _jsonFormatedReplayMinVersion)
            //{
            //    return ParseReplay_8_0(phisicalFile, readAdvancedData);
            //}
            return ParseReplay_8_11(phisicalFile, readAdvancedData);
        }

        /// <summary>
        /// Binary dossier cache to plain json.
        /// </summary>
        /// <param name="replayFile">The replay file.</param>
        /// <param name="readAdvancedData">if set to <c>true</c> [read advanced data].</param>
        /// <returns>Path to json file</returns>
        public static string ReplayToJson(FileInfo replayFile, bool readAdvancedData = false)
        {
            string outputJsonFilePath = replayFile.FullName.Replace(replayFile.Extension, ".json");
            
            //if file already created then delete it
            if (File.Exists(outputJsonFilePath))
            {
                File.Delete(outputJsonFilePath);
            }

            string directoryName = Environment.CurrentDirectory;

            string task = directoryName + @"\External\wotrp2j.exe";
            string arguments = string.Format("\"{0}\" {1}", replayFile.FullName, readAdvancedData ? "-a" : string.Empty);
            var logPath = directoryName + @"\Logs\wotrp2j.log";
            var workingDirectory = directoryName + @"\External";

            ExternalTask.Execute(task, arguments, logPath, workingDirectory);

            return outputJsonFilePath;
        }

        /// <summary>
        /// Loads the replay.
        /// </summary>
        /// <param name="replayFile">The replay file.</param>
        /// <param name="readAdvancedData">if set to <c>true</c> [read advanced data].</param>
        /// <returns></returns>
        public static Replay ParseReplay_8_0(FileInfo replayFile, bool readAdvancedData = false)
        {
            //convert dossier cache file to json
            string jsonFilePath = ReplayToJson(replayFile, false);

            string content = File.ReadAllText(jsonFilePath);

            JObject jObject = JsonConvert.DeserializeObject<JObject>(content);

            Replay replay = jObject.ToObject<Replay>();

            if (((IDictionary<string, JToken>)jObject).ContainsKey(REPLAY_DATABLOCK_2))
            {
                replay.datablock_1.vehicles = jObject[REPLAY_DATABLOCK_2][1].ToObject<Dictionary<long, Vehicle>>();
            }

            if (readAdvancedData)
            {
                string path = replayFile.FullName;
                if (File.Exists(path))
                {
                    using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        int blocksCount = 0;
                        byte[] buffer = new byte[4];
                        stream.Read(buffer, 0, 4);

                        if (buffer[0] != 0x21)
                        {
                            blocksCount = (int) stream.Read(4).ConvertLittleEndian();
                            _log.Trace("Found Replay Blocks: " + blocksCount);
                        }

                        for (int i = 0; i < blocksCount; i++)
                        {
                            int blockLength = (int)stream.Read(4).ConvertLittleEndian();
                            stream.Seek(blockLength, SeekOrigin.Current);
                        }

                        ReadAdvancedDataBlock(stream, replay);
                    }
                }
            }

            File.Delete(jsonFilePath);

            return replay;
        }

        /// <summary>
        /// Reads the replay statistic blocks.
        /// </summary>
        /// <param name="file">The replay file.</param>
        /// <param name="readAdvancedData">if set to <c>true</c> [read advanced data].</param>
        /// <returns></returns>
        public static Replay ParseReplay_8_11(FileInfo file, bool readAdvancedData = false)
        {
            _log.Trace("Parse Replay: " + file);

            string path = file.FullName;
            if (File.Exists(path))
            {
                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    Replay replay = new Replay();

                    try
                    {
                        int blocksCount = 0;
                        byte[] buffer = new byte[4];
                        stream.Read(buffer, 0, 4);

                        if (buffer[0] != 0x21)
                        {
                            blocksCount = (int) stream.Read(4).ConvertLittleEndian();
                            _log.Trace("Found Replay Blocks: " + blocksCount);
                        }

                        if(blocksCount > 5)
                        {
                            _log.ErrorFormat("Uncompressed replay({0})", file.FullName);
                            return null;
                        }

                        for (int i = 0; i < blocksCount; i++)
                        {
                            int blockLength = (int)stream.Read(4).ConvertLittleEndian();
                            _log.Trace(string.Format("{0} block length: {1}", i + 1, blockLength));
                            byte[] blockData = stream.Read(blockLength);

                            //read first block
                            if (i == 0)
                            {
                                InitFirstBlock(replay, blockData);
                            }

                            //read second block
                            if (i == 1)
                            {
                                DateTime playTime = DateTime.Parse(replay.datablock_1.dateTime, CultureInfo.GetCultureInfo("ru-RU"));
                                Version version = ResolveVersion(replay.datablock_1.Version, playTime);

                                if (version < _jsonFormatedReplayMinVersion && blocksCount < 3)
                                {
                                    InitThirdBlock(replay, blockData);
                                }
                                else
                                {
                                    InitSecondBlock(replay, blockData);
                                }
                            }

                            //read third block for replays 0.8.1-0.8.10
                            if (i == 2)
                            {
                                InitThirdBlock(replay, blockData);
                            }
                        }

                        if (replay.datablock_1 != null)
                        {
                            //read advanced data block
                            if (readAdvancedData)
                            {
                                try
                                {
                                    ReadAdvancedDataBlock(stream, replay);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                }
                            }
                            return replay;
                        }
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat("Error on replay file read. Incorrect file format({0})", e, file.FullName);
                    }
                }
            }
            return null;
        }

        private static void InitThirdBlock(Replay replay, byte[] blockData)
        {
            replay.datablock_battle_result = ReadThirdBlock(blockData).ToObject<BattleResult>();
        }

        private static void InitFirstBlock(Replay replay, byte[] blockData)
        {
            var json = ReadFirstBlock(blockData);
            if (!string.IsNullOrEmpty(json))
            {
                replay.datablock_1 = JsonConvert.DeserializeObject<FirstBlock>(json);
            }
        }

        public static string ReadFirstBlock(byte[] blockData)
        {
            string json = blockData.GetAsciiString();
            return json;
        }

        private static void InitSecondBlock(Replay replay, byte[] blockData)
        {
            var parsedData = ReadSecondBlock(blockData);
            if (parsedData.Count > 0)
            {
                //0.8.11+
                if (replay.datablock_1.Version >= _jsonFormatedReplayMinVersion)
                {
                    if (replay.datablock_1.Version >= _prevMinVersion)
                    {

	                    JsonSerializerSettings settings = new JsonSerializerSettings();
	                    settings.Converters.Add(new PersonalConverter());
	                    var battleResult = parsedData[0].ToObject<BattleResult98>(JsonSerializer.CreateDefault(settings));
						//var battleResult = parsedData[0].ToObject<BattleResult98>();

                        replay.datablock_battle_result = new BattleResult();
                        replay.datablock_battle_result.arenaUniqueID = battleResult.arenaUniqueID;
                        replay.datablock_battle_result.common = battleResult.common;
                        replay.datablock_battle_result.players = battleResult.players;
                        replay.datablock_battle_result.personal = battleResult.personal.Values.First() as Personal;
						if(battleResult.personal.ContainsKey("avatar"))
							replay.datablock_battle_result.avatar = battleResult.personal["avatar"] as Avatar;
						else
							replay.datablock_battle_result.avatar = new Avatar();
						replay.datablock_battle_result.vehicles = battleResult.vehicles.ToDictionary(x => x.Key, y => y.Value.First());
                    }
                    else
                    {
                        replay.datablock_battle_result = parsedData[0].ToObject<BattleResult>();
                    }
                }
                replay.datablock_1.vehicles = parsedData[1].ToObject<Dictionary<long, Vehicle>>();
            }
        }

        public static JArray ReadSecondBlock(byte[] blockData)
        {
            string json = blockData.GetAsciiString();
            var parsedData = JsonConvert.DeserializeObject<JArray>(json);
            return parsedData;
        }

        public static object ReadThirdBlock(byte[] blockData)
        {
            return Unpickle.Load(new MemoryStream(blockData));
        }

        private static void ReadAdvancedDataBlock(FileStream stream, Replay replay)
        {
            _log.Trace("Start read advanced data");
            const int startPosition = 8;
            stream.Seek(startPosition, SeekOrigin.Current);

            int replayDataLength = (int)(stream.Length - stream.Position) - startPosition;

            byte[] decrypt = DecryptReplayData(stream.Read(replayDataLength));
            byte[] uncompressed = DecompressReplayData(decrypt);

            //uncompressed.Dump(@"c:\\temp");

            var replayWriterVehicle = replay.datablock_battle_result.vehicles.First(v =>
                v.Value.accountDBID == replay.datablock_battle_result.personal.accountDBID).Key;

            //ReplayUser = Vehicles.First(v => v.AccountDBID == replay.datablock_battle_result.personal.accountDBID);

            using (var uncompressedReplayStream = new MemoryStream(uncompressed))
            {
                BaseParser parser = GetParser(replay);

                parser.ReplayWriterVehicle = Convert.ToUInt64(replayWriterVehicle);

                replay.datablock_advanced = new AdvancedReplayData();

                AdvancedReplayDataLoader loader = new AdvancedReplayDataLoader(replay.datablock_advanced);

                parser.ReadReplayStream(uncompressedReplayStream, loader.Handle);

                replay.Stream = uncompressed;
            }
            _log.Trace("End read advanced data");
        }

        public static BaseParser GetParser(Replay replay)
        {
            DateTime playTime = DateTime.Parse(replay.datablock_1.dateTime, CultureInfo.GetCultureInfo("ru-RU"));
            Version version = ResolveVersion(replay.datablock_1.Version, playTime);

            if (version <= new Version("0.8.1.0"))
            {
                return new Parser81();
            }
            if (version <= new Version("0.8.3.0"))
            {
                return new Parser83();
            }
            if (version <= new Version("0.8.5.0"))
            {
                return new Parser85();
            }
            if (version <= new Version("0.8.7.0"))
            {
                return new Parser86();
            }
            if (version <= new Version("0.8.8.0"))
            {
                return new Parser88();
            }
            if (version < new Version("0.9.3.0"))
            {
                return new BaseParser();
            }
            if (version < new Version("0.9.6.0"))
            {
                return new Parser93();
            }
            if (version < new Version("0.9.8.0"))
            {
                return new Parser96();
            }
            if (version < new Version("0.9.9.0"))
            {
                return new Parser98();
            }
            if (version < new Version("0.9.10.0"))
            {
                return new Parser99();
            }
            if (version < new Version("0.9.12.0"))
            {
                return new Parser910();
            }
            if (version < new Version("0.9.13.0"))
            {
                return new Parser912();
            }
            if (version < new Version("0.9.14.0"))
            {
                return new Parser913();
            }
            if (version < new Version("0.9.15.0"))
            {
                return new Parser914();
            }
            if (version < new Version("0.9.16.0"))
            {
                return new Parser915();
            }
            if (version < new Version("0.9.17.0"))
            {
                return new Parser916();
            }
            if (version < new Version("0.9.17.1"))
            {
                return new Parser917();
            }
	        if (version < new Version("0.9.19.1"))
	        {
		        return new Parser9171();
	        }
			if (version < new Version("0.9.20.0"))
	        {
		        return new Parser9191();
	        }
			return new Parser920();
        }


        /// <summary>
        /// Decompresses the specified decrypted replay bytes.
        /// </summary>
        /// <param name="decryptedReplaysBytes">The decrypted replay bytes.</param>
        /// <returns></returns>
        private static byte[] DecompressReplayData(byte[] decryptedReplaysBytes)
        {
            return ZlibStream.UncompressBuffer(decryptedReplaysBytes);
        }

        /// <summary>
        /// Decrypts the specified data.
        /// </summary>
        /// <param name="replayData">The replay data.</param>
        /// <returns></returns>
        private static byte[] DecryptReplayData(byte[] replayData)
        {
            byte[] key =
            {
                0xDE, 0x72, 0xBE, 0xA0,
                0xDE, 0x04, 0xBE, 0xB1,
                0xDE, 0xFE, 0xBE, 0xEF,
                0xDE, 0xAD, 0xBE, 0xEF
            };

            BlowFish blowFish = new BlowFish(key);

            byte[] block = new byte[8];

            MemoryStream dataStream = new MemoryStream();

            byte[] pb = null;

            for (int i = 0, bi = 0; i < replayData.Length; i++, bi++)
            {
                block[bi] = replayData[i];
                if (bi == 7 || i == replayData.Length - 1)
                {
                    byte[] db = blowFish.Decrypt_ECB(block);

                    if (pb != null)
                    {
                        db = ByteArrayExtensions.Xor(pb, db);
                    }

                    dataStream.Write(db, 0, 8);
                    pb = db;
                    block = new byte[8];
                    bi = -1;
                }
            }

            return dataStream.ToArray();
        }

        public static Version ResolveVersion(Version version, DateTime playTime)
        {
            if (version == new Version(0, 0))
            {
                if (playTime.Date >= new DateTime(2013, 4, 18))
                {
                    return new Version("0.8.5.0");
                }

                if (playTime.Date >= new DateTime(2013, 2, 28))
                {
                    return new Version("0.8.4.0");
                }

                if (playTime.Date >= new DateTime(2013, 1, 16))
                {
                    return new Version("0.8.3.0");
                }

                if (playTime.Date >= new DateTime(2012, 12, 8))
                {
                    return new Version("0.8.2.0");
                }

                if (playTime.Date >= new DateTime(2012, 10, 25))
                {
                    return new Version("0.8.1.0");
                }

                if (playTime.Date >= new DateTime(2012, 09, 25))
                {
                    return new Version("0.8.0.0");
                }
            }
            return version;
        }

        public static int GetAutoEquipCost(Replay replay)
        {
            if (replay.datablock_battle_result.personal.autoEquipCost != null)
            {
                return replay.datablock_battle_result.personal.autoEquipCost.Sum();
            }
            return 0;
        }

        public static int GetAutoLoadCost(Replay replay)
        {
            if (replay.datablock_battle_result.personal.autoLoadCost != null)
            {
                return replay.datablock_battle_result.personal.autoLoadCost.Sum();
            }
            return 0;
        }
    }

    internal class AdvancedReplayDataLoader
    {
        private AdvancedReplayData _data;

        public AdvancedReplayDataLoader(AdvancedReplayData data)
        {
            _data = data;
        }

        public void Handle(Packet packet)
        {
            dynamic data = packet.Data;

            if (packet.Type == PacketType.ArenaUpdate)
            {
                if (_data.roster == null)
                {
                    _data.roster = data.roster;
                }
            }

            if (packet.Type == PacketType.BattleLevel)
            {
                _data.playername = data.playername;
                _data.more = data.more;
            }

            if (packet.Type == PacketType.ChatMessage)
            {
                if (data.Message != null)
                {
                    _data.Messages.Add(data.Message);
                }
            }

            if (packet.Type == PacketType.DamageReceived)
            {

            }

            if (packet.Type == PacketType.SlotUpdate)
            {
                var foundItem = _data.Slots.FirstOrDefault(x => x.Item.Equals(data.Slot.Item));

                if (foundItem == null)
                {
                    _data.Slots.Add(data.Slot);
                }
                else
                {
                    foundItem.EndCount = data.Slot.Count;
                }
            }

            if (packet.Type == PacketType.Version)
            {
                _data.replay_version = data.replay_version;
            }
        }
    }

    internal class DamageReceived
    {
        public int Health { get; set; }
        public int Source { get; set; }
    }

    internal class PersonalConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(ResultBase));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Load the JSON for the Result into a JObject
            JObject jo = JObject.Load(reader);


	        if (jo["autoLoadCost"] != null)
		        return jo.ToObject<Personal>();

	        if (jo["totalDamaged"] != null)
		        return jo.ToObject<Avatar>();
			// Read the properties which will be used as constructor parameters
			//            int? code = (int?)jo["Code"];
			//            string format = (string)jo["Format"];

			// Construct the Result object using the non-default constructor
			//            Result result = new Result(code, format);

			// (If anything else needs to be populated on the result object, do that here)

			// Return the result
			//            return result;
			//return base.ReadJson(reader, objectType, existingValue, serializer);
			return jo.ToObject<Avatar>();
		}

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}