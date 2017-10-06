using System.Collections.Generic;
using WotDossier.Domain;

namespace WotDossier.Applications.ViewModel.Selectors
{
    public class BattleModeSelectorViewModel : Framework.Foundation.Model
    {
        private List<ListItem<BattleMode>> _battleModes = new List<ListItem<BattleMode>>
        {
            new ListItem<BattleMode>(BattleMode.RandomCompany, Resources.Strings.Profile.profile_dropdown_labels_random),
            new ListItem<BattleMode>(BattleMode.TeamBattle, Resources.Strings.Profile.profile_dropdown_labels_team), 
            new ListItem<BattleMode>(BattleMode.HistoricalBattle, Resources.Strings.Profile.profile_dropdown_labels_historical), 
            new ListItem<BattleMode>(BattleMode.Clan, Resources.Strings.Profile.profile_dropdown_labels_clan),
            new ListItem<BattleMode>(BattleMode.FortBattles, Resources.Strings.Profile.profile_dropdown_labels_fortifications_battles), 
            new ListItem<BattleMode>(BattleMode.FortSorties, Resources.Strings.Profile.profile_dropdown_labels_fortifications_sorties),
	        new ListItem<BattleMode>(BattleMode.TeamRated, Resources.Strings.Profile.profile_dropdown_labels_staticTeam),
	        new ListItem<BattleMode>(BattleMode.GlobalMap, Resources.Strings.Profile.profile_dropdown_labels_clan),
	        new ListItem<BattleMode>(BattleMode.Fallout, Resources.Strings.Profile.profile_dropdown_labels_fallout),
	        new ListItem<BattleMode>(BattleMode.Ranked, Resources.Strings.Profile.profile_dropdown_labels_ranked),
			new ListItem<BattleMode>(BattleMode.GrandBattle, Resources.Strings.Profile.profile_dropdown_labels_epicRandom),
        };
        
        public List<ListItem<BattleMode>> BattleModes
        {
            get { return _battleModes; }
            set { _battleModes = value; }
        }

        private BattleMode _battleMode;
        public BattleMode BattleMode
        {
            get { return _battleMode; }
            set
            {
                _battleMode = value;
                RaisePropertyChanged("BattleMode");
            }
        }
    }
}
