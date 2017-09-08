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
	    public TankJson95()
	    {
			A15x15 = new StatisticJson95();
			A15x15_2 = new StatisticJson95_2();
			A7x7 = new StatisticJson95();
			Historical = new StatisticJson95();
			RatedA7X7 = new StatisticJson95();
			FortBattles = new StatisticJsonFort95();
			FortSorties = new StatisticJsonFortSortie95();
			Clan = new StatisticJson95();
			Clan2 = new StatisticJson95_2();
			Company = new StatisticJson95();
			Company2 = new StatisticJson95_2();
			Fallout = new StatisticFallout95();
			GlobalMapCommon = new StatisticJson95();
	    }

	    public new StatisticJson95 A15x15
		{
		    get => (StatisticJson95)base.A15x15;
		    set => base.A15x15 = value;
	    }

	    public new StatisticJson95_2 A15x15_2
		{
		    get => (StatisticJson95_2)base.A15x15_2;
		    set => base.A15x15_2 = value;
	    }

	    public new StatisticJson95 A7x7
		{
		    get => (StatisticJson95)base.A7x7;
		    set => base.A7x7 = value;
	    }

	    public new StatisticJson95 Historical
		{
		    get => (StatisticJson95)base.Historical;
		    set => base.Historical = value;
	    }


		public StatisticJson95 RatedA7X7 { get; set; } = new StatisticJson95();

	    public new StatisticJsonFort95 FortBattles
		{
		    get => (StatisticJsonFort95)base.FortBattles;
		    set => base.FortBattles = value;
	    }

	    public new StatisticJsonFortSortie95 FortSorties
		{
		    get => (StatisticJsonFortSortie95)base.FortSorties;
		    set => base.FortSorties = value;
	    }

	    public new StatisticJson95 Clan
		{
		    get => (StatisticJson95)base.Clan;
		    set => base.Clan = value;
	    }

	    public new StatisticJson95_2 Clan2
		{
		    get => (StatisticJson95_2)base.Clan2;
		    set => base.Clan2 = value;
	    }

	    public new StatisticJson95 Company
		{
		    get => (StatisticJson95)base.Company;
		    set => base.Company = value;
	    }

	    public new StatisticJson95_2 Company2
		{
		    get => (StatisticJson95_2)base.Company2;
		    set => base.Company2 = value;
	    }

	    public new StatisticFallout95 Fallout
		{
		    get => (StatisticFallout95)base.Fallout;
		    set => base.Fallout = value;
	    }

	    public new StatisticJson95 GlobalMapCommon
		{
		    get => (StatisticJson95)base.GlobalMapCommon;
		    set => base.GlobalMapCommon = value;
	    }
    }
}
