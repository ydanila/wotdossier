using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
namespace WotDossier.Applications.Parser
{
    public class Parser913 : Parser912
    {
        protected override ulong PacketChat
        {
            get { return 0x23; }
        }

        protected override ulong UpdateEvent_Slot
        {
            get { return 0x9; }
        }

        protected override ulong UpdateEvent_Arena
        {
            get { return 0x28; }
        }
    }
}