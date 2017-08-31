using System.Collections.Generic;
using WotDossier.Domain.Dossier.TankV77;
using WotDossier.Domain.Dossier.TankV85;
using WotDossier.Domain.Dossier.TankV87;

namespace WotDossier.Domain.Dossier.TankV88
{
    public class TankJson88 : TankJson87
    {
        public StatisticJson77 RatedA7x7 { get; set; } = new StatisticJson77();

        public MaxJson77 MaxRated7x7 { get; set; } = new MaxJson77();

        #region Achievements

        public new AchievementsSingle88 SingleAchievements { get; set; } = new AchievementsSingle88();

        #endregion
    }
}
