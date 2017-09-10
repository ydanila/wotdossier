using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Applications.ViewModel.Statistic;
using WotDossier.Common;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Rating;
using WotDossier.Domain.Server;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.Logic.Adapter
{
    public class GrandBattleBattlesStatAdapter : RandomBattlesStatAdapterBase<GrandBattleBattlesStatisticEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public GrandBattleBattlesStatAdapter(List<TankJson> tanks) : base(tanks)
        {
            
        }

        public GrandBattleBattlesStatAdapter(Player stat) : base(stat)
        {
            
        }

        public override Func<TankJson, StatisticJson> Predicate
        {
            get { return tank => tank.A30x30 ?? new StatisticJson(); }
        }
    }
}