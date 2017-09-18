using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Common.Logging;
using WotDossier.Applications.Annotations;
using WotDossier.Applications.Logic;
using WotDossier.Applications.ViewModel.Replay;
using WotDossier.Dal;
using WotDossier.Framework.Forms.ProgressDialog;

namespace WotDossier.Applications.ViewModel
{
    public class BalancerDataViewModel : INotifyPropertyChanged
    {
        private static readonly ILog _log = LogManager.GetLogger<BalancerDataViewModel>();

        private bool _processing;
        private List<ReplayFile> _replays;

        public ReplaysManager ReplaysManager { get; set; }

        public DossierRepository DossierRepository { get; set; }

        public ProgressControlViewModel ProgressView { get; set; }

        public List<ReplayFolder> ReplaysFolders { get; set; }

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

            if(ReplaysFolders == null)
            {
                ReplaysFolders = ReplaysManager.GetFolders();
            }

            if(!_replays.Any())
            {
                _replays = ReplaysViewModel.LoadFromCache().ToList();
            }

            List<ReplayFolder> replayFolders = ReplaysFolders.GetAll();

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
                var root = ReplaysViewModel.PrepareReplays(reporter, _log, DossierRepository, ReplaysFolders, replayFolders, _replays);
                OnPropertyChanged(nameof(ReplaysFolders));
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