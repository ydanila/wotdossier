using System.Runtime.Serialization;

namespace WotDossier.Domain.Replay
{
    [DataContract]
    public class Avatar : ResultBase
    {
        [DataMember]
        public bool eligibleForCrystalRewards { get; set; }
    }
}