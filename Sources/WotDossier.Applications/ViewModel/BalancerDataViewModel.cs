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

namespace WotDossier.Applications.ViewModel
{
    public class BalancerDataViewModel : INotifyPropertyChanged
    {
        private static readonly ILog _log = LogManager.GetLogger<BalancerDataViewModel>();

        private bool _processing;
        private List<ReplayFolder> _replaysFolders;
        private List<ReplayFile> _replays = new List<ReplayFile>();
        private IOrderedEnumerable<TankBalanceViewModel> _tanksStat;

        public ReplaysManager ReplaysManager { get; set; }

        public DossierRepository DossierRepository { get; set; }

        public ProgressControlViewModel ProgressView { get; set; }

        public IOrderedEnumerable<TankBalanceViewModel> TanksStat
        {
            get { return _tanksStat; }
            set
            {
                _tanksStat = value;
                OnPropertyChanged(nameof(TanksStat));
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
            var tanks = new List<TankBalanceViewModel>();
            try
            {
                ReplaysViewModel.PrepareReplays(reporter, _log, DossierRepository, _replaysFolders, replayFolders,
                    _replays);

                //  now groups
                var groupByTank = from r in _replays group r by r.TankName;

                foreach (var entries in groupByTank)
                {
                    var first = entries.First();

                    var tanksEntry = new TankBalanceViewModel
                    {
                        Tier = first.Tank.Tier,
                        CountryId = first.Tank.CountryId,
                        Icon = first.Tank.Icon,
                        Tank = first.TankName
                    };

                    foreach (var entry in entries)
                    {
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

                        var tier = entry.Tank.Tier;
                        var min = tier;
                        var max = tier;

                        //  count range - enough to count one set
                        foreach (var member in entry.TeamMembers.Where(m => m.team == entry.Team))
                        {
                            var description = Dictionaries.Instance.GetReplayTankDescription(member.vehicleType,
                                entry.ClientVersion);

                            if (description.Title.Equals(TankDescription.UNKNOWN) || description.LevelRange == null)
                            {
                                continue;
                            }

                            min = min > description.LevelRange.Min ? description.LevelRange.Min : min;
                            max = max < description.LevelRange.Max ? description.LevelRange.Max : max;
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
                        else if (max - tier >= 2)
                        {
                            tanksEntry.Bottom357++;
                        }
                    }

                    tanks.Add(tanksEntry);
                }

                TanksStat = from t in tanks orderby t.BattlesCount descending select t;
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}