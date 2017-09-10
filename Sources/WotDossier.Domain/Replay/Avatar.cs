using System.Runtime.Serialization;

namespace WotDossier.Domain.Replay
{
    [DataContract]
    public class Avatar : ResultBase
    {
        [DataMember]
        private bool eligibleForCrystalRewards { get; set; }
    }
}