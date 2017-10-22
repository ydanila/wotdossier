﻿using System.Collections.Generic;
using WotDossier.Applications.Logic;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Interfaces;

namespace WotDossier.Applications.ViewModel.Statistic
{
    /// <summary>
    /// 
    /// </summary>
    public class FalloutBattlesPlayerStatisticViewModel : PlayerStatisticViewModel, IFalloutAchievements
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FalloutBattlesPlayerStatisticViewModel"/> class.
        /// </summary>
        /// <param name="stat">The stat.</param>
        public FalloutBattlesPlayerStatisticViewModel(RandomBattlesStatisticEntity stat)
            : this(stat, new List<StatisticSlice>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FalloutBattlesPlayerStatisticViewModel"/> class.
        /// </summary>
        /// <param name="stat">The stat.</param>
        /// <param name="list">The list.</param>
        public FalloutBattlesPlayerStatisticViewModel(RandomBattlesStatisticEntity stat, List<StatisticSlice> list)
            : base(stat, list)
        {
            #region Achievements

            if (stat.AchievementsIdObject != null)
            {
                Mapper.Map<IFalloutAchievements>(stat.AchievementsIdObject, this);
            }

            #endregion
        }
    }
}