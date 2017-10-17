using System;
using System.Collections.Generic;
using WotDossier.Applications.Logic;
using WotDossier.Applications.ViewModel.Statistic;
using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel.Rows
{
    public class TeamRatedBattlesTankStatisticRowViewModel : TankStatisticRowViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public TeamRatedBattlesTankStatisticRowViewModel(TankJson tank, IEnumerable<StatisticSlice> list)
            : base(tank, list)
        {
            #region Achievements

            Mapper.Map<ITeamRatedBattlesAchievements>(tank.Achievements7x7 ?? new Achievements7x7(), this);

            #endregion

        }

        public override Func<TankJson, StatisticJson> Predicate
        {
            get { return tank => tank.Rated7x7 ?? new StatisticJson(); }
        }
    }
}