using WotDossier.Dal;
using WotDossier.Domain;

namespace WotDossier.Applications.BattleModeStrategies
{
    public class StatisticViewStrategyManager
    {
        public static StatisticViewStrategyBase Get(BattleMode randomCompany, DossierRepository dossierRepository)
        {
            if (randomCompany == BattleMode.RandomCompany)
            {
                return new RandomStatisticViewStrategy(dossierRepository);
            }
            if (randomCompany == BattleMode.HistoricalBattle)
            {
                return new HistoricalStatisticViewStrategy(dossierRepository);
            }
            if (randomCompany == BattleMode.TeamBattle)
            {
                return new TeamStatisticViewStrategy(dossierRepository);
            }
	        if (randomCompany == BattleMode.TeamRated)
	        {
		        return new TeamRatedStatisticViewStrategy(dossierRepository);
	        }
			if (randomCompany == BattleMode.Clan)
            {
                return new ClanStatisticViewStrategy(dossierRepository);
            }

            if (randomCompany == BattleMode.FortBattles)
            {
                return new FortBattlesStatisticViewStrategy(dossierRepository);
            }

            if (randomCompany == BattleMode.FortSorties)
            {
                return new FortSortiesStatisticViewStrategy(dossierRepository);
            }

	        if (randomCompany == BattleMode.Fallout)
	        {
		        return new FalloutStatisticViewStrategy(dossierRepository);
	        }

	        if (randomCompany == BattleMode.GlobalMap)
	        {
		        return new GlobalMapStatisticViewStrategy(dossierRepository);
	        }

			if (randomCompany == BattleMode.Ranked)
	        {
		        return new RankedStatisticViewStrategy(dossierRepository);
	        }

			if (randomCompany == BattleMode.GrandBattle)
            {
                return new GrandBattleStatisticViewStrategy(dossierRepository);
            }
            return null;
        }
    }
}