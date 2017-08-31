using System.Runtime.Serialization;
using WotDossier.Domain.Interfaces;

namespace WotDossier.Domain.Entities
{
    /// <summary>
    ///     Object representation for table 'RandomBattlesAchievements'.
    /// </summary>
    [DataContract]
    public class RankedBattlesAchievementsEntity : EntityBase, IRankedBattlesAchievements, IRevised
    {
        /// <summary>
        ///     Gets/Sets the field "Warrior".
        /// </summary>
        [DataMember]
        public virtual int Season1Rank { get; set; }

    }
}