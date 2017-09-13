using System;
using System.Dynamic;
using System.IO;
using WotDossier.Common;
using WotDossier.Common.Extensions;
using WotDossier.Domain;
using WotDossier.Domain.Replay;

namespace WotDossier.Applications.Parser
{
    public class Parser920 : Parser9191
    {
	    protected override ulong UpdateEvent_Arena => 0x2d;

	    protected override ulong UpdateEvent_Slot => 0xC;

        /// <summary>
        /// Processes the player position.
        /// </summary>
        /// <param name="packet">The packet.</param>
        protected override void ProcessPacketPlayerPosition(Packet packet)
        {
            packet.Type = PacketType.PlayerPos;

            dynamic data = new ExpandoObject();

            packet.Data = data;

            using (MemoryStream f = new MemoryStream(packet.Payload))
            {
                var playerId = f.Read(4).ConvertLittleEndian();

                packet.PlayerId = playerId;
                data.PlayerId = playerId;

                f.Seek(8, SeekOrigin.Begin);

                var pos1 = f.Read(4).ToSingle();
                var pos2 = f.Read(4).ToSingle();
                var pos3 = f.Read(4).ToSingle();
                data.position = new[] { pos1, pos2, pos3 };

                f.Seek(32, SeekOrigin.Begin);

                var hull1 = f.Read(4).ToSingle();
                var hull2 = f.Read(4).ToSingle();
                var hull3 = f.Read(4).ToSingle();
                data.hull_orientation = new[] { hull1, hull2, hull3 };
            }
        }
    }
}