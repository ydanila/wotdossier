using WotDossier.Domain.Entities;

namespace WotDossier.Dal.Mappings
{
    /// <summary>
    /// Represents map class for <see cref="RankedBattlesAchievementsEntity"/>.
    /// </summary>
    public class RankedBattlesAchievementsEntityMapping : ClassMapBase<RankedBattlesAchievementsEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RankedBattlesAchievementsEntityMapping"/> class.
        /// </summary>
        public RankedBattlesAchievementsEntityMapping()
        {
            Map(v => v.UId);

            Map(v => v.Rev);
        }
    }
}
