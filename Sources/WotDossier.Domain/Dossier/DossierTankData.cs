using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WotDossier.Domain.Dossier
{
    public class DossierTankData
    {
        public Dictionary<string, Dictionary<string, int>> StaticSize { get; } = new Dictionary<string, Dictionary<string, int>>();
        public Dictionary<string, List<object>> List { get; } = new Dictionary<string, List<object>>();

        public Dictionary<string, Dictionary<object, object>> Dict { get; } = new Dictionary<string, Dictionary<object, object>>();

        public Dictionary<string, byte[]> Binary { get; } = new Dictionary<string, byte[]>();

        //public Dictionary<int, int> frags { get; } = new Dictionary<int, int>();
        public int compDescr { get; set; }
        public int basedonversion { get; set; }
        public int updated { get; set; }
        //public int frags_compare { get; set; }
    }
}
