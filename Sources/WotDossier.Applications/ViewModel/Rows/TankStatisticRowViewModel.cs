﻿using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Common;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel.Rows
{
    public class TankStatisticRowViewModel : PeriodStatisticViewModel<TankStatisticRowViewModel>, ITankRowBattles, ITankRowDamage, 
                           ITankRowFrags, ITankRowMasterTanker, ITankRowPerformance, ITankRowRatings, 
                           ITankRowTime, ITankRowXP, ITankFilterable
    {
        private DateTime _lastBattle;
        private IEnumerable<FragsJson> _tankFrags;
        private bool _isFavorite;

        #region Common

        public TankIcon Icon { get; set; }

        public string Tank { get; set; }

        public int Type { get; set; }

        public int CountryId { get; set; }

        public int TankId { get; set; }

        public int TankUniqueId { get; set; }

        public bool IsPremium { get; set; }

        public bool IsFavorite
        {
            get { return _isFavorite; }
            set
            {
                _isFavorite = value;
                OnPropertyChanged("IsFavorite");
            }
        }

        #endregion

        #region [ ITankRowBattles ]

        public int Draws
        {
            get { return BattlesCount - Wins - Losses; }
        }

        public double DrawsPercent
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return Draws/(double) BattlesCount*100.0;
                }
                return 0;
            }
        }

        public int SurvivedAndWon { get; set; }

        public double SurvivedAndWonPercent
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return SurvivedAndWon/(double) BattlesCount*100.0;
                }
                return 0;
            }
        }

        #endregion

        #region [ ITankRowDamage ]

        public int DamagePerHit
        {
            get
            {
                if (Hits > 0)
                {
                    return DamageDealt/Hits;
                }
                return 0;
            }
        }

        public double DamageRatioDelta
        {
            get { return DamageRatio - PrevStatistic.DamageRatio; }
        }

        #endregion
       
        #region [ ITankRowFrags ]

        public int MaxFrags { get; set; }

        public double FragsPerBattle
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return Frags/(double) BattlesCount;
                }
                return 0;
            }
        }

        public int Tier8Frags { get; set; }

        public int BeastFrags { get; set; }

        public int SinaiFrags { get; set; }

        #endregion

        #region [ ITankRowPerformance ]

        public int Shots { get; set; }

        public int Hits { get; set; }

        #endregion

        #region [ ITankRowRatings ]

        public int DamageRatingRev1
        {
            get
            {
                if (DamageTaken > 0)
                {
                    return (int) (DamageDealt/(double) DamageTaken*100);
                }
                return 0;
            }
        }

        public int MarkOfMastery { get; set; }

        #endregion

        #region [ ITankRowTime ]
        public DateTime LastBattle
        {
            get { return _lastBattle; }
            set { _lastBattle = value; }
        }

        public TimeSpan PlayTime { get; set; }
        public TimeSpan AverageBattleTime { get; set; }
        #endregion

        public int OriginalXP { get; set; }
        public int DamageAssistedTrack { get; set; }
        public int DamageAssistedRadio { get; set; }
        public double Mileage { get; set; }
        public int ShotsReceived { get; set; }
        public int NoDamageShotsReceived { get; set; }
        public int PiercedReceived { get; set; }
        public int HeHitsReceived { get; set; }
        public int HeHits { get; set; }
        public int Pierced { get; set; }
        public int XpBefore88 { get; set; }
        public int BattlesCountBefore88 { get; set; }
        public int BattlesCount88 { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TankStatisticRowViewModel"/> class.
        /// </summary>
        /// <param name="tank">The tank.</param>
        public TankStatisticRowViewModel(TankJson tank)
            : this(tank, new List<TankJson>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public TankStatisticRowViewModel(TankJson tank, IEnumerable<TankJson> list) : base(Utils.UnixDateToDateTime(tank.Common.updated), list.Select(x => new TankStatisticRowViewModel(x)).ToList())
        {
            Tier = tank.Common.tier;
            Type = tank.Common.type;
            Tank = tank.Common.tanktitle;
            Icon = tank.Icon;
            CountryId = tank.Common.countryid;
            TankId = tank.Common.tankid;
            TankUniqueId = tank.UniqueId();
            TankFrags = tank.Frags;
            OriginalXP = tank.Tankdata.originalXP;
            DamageAssistedTrack = tank.Tankdata.damageAssistedTrack;
            DamageAssistedRadio = tank.Tankdata.damageAssistedRadio;
            Mileage = tank.Tankdata.mileage / 1000;
            ShotsReceived = tank.Tankdata.shotsReceived;
            NoDamageShotsReceived = tank.Tankdata.noDamageShotsReceived;
            PiercedReceived = tank.Tankdata.piercedReceived;
            HeHitsReceived = tank.Tankdata.heHitsReceived;
            HeHits = tank.Tankdata.he_hits;
            Pierced = tank.Tankdata.pierced;
            XpBefore88 = tank.Tankdata.xpBefore8_8 != 0 ? tank.Tankdata.xpBefore8_8 : tank.Tankdata.xp;
            BattlesCountBefore88 = tank.Tankdata.battlesCountBefore8_8 != 0 ? tank.Tankdata.battlesCountBefore8_8 : tank.Tankdata.battlesCount;
            BattlesCount88 = tank.Tankdata.battlesCount - BattlesCountBefore88;
            IsPremium = tank.Common.premium == 1;

            #region [ ITankRowBattleAwards ]
            BattleHero = tank.Battle.battleHeroes;
            Warrior = tank.Battle.warrior;
            Invader = tank.Battle.invader;
            Sniper = tank.Battle.sniper;
            Defender = tank.Battle.sniper;
            SteelWall = tank.Battle.steelwall;
            Confederate = tank.Battle.supporter;
            Scout = tank.Battle.scout;
            PatrolDuty = tank.Battle.evileye;
            BrothersInArms = tank.Epic.BrothersInArms;
            CrucialContribution = tank.Epic.CrucialContribution;
            CoolHeaded = tank.Special.alaric;
            LuckyDevil = tank.Special.luckyDevil;
            Spartan = tank.Special.sturdy;
            Ranger = tank.Special.huntsman;
            #endregion

            #region [ ITankRowBattles ]
            BattlesCount = tank.Tankdata.battlesCount;
            Wins = tank.Tankdata.wins;
            Losses = tank.Tankdata.losses;
            SurvivedBattles = tank.Tankdata.survivedBattles;
            SurvivedAndWon = tank.Tankdata.winAndSurvived;
            #endregion

            #region [ ITankRowDamage ]
            DamageDealt = tank.Tankdata.damageDealt;
            DamageTaken = tank.Tankdata.damageReceived;
            #endregion

            #region [ ITankRowEpic ]
            Boelter = tank.Epic.Boelter;
            RadleyWalters = tank.Epic.RadleyWalters;
            LafayettePool = tank.Epic.LafayettePool;
            Orlik = tank.Epic.Orlik;
            Oskin = tank.Epic.Oskin;
            Lehvaslaiho = tank.Epic.Lehvaslaiho;
            Nikolas = tank.Epic.Nikolas;
            Halonen = tank.Epic.Halonen;
            Burda = tank.Epic.Burda;
            Pascucci = tank.Epic.Pascucci;
            Dumitru = tank.Epic.Dumitru;
            TamadaYoshio = tank.Epic.TamadaYoshio;
            Billotte = tank.Epic.Billotte;
            BrunoPietro = tank.Epic.BrunoPietro;
            Tarczay = tank.Epic.Tarczay;
            Kolobanov = tank.Epic.Kolobanov;
            Fadin = tank.Epic.Fadin;
            HeroesOfRassenay = tank.Special.heroesOfRassenay;
            DeLanglade = tank.Epic.DeLanglade;
            #endregion

            #region [ ITankRowFrags ]
            Frags = tank.Tankdata.frags;
            MaxFrags = tank.Tankdata.maxFrags;
            Tier8Frags = tank.Tankdata.frags8p;
            BeastFrags = tank.Tankdata.fragsBeast;
            SinaiFrags = tank.Battle.fragsSinai;
            #endregion

            #region [ ITankRowMasterTanker ]

            #endregion

            #region [ ITankRowMedals]
            Kay = tank.Major.Kay;
            Carius = tank.Major.Carius;
            Knispel = tank.Major.Knispel;
            Poppel = tank.Major.Poppel;
            Abrams = tank.Major.Abrams;
            Leclerk = tank.Major.LeClerc;
            Lavrinenko = tank.Major.Lavrinenko;
            Ekins = tank.Major.Ekins;
            #endregion

            #region [ ITankRowPerformance ]
            Shots = tank.Tankdata.shots;
            Hits = tank.Tankdata.hits;
            if (Shots > 0)
            {
                HitsPercents = Hits/(double) Shots*100.0;
            }
            CapturePoints = tank.Tankdata.capturePoints;
            DroppedCapturePoints = tank.Tankdata.droppedCapturePoints;
            Spotted = tank.Tankdata.spotted;
            #endregion

            #region [ ITankRowSeries ]
            ReaperLongest = tank.Series.maxKillingSeries;
            ReaperProgress = tank.Series.killingSeries;
            SharpshooterLongest = tank.Series.maxSniperSeries;
            SharpshooterProgress = tank.Series.sniperSeries;
            MasterGunnerLongest = tank.Series.maxPiercingSeries;
            MasterGunnerProgress = tank.Series.piercingSeries;
            InvincibleLongest = tank.Series.maxInvincibleSeries;
            InvincibleProgress = tank.Series.invincibleSeries;
            SurvivorLongest = tank.Series.maxDiehardSeries;
            SurvivorProgress = tank.Series.diehardSeries;
            #endregion

            #region [ ITankRowSpecialAwards ]
            Kamikaze = tank.Special.kamikaze;
            Raider = tank.Special.raider;
            Bombardier = tank.Special.bombardier;
            Reaper = tank.Series.maxKillingSeries;
            Sharpshooter = tank.Series.maxSniperSeries;
            Invincible = tank.Series.maxInvincibleSeries;
            Survivor = tank.Series.maxDiehardSeries;
            MouseTrap = tank.Special.mousebane;
            Hunter = tank.Special.beasthunter;
            Sinai = tank.Special.sinai;
            PattonValley = tank.Special.pattonValley;
            #endregion

            #region [ ITankRowTime ]
            LastBattle = tank.Common.lastBattleTimeR;
            PlayTime = new TimeSpan(0, 0, 0, tank.Tankdata.battleLifeTime);
            if (tank.Tankdata.battlesCount > 0)
            {
                AverageBattleTime = new TimeSpan(0, 0, 0, tank.Tankdata.battleLifeTime/tank.Tankdata.battlesCount);
            }
            #endregion

            #region [ ITankRowXP ]
            Xp = tank.Tankdata.xp;
            MaxXp = tank.Tankdata.maxXP;
            #endregion

            #region [ ITankRowRatings ]

            MarkOfMastery = tank.Special.markOfMastery;

            #endregion

            Updated = Utils.UnixDateToDateTime(tank.Common.updated);
        }

        public IEnumerable<FragsJson> TankFrags
        {
            get { return _tankFrags; }
            set { _tankFrags = value; }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return Tank;
        }
    }
}