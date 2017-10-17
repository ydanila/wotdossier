using System;
using System.Collections.Generic;
using WotDossier.Applications.ViewModel.Statistic;
using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Tank;
using Mapper = WotDossier.Applications.Logic.Mapper;

namespace WotDossier.Applications.ViewModel.Rows
{
    public class FalloutBattlesTankStatisticRowViewModel : TankStatisticRowViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public FalloutBattlesTankStatisticRowViewModel(TankJson tank, IEnumerable<StatisticSlice> list)
            : base(tank, list)
        {
            #region Achievements

            Mapper.Map<IFalloutAchievements>(tank.AchievementsFallout ?? new AchievementsFallout(), this);

            #endregion
        }

        public override Func<TankJson, StatisticJson> Predicate
        {
            get { return tank => tank.Fallout ?? new StatisticJson(); }
        }

    }
}