using System.Collections.Generic;
using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel.Rows
{
    public interface ITankStatisticRow : IStatisticBase, IStatisticExtended, IStatisticRatings, IRandomBattlesAchievements, ITeamBattlesAchievements, ITeamRatedBattlesAchievements, IHistoricalBattlesAchievements, IClanBattlesAchievements, IFalloutAchievements, IFortAchievements, ITankFilterable, ITankDescription, IRankedBattlesAchievements
    {
        int PlayerId { get; set; }
        string PlayerName { get; set; }

		int MarksOnGunSort { get; set; }

        IEnumerable<FragsJson> TankFrags { get; set; }

        IEnumerable<ITankStatisticRow> GetAll();

        void SetPreviousStatistic(ITankStatisticRow model);

        ITankStatisticRow PreviousStatistic { get; }
    }
}