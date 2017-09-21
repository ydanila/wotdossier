using System.Runtime.Serialization;

namespace WotDossier.Domain.Entities
{
    /// <summary>
    ///     Object representation for table 'RankedBattlesStatistic'.
    /// </summary>

    [DataContract]
    public class RankedBattlesStatisticEntity : StatisticEntity
    {
        /// <summary>
        ///     Gets/Sets the <see cref="RandomBattlesAchievementsEntity" /> object.
        /// </summary>
        [DataMember(Name = "Achievements")]
        public virtual RankedBattlesAchievementsEntity AchievementsIdObject { get; set; }
    }
}