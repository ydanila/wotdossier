using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WotDossier.Domain.Tank;

namespace WotDossier.Domain.Dossier
{
    public class DossierData
    {
        [DataMember]
        public DossierDataHeader header { get; } = new DossierDataHeader();

        [DataMember]
        public List<TankJson> tanks { get; } = new List<TankJson>();

        [DataMember]
        public List<TankJson> tanks_v2 { get; } = new List<TankJson>();
    }
}
