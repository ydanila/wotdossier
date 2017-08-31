using System.Collections.Generic;
using WotDossier.Domain.Dossier.TankV77;
using WotDossier.Domain.Dossier.TankV85;
using WotDossier.Domain.Dossier.TankV87;
using WotDossier.Domain.Dossier.TankV92;
using WotDossier.Domain.Dossier.TankV94;
using WotDossier.Domain.Dossier.TankV95;
using WotDossier.Domain.Dossier.TankV97;

namespace WotDossier.Domain.Dossier.TankV98
{
    /// <summary>
    /// 0.9.19.1.0
    /// </summary>
    public class TankJson98 : TankJson97
    {
        #region Achievements

        public new AchievementsFort98 FortAchievements { get; set; } = new AchievementsFort98();

        #endregion
    }
}
