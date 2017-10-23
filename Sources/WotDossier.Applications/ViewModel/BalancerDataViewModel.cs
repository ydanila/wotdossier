using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Common.Logging;
using WotDossier.Applications.Annotations;
using WotDossier.Applications.Logic;
using WotDossier.Applications.ViewModel.Balancer;
using WotDossier.Applications.ViewModel.Replay;
using WotDossier.Dal;
using WotDossier.Domain.Tank;
using WotDossier.Framework.Forms.ProgressDialog;
using WotDossier.Applications.ViewModel.Filter;
using WotDossier.Domain.Replay;
using WotDossier.Domain;

namespace WotDossier.Applications.ViewModel
{
    public class BalancerDataViewModel : INotifyPropertyChanged
    {
        private static readonly ILog _log = LogManager.GetLogger<BalancerDataViewModel>();

        private bool _processing;
        private List<ReplayFolder> _replaysFolders;
        private List<ReplayFile> _replays = new List<ReplayFile>();

        private IEnumerable<TankBalanceViewModel> _tanksStat;
        private IEnumerable<TankBalanceViewModel> _tanksSummary;

        private IEnumerable<TierBalanceViewModel> _tiersStat;
        private IEnumerable<TierBalanceViewModel> _tiersSummary;

        private IEnumerable<TypeBalanceViewModel> _typesStat;
        private IEnumerable<TypeBalanceViewModel> _typesSummary;

        public ReplaysManager ReplaysManager { get; set; }

        public DossierRepository DossierRepository { get; set; }

        public ProgressControlViewModel ProgressView { get; set; }

        public IEnumerable<TankBalanceViewModel> TanksStat
        {
            get { return _tanksStat; }
            set
            {
                _tanksStat = value;
                OnPropertyChanged(nameof(TanksStat));
            }
        }

        public IEnumerable<TankBalanceViewModel> TanksSummary
        {
            get { return _tanksSummary; }
            set
            {
                _tanksSummary = value;
                OnPropertyChanged(nameof(TanksSummary));
            }
        }

        public IEnumerable<TierBalanceViewModel> TiersStat
        {
            get { return _tiersStat; }
            set
            {
                _tiersStat = value;
                OnPropertyChanged(nameof(TiersStat));
            }
        }

        public IEnumerable<TierBalanceViewModel> TiersSummary
        {
            get { return _tiersSummary; }
            set
            {
                _tiersSummary = value;
                OnPropertyChanged(nameof(TiersSummary));
            }
        }

        public IEnumerable<TypeBalanceViewModel> TypesStat
        {
            get { return _typesStat; }
            set
            {
                _typesStat = value;
                OnPropertyChanged(nameof(TypesStat));
            }
        }

        public IEnumerable<TypeBalanceViewModel> TypesSummary
        {
            get { return _typesSummary; }
            set
            {
                _typesSummary = value;
                OnPropertyChanged(nameof(TypesSummary));
            }
        }

        public BalancerDataViewModel(DossierRepository dossierRepository, ProgressControlViewModel progressControlView)
        {
            ReplaysManager = new ReplaysManager();
            DossierRepository = dossierRepository;
            ProgressView = progressControlView;
        }
        
        public void LoadReplaysList()
        {
            if(_processing)
            {
                return;
            }

            _processing = true;

            if(_replaysFolders == null)
            {
                _replaysFolders = ReplaysManager.GetFolders();
            }

            if(!_replays.Any())
            {
                _replays = ReplaysViewModel.LoadFromCache().ToList();
            }

            List<ReplayFolder> replayFolders = _replaysFolders.GetAll();

            ProcessReplaysFoldersInBackground(replayFolders);
        }

        private void ProcessReplaysFoldersInBackground(List<ReplayFolder> replayFolders)
        {
            ProgressView.Execute(
                Resources.Resources.ProgressTitle_Loading_replays, (bw, we) =>
                {
                    var reporter = new Reporter(bw, we, ProgressView);
                    ProcessReplaysFolders(replayFolders, reporter);
                }, new ProgressDialogSettings(true, true, false));
        }

