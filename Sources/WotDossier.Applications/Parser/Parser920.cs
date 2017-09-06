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

	    protected override ulong UpdateEvent_Slot => 0x2d;

	}
}