using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Applications.Logic.Adapter;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Applications.ViewModel.Statistic;
using WotDossier.Dal;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Server;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.BattleModeStrategies
{
    public class TeamRatedStatisticViewStrategy : StatisticViewStrategyBase
    {
        private static TeamBattlesStatisticEntity _currentSnapshot;
        private static IEnumerable<TankTeamBattleStatisticEntity> _tanks;

        /// <summary>
        /// Predicate to get tank statistic
        /// </summary>
        public override Func<TankJson, StatisticJson> Predicate
        {
            get { return tank => tank.Rated7x7 ?? new StatisticJson(); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomStatisticViewStrategy"/> class.
        /// </summary>
        /// <param name="dossierRepository">The dossier repository.</param>
        public TeamRatedStatisticViewStrategy(DossierRepository dossierRepository)
            : base(dossierRepository)
        {
        }

        /// <summary>
        /// Gets the player statistic.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="tanks">The tanks.</param>
        /// <param name="playerData">The player data.</param>
        /// <returns></returns>
        public override PlayerStatisticViewModel GetPlayerStatistic(PlayerEntity player, List<TankJson> tanks, Player playerData = null)
        {
            var statisticEntities = new List<TeamBattlesStatisticEntity> { _currentSnapshot };

            var currentStatistic = statisticEntities.OrderByDescending(x => x.BattlesCount).First();
            List<StatisticSlice> oldStatisticEntities = statisticEntities.Where(x => x.Id != currentStatistic.Id)
                .Select(x => ToViewModel(x).ToStatisticSlice()).ToList();

            PlayerStatisticViewModel currentStatisticViewModel = ToViewModel(currentStatistic, oldStatisticEntities);
            currentStatisticViewModel.Name = player.Name;
            currentStatisticViewModel.Created = player.Creaded;
            currentStatisticViewModel.AccountId = player.AccountId;
            var days = (DateTime.Now - player.Creaded).Days;
            currentStatisticViewModel.BattlesPerDay = currentStatisticViewModel.BattlesCount / (days == 0 ? 1 : days);
            currentStatisticViewModel.PlayTime = new TimeSpan(0, 0, 0, tanks.Sum(x => x.Common.battleLifeTime));

            return currentStatisticViewModel;
        }

        /// <summary>
        /// To the tank statistic row.
        /// </summary>
        /// <param name="currentStatistic">The current statistic.</param>
        /// <param name="prevStatisticViewModels">The previous statistic view models.</param>
        /// <returns></returns>
        protected override ITankStatisticRow ToTankStatisticRow(TankJson currentStatistic, List<StatisticSlice> prevStatisticViewModels)
        {
            return new TeamRatedBattlesTankStatisticRowViewModel(currentStatistic, prevStatisticViewModels);
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="currentStatistic">The current statistic.</param>
        /// <returns></returns>
        protected override PlayerStatisticViewModel ToViewModel(StatisticEntity currentStatistic)
        {
            return new TeamRatedBattlesPlayerStatisticViewModel((TeamBattlesStatisticEntity)currentStatistic);
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="currentStatistic">The current statistic.</param>
        /// <param name="oldStatisticEntities">The old statistic entities.</param>
        /// <returns></returns>
        protected override PlayerStatisticViewModel ToViewModel(StatisticEntity currentStatistic, List<StatisticSlice> oldStatisticEntities)
        {
            return new TeamRatedBattlesPlayerStatisticViewModel((TeamBattlesStatisticEntity)currentStatistic, oldStatisticEntities);
        }

        /// <summary>
        /// Updates the tank statistic.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="tanks">The tanks.</param>
        /// <returns></returns>
        public override PlayerEntity UpdateTankStatistic(int playerId, List<TankJson> tanks)
        {
            _tanks = tanks.Select(x =>
                new TankTeamBattleStatisticEntity
				{
                    Updated = x.Common.lastBattleTimeR,
                    Version = x.Common.basedonversion,
                    Raw = x.Raw,
                    BattlesCount = Predicate(x).battlesCount,
                    TankId = x.UniqueId(),
                    TankIdObject = new TankEntity
                    {
                        CountryId = x.Common.countryid,
                        TankId = x.Common.tankid,
                        Icon = x.Description.IconId,
                        PlayerId = playerId,
                        IsPremium = x.Common.premium == 1,
                        Name = x.Common.tanktitle,
                        TankType = x.Common.type,
                        Tier = x.Common.tier,
                        UniqueId = x.UniqueId()
                    }
                }).ToList();
            
            return DossierRepository.GetPlayer(playerId);
        }

        /// <summary>
        /// Gets the tanks statistic.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <returns></returns>
        public override List<ITankStatisticRow> GetTanksStatistic(int playerId)
        {
            return _tanks.GroupBy(x => x.TankId).Select(x => ToTankStatisticRow(x, Predicate)).OrderByDescending(x => x.Tier).ThenBy(x => x.TankDescription).Where(x => x.BattlesCount > 0).ToList();
        }

        /// <summary>
        /// Updates the player statistic.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="tanks">The tanks.</param>
        /// <param name="serverStatistic">The server statistic.</param>
        /// <returns></returns>
        public override PlayerEntity UpdatePlayerStatistic(int playerId, List<TankJson> tanks, Player serverStatistic)
        {
            PlayerEntity playerEntity = DossierRepository.GetPlayer(playerId);

            _currentSnapshot = new TeamBattlesStatisticEntity { PlayerId = playerEntity.Id };

            TeamRatedBattlesStatAdapter newSnapshot = new TeamRatedBattlesStatAdapter(tanks);

            newSnapshot.Update(_currentSnapshot);

            return playerEntity;
        }
    }
}