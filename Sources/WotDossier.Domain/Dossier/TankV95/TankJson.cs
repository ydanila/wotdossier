using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WotDossier.Domain.Dossier.TankV77;
using WotDossier.Domain.Dossier.TankV85;
using WotDossier.Domain.Dossier.TankV87;
using WotDossier.Domain.Dossier.TankV89;
using WotDossier.Domain.Dossier.TankV94;

namespace WotDossier.Domain.Dossier.TankV95
{
    /// <summary>
    /// 0.9.18
    /// </summary>
    public class TankJson95 : TankJson94
    {
        public new StatisticJson95 A15x15 { get; set; } = new StatisticJson95();

        public new StatisticJson95_2 A15x15_2 { get; set; } = new StatisticJson95_2();

        public new StatisticJson95 A7x7 { get; set; } = new StatisticJson95();

        public StatisticJson95 RatedA7X7 { get; set; } = new StatisticJson95();

        public new StatisticJson95 Historical { get; set; } = new StatisticJson95();

        public new StatisticJsonFort95 FortBattles { get; set; } = new StatisticJsonFort95();

        public new StatisticJsonFortSortie95 FortSorties { get; set; } = new StatisticJsonFortSortie95();

        public new StatisticJson95 Clan { get; set; } = new StatisticJson95();

        public new StatisticJson95_2 Clan2 { get; set; } = new StatisticJson95_2();

        public new StatisticJson95 Company { get; set; } = new StatisticJson95();

        public new StatisticJson95_2 Company2 { get; set; } = new StatisticJson95_2();

        public new StatisticFallout95 Fallout { get; set; } = new StatisticFallout95();

        public new StatisticJson95 GlobalMapCommon { get; set; } = new StatisticJson95();

        #region Achievements

        public new Achievements_87 Achievements { get; set; } = new Achievements_87();

        public new Achievements7x7_85 Achievements7X7 { get; set; } = new Achievements7x7_85();

        public new AchievementsSingle89 SingleAchievements { get; set; } = new AchievementsSingle89();

        public new AchievementsClan_85 ClanAchievements { get; set; } = new AchievementsClan_85();

        public new AchievementsHistorical_85 HistoricalAchievements { get; set; } = new AchievementsHistorical_85();

        public new AchievementsFort_87 FortAchievements { get; set; } = new AchievementsFort_87();

        public new AchievementsFallout94 FalloutAchievements { get; set; } = new AchievementsFallout94();

        #endregion

    }
}
