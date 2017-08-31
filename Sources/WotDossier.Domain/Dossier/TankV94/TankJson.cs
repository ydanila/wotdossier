using System.Collections.Generic;
using WotDossier.Domain.Dossier.TankV77;
using WotDossier.Domain.Dossier.TankV85;
using WotDossier.Domain.Dossier.TankV87;
using WotDossier.Domain.Dossier.TankV92;

namespace WotDossier.Domain.Dossier.TankV94
{
    /// <summary>
    /// 0.9.17
    /// </summary>
    public class TankJson94 : TankJson92
    {
        public StatisticFallout94 Fallout { get; set; } = new StatisticFallout94();

        public MaxJsonFallout94 MaxJsonFallout { get; set; } = new MaxJsonFallout94();

        #region Achievements

        public AchievementsFallout94 FalloutAchievements { get; set; } = new AchievementsFallout94();

        #endregion

        
    }
}
