using System.Collections.Generic;
using WotDossier.Domain.Dossier.TankV77;
using WotDossier.Domain.Dossier.TankV85;
using WotDossier.Domain.Dossier.TankV87;
using WotDossier.Domain.Dossier.TankV92;
using WotDossier.Domain.Dossier.TankV94;
using WotDossier.Domain.Dossier.TankV95;
using WotDossier.Domain.Dossier.TankV96;

namespace WotDossier.Domain.Dossier.TankV97
{
    /// <summary>
    /// 0.9.19.0.2
    /// </summary>
    public class TankJson97 : TankJson96
    {
	    public TankJson97()
	    {
		    FortBattles = new StatisticJsonFort97();
		    FortSorties = new StatisticJsonFortSortie97();
	    }

		public StatisticJson95 Ranked { get; set; } = new StatisticJson95();

        public MaxJson77 MaxRanked { get; set; } = new MaxJson77();

        public new StatisticJsonFort97 FortBattles
	    {
		    get => (StatisticJsonFort97)base.FortBattles;
		    set => base.FortBattles = value;
	    }

	    public new StatisticJsonFortSortie97 FortSorties
	    {
		    get => (StatisticJsonFortSortie97)base.FortSorties;
		    set => base.FortSorties = value;
	    }

	}
}
