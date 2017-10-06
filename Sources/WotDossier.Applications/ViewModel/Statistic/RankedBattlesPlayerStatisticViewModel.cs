using System.Collections.Generic;
using WotDossier.Applications.Logic;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Interfaces;

namespace WotDossier.Applications.ViewModel.Statistic
{
    public class RankedBattlesPlayerStatisticViewModel : PlayerStatisticViewModel, IRankedBattlesAchievements
    {
        public RankedBattlesPlayerStatisticViewModel(RankedBattlesStatisticEntity stat)
            : this(stat, new List<StatisticSlice>())
        {
        }

        public RankedBattlesPlayerStatisticViewModel(RankedBattlesStatisticEntity stat, List<StatisticSlice> list)
            : base(stat, list)
        {
            #region Awards

            if (stat.AchievementsIdObject != null)
            {
                Mapper.Map<IRankedBattlesAchievements>(stat.AchievementsIdObject, this);
            }

            #endregion
        }
    }
}