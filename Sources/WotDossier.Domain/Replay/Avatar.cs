using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WotDossier.Domain.Replay
{
    [DataContract]
    public class Avatar : ResultBase
    {
        [DataMember]
        public bool eligibleForCrystalRewards { get; set; }
	    [DataMember]
	    public List<int> fairplayViolations { get; set; } = new List<int>();
	}
}