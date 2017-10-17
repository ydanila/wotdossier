using System.Collections.Generic;
using WotDossier.Applications.Logic;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Interfaces;

namespace WotDossier.Applications.ViewModel.Statistic
{
    public class TeamRatedBattlesPlayerStatisticViewModel : PlayerStatisticViewModel, ITeamRatedBattlesAchievements
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TeamBattlesPlayerStatisticViewModel"/> class.
        /// </summary>
        /// <param name="stat">The stat.</param>
        public TeamRatedBattlesPlayerStatisticViewModel(TeamBattlesStatisticEntity stat)
            : this(stat, new List<StatisticSlice>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamBattlesPlayerStatisticViewModel"/> class.
        /// </summary>
        /// <param name="stat">The stat.</param>
        /// <param name="list">The list.</param>
        public TeamRatedBattlesPlayerStatisticViewModel(TeamBattlesStatisticEntity stat, List<StatisticSlice> list)
            : base(stat, list)
        {
            #region Awards

            if (stat.AchievementsIdObject != null)
            {
                Mapper.Map<ITeamRatedBattlesAchievements>(stat.AchievementsIdObject, this);
            }

            #endregion
        }
    }
}