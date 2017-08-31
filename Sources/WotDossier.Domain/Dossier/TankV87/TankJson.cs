using System.Collections.Generic;
using WotDossier.Domain.Dossier.TankV77;
using WotDossier.Domain.Dossier.TankV85;

namespace WotDossier.Domain.Dossier.TankV87
{
    /*0.9.2*/
    public class TankJson87 : TankJson85
    {
        #region Achievements

        public new Achievements_87 Achievements { get; set; } = new Achievements_87();

        public new AchievementsFort_87 FortAchievements { get; set; } = new AchievementsFort_87();

        #endregion
    }
}
