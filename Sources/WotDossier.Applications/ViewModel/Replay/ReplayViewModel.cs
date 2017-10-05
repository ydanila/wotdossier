using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using Common.Logging;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using WotDossier.Applications.Logic;
using WotDossier.Applications.View;
using WotDossier.Applications.ViewModel.Replay.Viewer;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Replay;
using WotDossier.Domain.Tank;
using WotDossier.Framework;
using WotDossier.Framework.Applications;
using WotDossier.Framework.Forms.Commands;
using WotDossier.Framework.Forms.ProgressDialog;

namespace WotDossier.Applications.ViewModel.Replay
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof (ReplayViewModel))]
    public class ReplayViewModel : ViewModel<IReplayView>
    {
        private static readonly ILog _log = LogManager.GetLogger<ReplayViewModel>();

        #region Fields and Properties

        public string Title { get; set; }

        public Domain.Replay.Replay Replay { get; set; }

        public ReplayFile ReplayFile { get; set; }

        public bool IsBase { get; set; }

        public bool IsPremium { get; set; }

        public List<CombatTarget> CombatEffects { get; set; }

        public List<TeamMember> FirstTeam { get; set; }

        public List<TeamMember> SecondTeam { get; set; }

        public TankIcon TankIcon { get; set; }

        public string Date { get; set; }

        public string FullName { get; set; }

        public string Tank { get; set; }

        public List<Medal> BattleMedals { get; set; }

        public List<Medal> AchievMedals { get; set; }

        public BattleStatus Status { get; set; }

        public int HEHits { get; set; }

        public int HEHitsReceived { get; set; }

        public int XpFactor { get; set; }
	    public bool XpFactorVisible => XpFactor > 1;

	    public string UserBattleTime { get; set; }

        public string BattleTime { get; set; }

        public string StartTime { get; set; }

        public string Mileage { get; set; }

        public int DroppedCapturePoints { get; set; }

        public int PotentialDamageReceived { get; set; }

        public int DamageBlockedByArmor { get; set; }

        public int CapturePoints { get; set; }

        public int DamageAssisted { get; set; }

        public int DamageAssistedTrack { get; set; }
        
        public int DamageAssistedRadio { get; set; }

        public int Spotted { get; set; }

	    public int SniperDamageDealt { get; set; }

		public int Killed { get; set; }

	    public bool IsAlive { get; set; }

		public int Damaged { get; set; }

        public string TDamage { get; set; }
	    public int TDamageSign { get; set; }

		public int ShotsReceived { get; set; }

        public int DamageDealt { get; set; }

        public int Pierced { get; set; }

        public int Hits { get; set; }

        public int Crits { get; set; }

        public int Shots { get; set; }

        public int CreditsContributionOut { get; set; }
        public int CreditsContributionOutPremium { get; set; }

        public int CreditsContributionIn { get; set; }
        public int CreditsContributionInPremium { get; set; }

	    public ObservableCollection<int> TotalCredits { get; } = new ObservableCollection<int>();
		public ObservableCollection<int> TotalCreditsPremium { get; } = new ObservableCollection<int>();

		public ObservableCollection<int> AutoEquipCost { get; set; }

        public ObservableCollection<int> AutoLoadCost { get; set; }

        public int AutoRepairCost { get; set; }

        public int EventCredits { get; set; }
	    public int BoosterCredits { get; set; }
	    public int OrderCredits { get; set; }
	    public int AchievementCredits { get; set; }

	    public ObservableCollection<int> FairplayViolationsCredits { get; set; }
	    public ObservableCollection<int> FairplayViolationsCreditsPremium { get; set; }

		public int EventCreditsPremium { get; set; }
	    public int BoosterCreditsPremium { get; set; }
	    public int OrderCreditsPremium { get; set; }
	    public int AchievementCreditsPremium { get; set; }

        public int BaseTotalXp { get; set; }
	    public int BaseTotalFreeXp { get; set; }
		public int BattleXp { get; set; }
	    public int BattleCredits { get; set; }
		public int Crystal { get; set; }

		public bool EligibleForCrystalRewards { get; set; }

		public int Xp { get; set; }
	    public int XpPenalty { get; set; }
		public int FreeXp { get; set; }

		public int PremiumTotalXp { get; set; }
	    public int PremiumTotalFreeXp { get; set; }
		public int PremiumXp { get; set; }
	    public int PremiumXpPenalty { get; set; }
		public int PremiumFreeXp { get; set; }

		//Xp, FreeXp, PremiumXp, PremiumFreeXp
	    public ObservableCollection<int> EventXp { get; } = new ObservableCollection<int>();
		public ObservableCollection<int> BoosterXp { get; } = new ObservableCollection<int>();
		public ObservableCollection<int> OrderXp { get; } = new ObservableCollection<int>();
		public ObservableCollection<int> AchievementXp { get; } = new ObservableCollection<int>();




		public int OriginalCredits { get; set; }
        public int OriginalCreditsPremium { get; set; }
	    public int Credits { get; set; }
	    public int CreditsPremium { get; set; }

		public FinishReason FinishReason { get; set; }

        public DeathReason DeathReason { get; set; }

        public int NoDamageDirectHitsReceived { get; set; }
        public int RickochetsReceived { get; set; }

        public int DamageAssistedStun { get; set; }

        public int StunNum { get; set; }
        public int PiercingsReceived { get; set; }

	    public string Raw { get; set; }

		public string HitsPenetrations => $"{Hits}/{Pierced}";

        public string DamagedDestroyed => $"{Damaged}/{Killed}";

        public string CaptureDefensePoints => $"{CapturePoints}/{DroppedCapturePoints}";

        public List<ChatMessage> ChatMessages { get; set; }

        private List<DeviceDescription> _devices;
        public List<DeviceDescription> Devices
        {
            get { return _devices; }
            set { _devices = value; }
        }

        private List<Slot> _consumables = new List<Slot>();
        public List<Slot> Consumables
        {
            get { return _consumables; }
            set { _consumables = value; }
        }

        private List<Slot> _shells = new List<Slot>();
        public List<Slot> Shells
        {
            get { return _shells; }
            set { _shells = value; }
        }

        private TeamMember _alienTeamMember;
        public TeamMember AlienTeamMember
        {
            get { return _alienTeamMember; }
            set
            {
                _alienTeamMember = value;
                RaisePropertyChanged("AlienTeamMember");
            }
        }
        
        private ReplayViewer _replayViewer;
        
        public IReplayMap MapDescription { get; private set; }

        private TeamMember _ourTeamMember;
        public TeamMember OurTeamMember
        {
            get { return _ourTeamMember; }
            set
            {
                _ourTeamMember = value;
                RaisePropertyChanged("OurTeamMember");
            }
        }

        private List<TeamMember> _teamMembers;
        private ProgressControlViewModel _simulationWorker;

        public List<TeamMember> TeamMembers
        {
            get { return _teamMembers; }
            set
            {
                _teamMembers = value;
                RaisePropertyChanged("TeamMembers");
            }
        }

        public DelegateCommand HideTeamMemberResultsCommand { get; set; }
        public DelegateCommand<IList<object>> CopyCommand { get; set; }
        public DelegateCommand<TeamMember> CopyPlayerNameCommand { get; set; }
        public DelegateCommand<TeamMember> OpenPlayerCommand { get; set; }

        public DelegateCommand<ReplayFile> PlayReplayWithCommand { get; set; }

        public TeamMember ReplayUser { get; set; }

        public ReplaysManager ReplaysManager { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;"/> class and
        /// attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        [ImportingConstructor]
        public ReplayViewModel([Import(typeof(IReplayView))]IReplayView view)
            : base(view)
        {
            ReplaysManager = new ReplaysManager();

            HideTeamMemberResultsCommand = new DelegateCommand(OnHideTeamMemberResultsCommand);
            CopyCommand = new DelegateCommand<IList<object>>(OnCopyCommand);
            CopyPlayerNameCommand = new DelegateCommand<TeamMember>(OnCopyPlayerNameCommand);
            OpenPlayerCommand = new DelegateCommand<TeamMember>(OnOpenPlayerCommand);
            PlayCommand = new DelegateCommand(OnPlayCommand);
            SetSpeedCommand = new DelegateCommand<int>(OnSetSpeedCommand);

            PlayReplayWithCommand = new DelegateCommand<ReplayFile>(ReplaysManager.Play);

            ViewTyped.Closing += OnClosing;
        }

        private void OnSetSpeedCommand(int speed)
        {
            if (ReplayViewer != null)
            {
                ReplayViewer.SetSpeed(speed);
            }
        }

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            ReplayViewer.Stop();
        }

        public DelegateCommand PlayCommand { get; set; }
        public DelegateCommand<int> SetSpeedCommand { get; set; }

        private void OnPlayCommand()
        {
            _simulationWorker = new ProgressControlViewModel();

            ReplayViewer.Stop();

            _simulationWorker.Execute(Resources.Resources.ProgressTitle_Loading_replays, (bw, we) => ReplayViewer.Start());
        }

        
        public ReplayViewer ReplayViewer
        {
            get { return _replayViewer; }
            set
            {
                _replayViewer = value;
                RaisePropertyChanged("ReplayViewer");
            }
        }

        private void OnCopyPlayerNameCommand(TeamMember player)
        {
            if (player != null)
            {
                try
                {
                    Clipboard.SetText(player.Name);
                }
                catch (Exception e)
                {
                    _log.Error("Clipboard error", e);
                }
            }
        }

        private void OnOpenPlayerCommand(TeamMember member)
        {
            Domain.Server.Player player;
            using (new WaitCursor())
            {
                player = WotApiClient.Instance.LoadPlayerStat((int) member.AccountDBID, SettingsReader.Get(), PlayerStatLoadOptions.LoadVehicles | PlayerStatLoadOptions.LoadAchievments);
            }
            if (player != null)
            {
                PlayerServerStatisticViewModel viewModel = CompositionContainerFactory.Instance.GetExport<PlayerServerStatisticViewModel>();
                viewModel.Init(player);
                viewModel.Show();
            }
            else
            {
                MessageBox.Show(string.Format(Resources.Resources.Msg_GetPlayerData, member.Name), Resources.Resources.WindowCaption_Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnCopyCommand(IList<object> rows)
        {
            if (rows != null)
            {
                IEnumerable<ChatMessage> chatMessages = rows.Cast<ChatMessage>();

                StringBuilder builder = new StringBuilder();

                foreach (ChatMessage message in chatMessages)
                {
                    builder.AppendFormat("{0}: {1}", message.Player, message.Text);
                    builder.AppendLine();
                }

                Clipboard.SetText(builder.ToString());
            }
        }

        private void OnHideTeamMemberResultsCommand()
        {
            OurTeamMember = null;
            AlienTeamMember = null;
        }

        public void Show()
        {
            ViewTyped.Show();
        }

        public bool Init(Domain.Replay.Replay replay, ReplayFile replayFile)
        {
            Replay = replay;
            ReplayFile = replayFile;
            
            if (replay.datablock_battle_result != null)
            {
	            Raw = replay.datablock_battle_result.raw;

				MapDescription = GetMapDescription(replay);

                var tankDescription = Dictionaries.Instance.GetTankDescription(replay.datablock_battle_result.personal.typeCompDescr);

                Tank = tankDescription.Title;
                TankIcon = tankDescription.Icon;
                
                Date = replay.datablock_1.dateTime;

                TeamMembers = GetTeamMembers(replay, MapDescription.Team);

                DateTime playTime = DateTime.Parse(replay.datablock_1.dateTime, CultureInfo.GetCultureInfo("ru-RU"));

                Version clientVersion = ReplayFileHelper.ResolveVersion(replay.datablock_1.Version, playTime);

                long playerId = replay.datablock_battle_result.personal.accountDBID;
                FirstTeam = TeamMembers.Where(x => x.Team == MapDescription.Team).OrderByDescending(x => x.Xp).ToList();
                SecondTeam = TeamMembers.Where(x => x.Team != MapDescription.Team).OrderByDescending(x => x.Xp).ToList();
                ReplayUser = TeamMembers.First(x => x.AccountDBID == playerId);

                List<long> squads1 = FirstTeam.Where(x => x.platoonID > 0).OrderBy(x => x.platoonID).Select(x => x.platoonID).Distinct().ToList();
                List<long> squads2 = SecondTeam.Where(x => x.platoonID > 0).OrderBy(x => x.platoonID).Select(x => x.platoonID).Distinct().ToList();

                FirstTeam.ForEach(delegate(TeamMember tm) { tm.Squad = squads1.IndexOf(tm.platoonID) + 1; });
                SecondTeam.ForEach(delegate(TeamMember tm) { tm.Squad = squads2.IndexOf(tm.platoonID) + 1; });

                CombatEffects = GetCombatTargets(replay, TeamMembers);

                FullName = ReplayUser.FullName;

                double premiumFactor = replay.datablock_battle_result.personal.premiumCreditsFactor10 / (double)10;
                XpFactor = replay.datablock_battle_result.personal.dailyXPFactor10 / 10;

                int creditsPenalty = replay.datablock_battle_result.personal.creditsPenalty;
                int premiumCreditsPenalty = (int)Math.Round(creditsPenalty * premiumFactor, 0);

                Crystal = replay.datablock_battle_result.personal.crystal;
				EligibleForCrystalRewards = replay.datablock_battle_result.avatar.eligibleForCrystalRewards;

				PiercingsReceived = replay.datablock_battle_result.personal.piercedReceived;
                NoDamageDirectHitsReceived = replay.datablock_battle_result.personal.noDamageShotsReceived;
                RickochetsReceived = replay.datablock_battle_result.personal.rickochetsReceived;
                DamageAssistedStun = replay.datablock_battle_result.personal.damageAssistedStun;
                HEHitsReceived = replay.datablock_battle_result.personal.heHitsReceived;
                StunNum = replay.datablock_battle_result.personal.stunNum;
				IsAlive = true;

				IsPremium = replay.datablock_battle_result.personal.isPremium;
                IsBase = !IsPremium;

	            #region [Credits]

	            //начислено за бой
	            OriginalCredits = replay.datablock_battle_result.personal.originalCredits;
	            OriginalCreditsPremium = (int)(replay.datablock_battle_result.personal.originalCredits * premiumFactor);
	            
	            //Итого за бой
	            var temp = replay.datablock_battle_result.personal.credits;
	            CreditsPremium = IsPremium ? temp : (int)Math.Round(temp * premiumFactor, 0);
	            Credits = IsPremium ? (int)Math.Round((temp / premiumFactor), 0) : temp;
	            
	            //за боевые задачи
	            temp = replay.datablock_battle_result.personal.eventCredits;
	            EventCreditsPremium = IsPremium ? temp : (int)Math.Round(temp * premiumFactor, 0);
	            EventCredits = IsPremium ? (int)Math.Round((temp / premiumFactor), 0) : temp;

	            //личные резервы
	            temp = replay.datablock_battle_result.personal.boosterCredits;
	            BoosterCreditsPremium = IsPremium ? temp : (int) Math.Round(temp * premiumFactor, 0);
	            BoosterCredits = IsPremium ? (int) Math.Round((temp / premiumFactor), 0) : temp;

	            //боевые выплаты
	            temp = replay.datablock_battle_result.personal.orderCredits;
	            OrderCreditsPremium = IsPremium ? temp : (int)Math.Round(temp * premiumFactor, 0);
	            OrderCredits = IsPremium ? (int)Math.Round((temp / premiumFactor), 0) : temp;

	            //За достижения полученные в бою
	            temp = replay.datablock_battle_result.personal.achievementCredits;
	            AchievementCreditsPremium = IsPremium ? temp : (int)Math.Round(temp * premiumFactor, 0);
	            AchievementCredits = IsPremium ? (int)Math.Round((temp / premiumFactor), 0) : temp;

				
	            //Компенсация за урон от союзников
	            temp = replay.datablock_battle_result.personal.creditsContributionIn;
	            CreditsContributionInPremium = IsPremium ? temp : (int)Math.Round(temp * premiumFactor, 0);
	            CreditsContributionIn = IsPremium ? (int)Math.Round((temp / premiumFactor), 0) : temp;

	            //Штраф за урон союзникам
	            temp = replay.datablock_battle_result.personal.creditsPenalty;
	            CreditsContributionOutPremium = IsPremium ? -temp : -(int)Math.Round(temp * premiumFactor, 0);
	            CreditsContributionOut = IsPremium ? -(int)Math.Round((temp / premiumFactor), 0) : -temp;

	            //Штрафы
	            FairplayViolationsCreditsPremium = new ObservableCollection<int>(
		            replay.datablock_battle_result.avatar.fairplayViolations.Select(t =>
			            IsPremium ? -t : -(int) Math.Round(t * premiumFactor, 0)));
	            FairplayViolationsCredits = new ObservableCollection<int>(
		            replay.datablock_battle_result.avatar.fairplayViolations.Select(t =>
			            IsPremium ? -(int) Math.Round((t / premiumFactor), 0) : -t));

	            AutoRepairCost = -(replay.datablock_battle_result.personal.autoRepairCost ?? 0);
	            AutoLoadCost = new ObservableCollection<int>(replay.datablock_battle_result.personal.autoLoadCost.Select(t => -t));
	            AutoEquipCost = new ObservableCollection<int>(replay.datablock_battle_result.personal.autoEquipCost.Select(t => -t));

	            TotalCredits.Add(Credits + AutoRepairCost + AutoLoadCost[0] + AutoEquipCost[0]);
	            TotalCredits.Add(AutoLoadCost[1] + AutoEquipCost[1]);

	            TotalCreditsPremium.Add(CreditsPremium + AutoRepairCost + AutoLoadCost[0] + AutoEquipCost[0]);
	            TotalCreditsPremium.Add(AutoLoadCost[1] + AutoEquipCost[1]);

		            #endregion

				Xp = replay.datablock_battle_result.personal.originalXP;
	            PremiumXp = (int)(Xp * premiumFactor);

				XpPenalty = -replay.datablock_battle_result.personal.xpPenalty;
	            PremiumXpPenalty = (int)(XpPenalty * premiumFactor);

				FreeXp = replay.datablock_battle_result.personal.originalFreeXP;
	            PremiumFreeXp = (int)(FreeXp * premiumFactor);

	            temp = replay.datablock_battle_result.personal.eventXP;
	            EventXp.Add(IsPremium ? (int)Math.Round((temp / premiumFactor), 0) : temp);
	            EventXp.Add(IsPremium ? temp : (int)Math.Round(temp * premiumFactor, 0));
	            temp = replay.datablock_battle_result.personal.eventFreeXP;
				EventXp.Add(IsPremium ? (int)Math.Round((temp / premiumFactor), 0) : temp);
	            EventXp.Add(IsPremium ? temp : (int)Math.Round(temp * premiumFactor, 0));

	            temp = replay.datablock_battle_result.personal.boosterXP;
	            BoosterXp.Add(IsPremium ? (int)Math.Round((temp / premiumFactor), 0) : temp);
	            BoosterXp.Add(IsPremium ? temp : (int)Math.Round(temp * premiumFactor, 0));
	            temp = replay.datablock_battle_result.personal.boosterFreeXP;
	            BoosterXp.Add(IsPremium ? (int)Math.Round((temp / premiumFactor), 0) : temp);
	            BoosterXp.Add(IsPremium ? temp : (int)Math.Round(temp * premiumFactor, 0));

	            temp = replay.datablock_battle_result.personal.orderXP;
	            OrderXp.Add(IsPremium ? (int)Math.Round((temp / premiumFactor), 0) : temp);
	            OrderXp.Add(IsPremium ? temp : (int)Math.Round(temp * premiumFactor, 0));
	            temp = replay.datablock_battle_result.personal.orderFreeXP;
	            OrderXp.Add(IsPremium ? (int)Math.Round((temp / premiumFactor), 0) : temp);
	            OrderXp.Add(IsPremium ? temp : (int)Math.Round(temp * premiumFactor, 0));


	            temp = replay.datablock_battle_result.personal.achievementXP;
	            AchievementXp.Add(IsPremium ? (int)Math.Round((temp / premiumFactor), 0) : temp);
	            AchievementXp.Add(IsPremium ? temp : (int)Math.Round(temp * premiumFactor, 0));
	            temp = replay.datablock_battle_result.personal.achievementFreeXP;
	            AchievementXp.Add(IsPremium ? (int)Math.Round((temp / premiumFactor), 0) : temp);
	            AchievementXp.Add(IsPremium ? temp : (int)Math.Round(temp * premiumFactor, 0));

	            temp = replay.datablock_battle_result.personal.xp;
	            PremiumTotalXp = IsPremium ? temp : (int)Math.Round(temp * premiumFactor, 0);
	            BaseTotalXp = IsPremium ? (int)Math.Round((temp / premiumFactor), 0) : temp;

				temp = replay.datablock_battle_result.personal.freeXP;
	            PremiumTotalFreeXp = IsPremium ? temp : (int)Math.Round(temp * premiumFactor, 0);
	            BaseTotalFreeXp = IsPremium ? (int)Math.Round((temp / premiumFactor), 0) : temp;

	            BattleXp = replay.datablock_battle_result.personal.xp;
	            BattleCredits = replay.datablock_battle_result.personal.credits;

				Shots = replay.datablock_battle_result.personal.shots;
                Hits = replay.datablock_battle_result.personal.hits;
                Crits = CombatEffects.Sum(x => x.Crits);
                HEHits = replay.datablock_battle_result.personal.he_hits;
                Pierced = replay.datablock_battle_result.personal.pierced;
                DamageDealt = replay.datablock_battle_result.personal.damageDealt;
                ShotsReceived = replay.datablock_battle_result.personal.shotsReceived;
                TDamage = $"{replay.datablock_battle_result.personal.tkills}/{replay.datablock_battle_result.personal.tdamageDealt}";
	            TDamageSign = (int)(-replay.datablock_battle_result.personal.tkills -replay.datablock_battle_result.personal.tdamageDealt);

				Damaged = replay.datablock_battle_result.personal.damaged;
                Killed = replay.datablock_battle_result.personal.kills;
                Spotted = replay.datablock_battle_result.personal.spotted;
                DamageAssisted = replay.datablock_battle_result.personal.damageAssisted;
                DamageAssistedTrack = replay.datablock_battle_result.personal.damageAssistedTrack;
                DamageAssistedRadio = replay.datablock_battle_result.personal.damageAssistedRadio == 0
                    ? replay.datablock_battle_result.personal.damageAssisted - replay.datablock_battle_result.personal.damageAssistedTrack 
                    : replay.datablock_battle_result.personal.damageAssistedRadio;
                CapturePoints = replay.datablock_battle_result.personal.capturePoints;
                DroppedCapturePoints = replay.datablock_battle_result.personal.droppedCapturePoints;
                PotentialDamageReceived = replay.datablock_battle_result.personal.potentialDamageReceived;
                DamageBlockedByArmor = replay.datablock_battle_result.personal.damageBlockedByArmor;
	            SniperDamageDealt = replay.datablock_battle_result.personal.sniperDamageDealt;
				Mileage = string.Format(Resources.Resources.Traveled_Format, replay.datablock_battle_result.personal.mileage/(double)1000);

                StartTime = DateTime.Parse(replay.datablock_1.dateTime, CultureInfo.GetCultureInfo("ru-RU")).ToShortTimeString();
                TimeSpan battleLength = new TimeSpan(0, 0, (int) replay.datablock_battle_result.common.duration);
                BattleTime = battleLength.ToString(Resources.Resources.ExtendedTimeFormat);

                List<Medal> medals = GetMedals(replay);

                BattleMedals = medals.Where(x => x.Type == 0).ToList();
                AchievMedals = medals.Where(x => x.Type == 1).ToList();

                TimeSpan userbattleLength = new TimeSpan(0, 0, replay.datablock_battle_result.personal.lifeTime);
                UserBattleTime = (replay.datablock_battle_result.personal.deathReason == -1 ||
                                    replay.datablock_battle_result.personal.killerID == 0) ? "-": userbattleLength.ToString(Resources.Resources.ExtendedTimeFormat);

                //calc levels by squad
                List<LevelRange> membersLevels = new List<LevelRange>();
                membersLevels.AddRange(TeamMembers.Where(x => x.Squad == 0).Select(x => x.LevelRange));
                IEnumerable<IGrouping<int, TeamMember>> squads = TeamMembers.Where(x => x.Squad > 0).GroupBy(x => x.Squad);
                membersLevels.AddRange(squads.Select(x => x.Select(s => s.LevelRange).OrderByDescending(o => o.Max).First()));

                int level = Dictionaries.Instance.GetBattleLevel(membersLevels);

                Status = GetBattleStatus(replay);

                DeathReason = (DeathReason)replay.datablock_battle_result.personal.deathReason;

                FinishReason = (FinishReason)replay.datablock_battle_result.common.finishReason;

                _devices = new List<DeviceDescription>();

                if (replay.datablock_advanced != null)
                {
                    if (replay.datablock_advanced.roster != null && replay.datablock_advanced.roster.ContainsKey(replay.datablock_1.playerName))
                    {
                        var info = replay.datablock_advanced.roster[replay.datablock_1.playerName];

                        if (Dictionaries.Instance.DeviceDescriptions.ContainsKey(info.vehicle.module_0))
                        {
                            _devices.Add(Dictionaries.Instance.DeviceDescriptions[info.vehicle.module_0]);
                        }

                        if (Dictionaries.Instance.DeviceDescriptions.ContainsKey(info.vehicle.module_1))
                        {
                            _devices.Add(Dictionaries.Instance.DeviceDescriptions[info.vehicle.module_1]);
                        }

                        if (Dictionaries.Instance.DeviceDescriptions.ContainsKey(info.vehicle.module_2))
                        {
                            _devices.Add(Dictionaries.Instance.DeviceDescriptions[info.vehicle.module_2]);
                        }
                    }
                    
                    foreach (Slot slot in replay.datablock_advanced.Slots)
                    {
                        if (TankIcon.CountryId != Country.Unknown)
                        {
                            if (Dictionaries.Instance.ConsumableDescriptions.ContainsKey(slot.Item.Id) &&
                                slot.Item.TypeId == SlotType.Equipment)
                            {
                                slot.Description = Dictionaries.Instance.ConsumableDescriptions[slot.Item.Id];
                                Consumables.Add(slot);
                            }
                            if (Dictionaries.Instance.Shells[TankIcon.CountryId].ContainsKey(slot.Item.Id) &&
                                slot.Item.TypeId == SlotType.Shell)
                            {
                                slot.Description = Dictionaries.Instance.Shells[TankIcon.CountryId][slot.Item.Id];
                                Shells.Add(slot);
                            }
                        }
                    }

                    if (replay.datablock_advanced.more != null)
                    {
                        level = replay.datablock_advanced.more.battleLevel;
                    }

                    ChatMessages = replay.datablock_advanced.Messages;
                }

                Title = string.Format(Resources.Resources.WindowTitleFormat_Replay, Tank, MapDescription.MapName, level > 0 ? level.ToString(CultureInfo.InvariantCulture) : "n/a", clientVersion.ToString(3));

                ReplayViewer = new ReplayViewer(Replay, TeamMembers.Select(x => new MapVehicle(x)).ToList());

                return true;
            }
            return false;
        }

        private IReplayMap GetMapDescription(Domain.Replay.Replay replay)
        {
            ReplayMapViewModel mapDescription = new ReplayMapViewModel();

            mapDescription.MapNameId = replay.datablock_1.mapName;
            mapDescription.Gameplay = (Gameplay) Enum.Parse(typeof (Gameplay), replay.datablock_1.gameplayID);
            mapDescription.MapName = string.Format("{0} - {1}", replay.datablock_1.mapDisplayName,
                GetMapMode(mapDescription.Gameplay, (BattleType)replay.datablock_1.battleType));

            if (Dictionaries.Instance.Maps.ContainsKey(replay.datablock_1.mapName))
            {
                mapDescription.MapId = Dictionaries.Instance.Maps[replay.datablock_1.mapName].MapId;
            }
            else
            {
                _log.WarnFormat("Unknown map: {0}", replay.datablock_1.mapName);
            }

            long playerId = replay.datablock_battle_result.personal.accountDBID;

            mapDescription.Team = replay.datablock_battle_result.players[playerId].team;
            return mapDescription;
        }

        private List<Medal> GetMedals(Domain.Replay.Replay replay)
        {
            return ReplayUser.BattleMedals.Union(Dictionaries.Instance.GetAchievMedals(replay.datablock_battle_result.personal.dossierPopUps))
                .Union(Dictionaries.Instance.GetAchievMedals(new List<List<JValue>> { new List<JValue> { new JValue(7900 + replay.datablock_battle_result.personal.markOfMastery), new JValue(0) } })).ToList();
        }

        private List<CombatTarget> GetCombatTargets(Domain.Replay.Replay replay, List<TeamMember> teamMembers)
        {
            return replay.datablock_battle_result.personal.details.Select(x => new GenericListItem<long, DamagedVehicle>(KeyToLong(x.Key), x.Value))
                .Where(x => x.Id != ReplayUser.Id)
                .Select(x => new CombatTarget(new KeyValuePair<long, DamagedVehicle>(x.Id, x.Value), teamMembers.First(tm => tm.Id == x.Id), replay.datablock_1.Version))
                .OrderBy(x => x.TeamMember.FullName)
                .ToList();
        }

        private long KeyToLong(string key)
        {
            if (key.Contains(","))
            {
                var longKey = Convert.ToInt64(key.Replace("(", string.Empty).Replace(")", string.Empty).Split(',')[0]);
                return longKey;
            }
            return Convert.ToInt64(key);
        }

        private static List<TeamMember> GetTeamMembers(Domain.Replay.Replay replay, int myTeamId)
        {
            List<KeyValuePair<long, Player>> players = replay.datablock_battle_result.players.ToList();
            List<KeyValuePair<long, VehicleResult>> vehicleResults = replay.datablock_battle_result.vehicles.ToList();
            List<KeyValuePair<long, Vehicle>> vehicles = replay.datablock_1.vehicles.ToList();
            List<TeamMember> teamMembers =
                players.Join(vehicleResults, p => p.Key, vr => vr.Value.accountDBID, Tuple.Create)
                    .Join(vehicles, pVr => pVr.Item2.Key, v => v.Key,
                        (pVr, v) => new TeamMember(pVr.Item1, pVr.Item2, v, myTeamId, replay.datablock_1.regionCode, replay.datablock_battle_result.players, replay.datablock_battle_result.vehicles))
                    .ToList();
            return teamMembers;
        }

        private BattleStatus GetBattleStatus(Domain.Replay.Replay replay)
        {
            if(replay.datablock_battle_result.common.winnerTeam == 0)
            {
                return BattleStatus.Draw;
            }

            if (replay.datablock_battle_result.common.winnerTeam == ReplayUser.Team)
            {
                return BattleStatus.Victory;
            }

            return BattleStatus.Defeat;
        }

        private object GetMapMode(Gameplay gameplayId, BattleType battleType)
        {
            List<BattleType> list = new List<BattleType>
            {
                BattleType.Unknown,
                BattleType.Regular,
                BattleType.CompanyWar,
                BattleType.ClanWar,
                BattleType.Event
            };

            if (list.Contains(battleType))
            {
                if (gameplayId == Gameplay.ctf)
                {
                    return Resources.Resources.BattleType_ctf;
                }
                if (gameplayId == Gameplay.domination)
                {
                    return Resources.Resources.BattleType_domination;
                }
                if (gameplayId == Gameplay.nations)
                {
                    return Resources.Resources.BattleType_nations;
                }
                return Resources.Resources.BattleType_assault;
            }
            return Resources.Resources.ResourceManager.GetEnumResource(battleType);
        }
    }
}
