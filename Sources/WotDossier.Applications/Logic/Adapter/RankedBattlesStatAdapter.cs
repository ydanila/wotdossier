using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Server;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.Logic.Adapter
{
    public class RankedBattlesStatAdapter : AbstractStatisticAdapter<RankedBattlesStatisticEntity>, IRankedBattlesAchievements
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public RankedBattlesStatAdapter(List<TankJson> tanks) : base(tanks)
        {
            Func<TankJson, AchievementsRanked> achievementsRankedPredicate = tankJson => tankJson.AchievementsRanked ?? new AchievementsRanked();

            #region [ Awards ]

            Rank = tanks.Sum(x => achievementsRankedPredicate(x).Rank);

            #endregion
        }

        public List<Vehicle> Vehicle { get; set; }

        #region Achievments

        public int Rank { get; set; }
        #endregion

        public override void Update(RankedBattlesStatisticEntity entity)
        {
            base.Update(entity);

            if (entity.AchievementsIdObject == null)
            {
                entity.AchievementsIdObject = new RankedBattlesAchievementsEntity { UId = Guid.NewGuid() };
                entity.AchievementsUId = entity.AchievementsIdObject.UId;
            }

            Mapper.Map<IRankedBattlesAchievements>(this, entity.AchievementsIdObject);
        }

        public override Func<TankJson, StatisticJson> Predicate
        {
            get { return tank => tank.Ranked ?? new StatisticJson(); }
        }
    }
}