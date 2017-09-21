using System;
using System.Dynamic;
using System.IO;
using WotDossier.Common;
using WotDossier.Common.Extensions;
using WotDossier.Domain.Replay;

namespace WotDossier.Applications.Parser
{
    public class Parser915 : Parser914
    {
        protected override ulong PacketVersion => 0x18;
	    public override ulong UpdateEvent_Health => 0x05;

		/// <summary>
		/// Process packet 0x00
		/// Contains Battle level setup and Player Name.
		/// </summary>
		/// <param name="packet">The packet.</param>
		public override void ProcessPacketBattleLevel(Packet packet)
        {
            packet.Type = PacketType.BattleLevel;

            dynamic data = new ExpandoObject();

            packet.Data = data;

            using (var f = new MemoryStream(packet.Payload))
            {
                f.Seek(14, SeekOrigin.Begin);

                int playernamelength = (int)f.Read(1).ConvertLittleEndian();

                data.playername = f.Read(playernamelength).GetUtf8String();

                f.Seek(playernamelength + 29, SeekOrigin.Begin);

                data.more = new BattleInfo915();
                int advancedlength = (int)f.Read(1).ConvertLittleEndian();

                if (advancedlength == 255)
                {
                    advancedlength = (int)f.Read(2).ConvertLittleEndian();
                    f.Seek(1, SeekOrigin.Current);
                }

                try
                {
                    byte[] advancedPickles = f.Read(advancedlength);
                    object load = Unpickle.Load(new MemoryStream(advancedPickles));
                    data.more = load.ToObject<BattleInfo915>();
                }
                catch (Exception e)
                {
                    _log.Error("Cannot load battle info pickle object. \nPosition: " + f.Position + ", Length: " + advancedlength, e);
                }
            }
        }
    }
}