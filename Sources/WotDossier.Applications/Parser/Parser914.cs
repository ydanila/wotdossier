using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using WotDossier.Common;
using WotDossier.Common.Extensions;
using WotDossier.Domain.Replay;

namespace WotDossier.Applications.Parser
{
    public class Parser914 : Parser913
    {
        List<long> Ids = new List<long>();

        protected override ulong PacketVersion => 0x18;

        protected override ulong UpdateEvent_Slot
        {
            get { return 0x0a; }
        }

        protected override ulong UpdateEvent_Arena
        {
            get { return 0x29; }
        }

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

                data.more = new BattleInfo();
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
                    data.more = load.ToObject<BattleInfo>();
                }
                catch (Exception e)
                {
                    _log.Error("Cannot load battle info pickle object. \nPosition: " + f.Position + ", Length: " + advancedlength, e);
                }
            }
        }

        /// <summary>
        /// Processes the player position.
        /// </summary>
        /// <param name="packet">The packet.</param>
        //protected override void ProcessPacketPlayerPosition(Packet packet)
        //{
        //    packet.Type = PacketType.PlayerPos;

        //    dynamic data = new ExpandoObject();

        //    packet.Data = data;

        //    using (MemoryStream f = new MemoryStream(packet.Payload))
        //    {
        //        var id1 = (long)f.Read(4).ConvertLittleEndian();
        //        var id2 = (long)f.Read(4).ConvertLittleEndian();
        //        data.PlayerId = id2 == 0 ? id1 : id2;
        //        //data.PlayerId = (long)f.Read(4).ConvertLittleEndian();
        //        if (id2 != 0 && id1 == 6555161 && id2 != 6615128)
        //        {
        //            var x = 0;
        //        }

        //        if (!Ids.Contains(data.PlayerId))
        //            Ids.Add(data.PlayerId);

        //        f.Seek(12, SeekOrigin.Begin);

        //        var pos1 = f.Read(4).ToSingle();
        //        var pos2 = f.Read(4).ToSingle();
        //        var pos3 = f.Read(4).ToSingle();
        //        data.position = new[] { pos1, pos2, pos3 };

        //        f.Seek(36, SeekOrigin.Begin);

        //        var hull1 = f.Read(4).ToSingle();
        //        var hull2 = f.Read(4).ToSingle();
        //        var hull3 = f.Read(4).ToSingle();
        //        data.hull_orientation = new[] { hull1, hull2, hull3 };
        //    }
        //}
    }
}