        public void ProcessReplaysFolders(List<ReplayFolder> replayFolders, IReporter reporter)
        {            
            try
            {
                //ReplaysViewModel.PrepareReplays(reporter, _log, DossierRepository, _replaysFolders, replayFolders, _replays);
                ReplaysViewModel.PrepareReplays(reporter, _log, _replaysFolders, replayFolders, _replays);

                //  tanks
                var groupByTank = from r in _replays group r by r.TankName;
                var tanks = new List<TankBalanceViewModel>();
                var tanksSummary = new TankBalanceViewModel();
                ProcessBalancerGroups(groupByTank, tanks, tanksSummary, CreateTankBalanceViewModel);
                TanksStat = from t in tanks where t.BattlesCount > 0 orderby t.BattlesCount descending select t;
                TanksSummary = new List<TankBalanceViewModel>() { tanksSummary };

                //  tiers
                var groupByTier = from r in _replays group r by r.TankDescription.Tier;
                var tiers = new List<TierBalanceViewModel>();
                var tiersSummary = new TierBalanceViewModel();
                ProcessBalancerGroups(groupByTier, tiers, tiersSummary, CreateTierBalanceViewModel);
                TiersStat = from t in tiers where t.BattlesCount > 0 orderby t.Tier descending select t;
                TiersSummary = new List<TierBalanceViewModel>() { tiersSummary };

                //  types
                var groupByType = from r in _replays group r by r.TankDescription.Type;
                var types = new List<TypeBalanceViewModel>();
                var typesSummary = new TypeBalanceViewModel();
                ProcessBalancerGroups(groupByType, types, typesSummary, CreateTypeBalanceViewModel);
                TypesStat = from t in types where t.BattlesCount > 0 orderby t.BattlesCount descending select t;
                TypesSummary = new List<TypeBalanceViewModel>() { typesSummary };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _processing = false;
            }
        }
        
        private static void ProcessBalancerGroups<TKey, TView>(IEnumerable<IGrouping<TKey, ReplayFile>> groups, List<TView> tanks, TView tanksSummary, Func<ReplayFile, TView> createBalanceViewModel) where TView : BaseBalanceViewModel
        {
            foreach (var entries in groups)
            {
                var first = entries.First();
                TView tanksEntry = createBalanceViewModel(first);

                foreach (var entry in entries)
                {
                    //  ignore not regular
                    if (!ReplaysFilterViewModel.CheckRegularBattle(entry, BattleType.ctf) &&
                        !ReplaysFilterViewModel.CheckRegularBattle(entry, BattleType.Ctf30x30) &&
                        !ReplaysFilterViewModel.CheckRegularBattle(entry, BattleType.domination) &&
                        !ReplaysFilterViewModel.CheckRegularBattle(entry, BattleType.assault) &&
                        !ReplaysFilterViewModel.CheckRegularBattle(entry, BattleType.nations))
                    {
                        continue;
                    }

                    tanksEntry.BattlesCount++;
                    switch (entry.IsWinner)
                    {
                        case BattleStatus.Victory:
                            tanksEntry.Victory++;
                            break;

                        case BattleStatus.Defeat:
                            tanksEntry.Defeat++;
                            break;

                        case BattleStatus.Draw:
                            tanksEntry.Draw++;
                            break;
                    }

                    var tier = entry.TankDescription.Tier;
                    var min = tier;
                    var max = tier;

                    //  count range - enough to count one set
                    foreach (var member in entry.TeamMembers.Where(m => m.team == entry.Team))
                    {
                        var description = Dictionaries.Instance.GetReplayTankDescription(member.vehicleType,
                            entry.ClientVersion);

                        if (description.Title.Equals(TankDescription.UNKNOWN))
                        {
                            continue;
                        }

                        min = min > description.Tier ? description.Tier : min;
                        max = max < description.Tier ? description.Tier : max;
                    }

                    //  top 3-5-7
                    if (tier - min >= 2)
                    {
                        tanksEntry.Top357++;
                    }
                    else if (tier - min == 1 && max == tier)
                    {
                        tanksEntry.Top510++;
                    }
                    else if (tier == min && max == tier)
                    {
                        tanksEntry.OneLevel++;
                    }
                    else if (max - tier == 1 && min == tier)
                    {
                        tanksEntry.Bottom510++;
                    }
                    else if (max > tier && tier > min && max - min >= 2)
                    {
                        tanksEntry.Middle357++;
                    }
                    else if (max > tier && tier == min && max - min >= 2)
                    {
                        tanksEntry.Bottom357++;
                    }
                }

                tanksSummary.Add(tanksEntry);
                tanks.Add(tanksEntry);
            }
        }

        private static TankBalanceViewModel CreateTankBalanceViewModel(ReplayFile replay)
        {
            return new TankBalanceViewModel
            {
                Tier = replay.TankDescription.Tier,
                CountryId = replay.TankDescription.CountryId,
                IconId = replay.TankDescription.IconId,
                Tank = replay.TankName,
            };
        }

        private TierBalanceViewModel CreateTierBalanceViewModel(ReplayFile replay)

        {
            return new TierBalanceViewModel
            {
                Tier = replay.TankDescription.Tier
            };
        }

        private TypeBalanceViewModel CreateTypeBalanceViewModel(ReplayFile replay)
        {
            return new TypeBalanceViewModel
            {
                Type = (TankType)replay.TankDescription.Type
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}