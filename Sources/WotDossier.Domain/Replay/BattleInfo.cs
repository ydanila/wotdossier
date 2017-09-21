using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WotDossier.Domain.Replay
{
    public class BattleInfo
    {
        public int battleLevel { get; set; }
        public long gameParamsRev { get; set; }
    }

    public class BattleInfo915 : BattleInfo
    {
        public long gameParamsRev { get; set; }
    }
}
