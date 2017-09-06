using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using WotDossier.Common;
using WotDossier.Common.Extensions;
using WotDossier.Domain.Replay;

namespace WotDossier.Applications.Parser
{
    public class Parser98 : BaseParser
    {
        private bool RosterProcessed = false;

        protected override ulong UpdateEvent_Slot
        {
            get { return 0x09; }
        }

        protected override ulong UpdateEvent_Arena
        {
            get { return 0x22; }
        }

        protected override byte[] ProcessPacketStateArenaUpdateBase(Packet packet, dynamic data, Stream stream)
        {
            ulong updateType = data.updateType;
            //First packet of this subtype contains pickled roster. BUT compressed using zlib compression. 
            if (updateType == 0x01 || updateType == 0x04)
            {
                var offset = 17;
                stream.Seek(offset, SeekOrigin.Begin);
            }
            else
            {
                stream.Seek(1, SeekOrigin.Current);
                packet.SubTypePayloadLength = packet.SubTypePayloadLength - 1;
            }

            //Read from your offset to the end of the packet, this will be the "update pickle". 
            byte[] updatePayload = stream.Read((int)(packet.SubTypePayloadLength));

            if (!RosterProcessed)
            {
                updatePayload = DecompressData(updatePayload);
                RosterProcessed = true;
            }
            return updatePayload;
        }

        protected override void ProcessPacketStateArenaUpdatePeriod(dynamic data, byte[] updatePayload)
        {
            try
            {
                var decompressData = DecompressData(updatePayload);

                using (var updatePayloadStream = new MemoryStream(decompressData))
                {
                    object[] update = (object[])Unpickle.Load(updatePayloadStream);
                    data.period = update[0];
                    data.period_end = update[1];
                    data.period_length = Convert.ToInt32(update[2]);
                    data.activities = update[3];
                }
            }
            catch (Exception e)
            {
                _log.Error("Error on update load", e);
            }
        }
    }
}