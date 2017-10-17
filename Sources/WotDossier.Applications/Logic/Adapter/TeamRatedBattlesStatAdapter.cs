using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.Logic.Adapter
{
    public class TeamRatedBattlesStatAdapter : AbstractStatisticAdapter<TeamBattlesStatisticEntity>, ITeamRatedBattlesAchievements
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public TeamRatedBattlesStatAdapter(List<TankJson> tanks)
            : base(tanks)
        {
            
		}

        public List<ITankStatisticRow> Tanks { get; set; }

		#region Achievements

		#endregion

		public override void Update(TeamBattlesStatisticEntity entity)
        {
            base.Update(entity);

            if (entity.AchievementsIdObject == null)
            {
                entity.AchievementsIdObject = new TeamBattlesAchievementsEntity();
            }

            Mapper.Map<ITeamRatedBattlesAchievements>(this, entity.AchievementsIdObject);
        }

        public override Func<TankJson, StatisticJson> Predicate
        {
            get { return tank => tank.Rated7x7 ?? new StatisticJson(); }
        }
    }
}