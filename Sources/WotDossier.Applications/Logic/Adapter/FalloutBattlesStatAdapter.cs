using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.Logic.Adapter
{
    public class FalloutBattlesStatAdapter : AbstractStatisticAdapter<RandomBattlesStatisticEntity>, IFalloutAchievements
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public FalloutBattlesStatAdapter(List<TankJson> tanks)
            : base(tanks)
        {
            var achievementsFallout = new AchievementsFallout();
            Func<TankJson, AchievementsFallout> fortAchievementsPredicate = tankJson => tankJson.AchievementsFallout ?? achievementsFallout;

	        ShoulderToShoulder = tanks.Sum(x => fortAchievementsPredicate(x).ShoulderToShoulder);
	        AloneInTheField = tanks.Sum(x => fortAchievementsPredicate(x).AloneInTheField);
	        FallenFlags = tanks.Sum(x => fortAchievementsPredicate(x).FallenFlags);
	        EffectiveSupport = tanks.Sum(x => fortAchievementsPredicate(x).EffectiveSupport);
	        StormLord = tanks.Sum(x => fortAchievementsPredicate(x).StormLord);
	        WinnerLaurels = tanks.Sum(x => fortAchievementsPredicate(x).WinnerLaurels);
	        Unreachable = tanks.Sum(x => fortAchievementsPredicate(x).Unreachable);
	        Champion = tanks.Sum(x => fortAchievementsPredicate(x).Champion);
	        Bannerman = tanks.Sum(x => fortAchievementsPredicate(x).Bannerman);
	        FalloutDieHard = tanks.Sum(x => fortAchievementsPredicate(x).FalloutDieHard);
		}

        public List<ITankStatisticRow> Tanks { get; set; }

		#region Achievements

		public int ShoulderToShoulder { get; set; }
	    public int AloneInTheField { get; set; }
	    public int FallenFlags { get; set; }
	    public int EffectiveSupport { get; set; }
	    public int StormLord { get; set; }
	    public int WinnerLaurels { get; set; }
	    public int Predator { get; set; }
	    public int Unreachable { get; set; }
	    public int Champion { get; set; }
	    public int Bannerman { get; set; }
	    public int FalloutDieHard { get; set; }

		#endregion

		public override void Update(RandomBattlesStatisticEntity entity)
        {
            base.Update(entity);

            if (entity.AchievementsIdObject == null)
            {
                entity.AchievementsIdObject = new RandomBattlesAchievementsEntity();
            }

            Mapper.Map<IFalloutAchievements>(this, entity.AchievementsIdObject);
        }

        public override Func<TankJson, StatisticJson> Predicate
        {
            get { return tank => tank.Fallout ?? new StatisticJson(); }
        }
    }
}