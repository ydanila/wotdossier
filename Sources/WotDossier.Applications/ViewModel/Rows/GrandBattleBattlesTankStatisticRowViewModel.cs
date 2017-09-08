using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Applications.ViewModel.Statistic;
using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Tank;
using Mapper = WotDossier.Applications.Logic.Mapper;

namespace WotDossier.Applications.ViewModel.Rows
{
    public class GrandBattleBattlesTankStatisticRowViewModel : RandomBattlesTankStatisticRowViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public GrandBattleBattlesTankStatisticRowViewModel(TankJson tank, IEnumerable<StatisticSlice> list)
            : base(tank, list)
        {
        }

        public override Func<TankJson, StatisticJson> Predicate
        {
            get { return tank => tank.A30x30; }
        }
    }
}