using System.Collections.Generic;
using WotDossier.Domain.Dossier.TankV77;
using WotDossier.Domain.Dossier.TankV85;
using WotDossier.Domain.Dossier.TankV87;

namespace WotDossier.Domain.Dossier.TankV89
{
    public class TankJson89 : TankJson87
    {
        #region Achievements
        public new AchievementsSingle89 SingleAchievements { get; set; } = new AchievementsSingle89();

        #endregion
    }
}
