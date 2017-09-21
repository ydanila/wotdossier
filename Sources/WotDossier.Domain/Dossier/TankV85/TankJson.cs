using System.Collections.Generic;
using WotDossier.Domain.Dossier.TankV77;

namespace WotDossier.Domain.Dossier.TankV85
{
    /*0.9.2*/
    public class TankJson85
    {
	    public CommonJson77 Common { get; set; } = new CommonJson77();

	    public StatisticJson77 A15x15 { get; set; } = new StatisticJson77();

	    public StatisticJson77_2 A15x15_2 { get; set; } = new StatisticJson77_2();

	    public MaxJson77 Max15x15 { get; set; } = new MaxJson77();

	    public StatisticJson77 A7x7 { get; set; } = new StatisticJson77();

	    public MaxJson77 Max7x7 { get; set; } = new MaxJson77();

	    public StatisticJson77 Historical { get; set; } = new StatisticJson77();

	    public StatisticJson77 FortBattles { get; set; } = new StatisticJson77();

	    public StatisticJson77 FortSorties { get; set; } = new StatisticJson77();

	    public MaxJson77 MaxHistorical { get; set; } = new MaxJson77();

	    public StatisticJson77 Clan { get; set; } = new StatisticJson77();

	    public StatisticJson77_2 Clan2 { get; set; } = new StatisticJson77_2();

	    public StatisticJson77 Company { get; set; } = new StatisticJson77();

	    public StatisticJson77_2 Company2 { get; set; } = new StatisticJson77_2();

	    public TotalJson77 Total { get; set; } = new TotalJson77();

	    #region Achievements

	    public Achievements_85 Achievements { get; set; } = new Achievements_85();

	    public Achievements7x7_85 Achievements7X7 { get; set; } = new Achievements7x7_85();

	    public AchievementsSingle_85 SingleAchievements { get; set; } = new AchievementsSingle_85();

	    public AchievementsClan_85 ClanAchievements { get; set; } = new AchievementsClan_85();

	    public AchievementsHistorical_85 HistoricalAchievements { get; set; } = new AchievementsHistorical_85();

	    public AchievementsFort_85 FortAchievements { get; set; } = new AchievementsFort_85();

	    #endregion

	    public IList<IList<string>> FragsList { get; set; } = new List<IList<string>>();

	    public MaxJson77 MaxFort { get; set; } = new MaxJson77();

	    public MaxJson77 MaxSorties { get; set; } = new MaxJson77();

	    private int _uniqueId = -1;
        public int UniqueId()
        {
            if (_uniqueId == -1)
            {
                _uniqueId = DossierUtils.ToUniqueId(Common.countryid, Common.tankid);
            }
            return _uniqueId;
        }



        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}", Common.tanktitle);
        }
    }
}
