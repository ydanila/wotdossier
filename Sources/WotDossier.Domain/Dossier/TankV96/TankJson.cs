using System.Collections.Generic;
using WotDossier.Domain.Dossier.TankV77;
using WotDossier.Domain.Dossier.TankV85;
using WotDossier.Domain.Dossier.TankV87;
using WotDossier.Domain.Dossier.TankV92;
using WotDossier.Domain.Dossier.TankV94;
using WotDossier.Domain.Dossier.TankV95;

namespace WotDossier.Domain.Dossier.TankV96
{
    /// <summary>
    /// 0.9.19.0.0
    /// </summary>
    public class TankJson96 : TankJson95
    {
		#region Achievements

		public AchievementsFallout96 FalloutAchievements { get; set; } = new AchievementsFallout96();

        #endregion
    }
}
