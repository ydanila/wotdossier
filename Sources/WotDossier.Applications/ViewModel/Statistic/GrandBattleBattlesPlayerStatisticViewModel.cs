using System.Collections.Generic;
using WotDossier.Applications.Logic;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Interfaces;

namespace WotDossier.Applications.ViewModel.Statistic
{
    public class GrandBattleBattlesPlayerStatisticViewModel : RandomBattlesPlayerStatisticViewModel, IRandomBattlesAchievements
    {
		public GrandBattleBattlesPlayerStatisticViewModel(GrandBattleBattlesStatisticEntity stat) : this(stat, new List<StatisticSlice>())
		{
		}

		public GrandBattleBattlesPlayerStatisticViewModel(GrandBattleBattlesStatisticEntity stat, List<StatisticSlice> list)
			: base(stat, list)
		{

		}
	}
}