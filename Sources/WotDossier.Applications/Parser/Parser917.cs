using System;
using System.Dynamic;
using System.IO;
using WotDossier.Common;
using WotDossier.Common.Extensions;
using WotDossier.Domain;
using WotDossier.Domain.Replay;

namespace WotDossier.Applications.Parser
{
    public class Parser917 : Parser916
    {
        protected override ulong PacketChat
        {
            get { return 0x23; }
        }

        protected override ulong UpdateEvent_Slot => 0x0e; //0x0a;

        protected override ulong UpdateEvent_Health
        {
            get { return 0x05; }
        }

        protected override ulong UpdateEvent_Arena => 0x2c;

        /// <summary>
        /// Process packet 0x08 subType 0x09
        /// Contains slots updates
        /// </summary>
        /// <param name="packet">The packet.</param>
        /// <param name="stream">The stream.</param>
        protected override void ProcessPacket_0x08_0x09(Packet packet, MemoryStream stream)
        {
            packet.Type = PacketType.SlotUpdate;
            //var buffer = new byte[packet.SubTypePayloadLength];
            //Read from your offset to the end of the packet, this will be the "update pickle". 
            //stream.Read(buffer, 0, (int) (packet.SubTypePayloadLength));

            dynamic data = new ExpandoObject();

            packet.Data = data;

            ulong u1 = stream.Read(4).ConvertLittleEndian();

            ulong value = stream.Read(4).ConvertLittleEndian();
            var item = new SlotItem((SlotType)(value & 15), (int)(value >> 4 & 15), (int)(value >> 8 & 65535));

            ulong count = stream.Read(2).ConvertLittleEndian();

            ulong rest = stream.Read(3).ConvertLittleEndian();

            data.Slot = new Slot(item, (int)count, (int)rest);
        }
    }
}