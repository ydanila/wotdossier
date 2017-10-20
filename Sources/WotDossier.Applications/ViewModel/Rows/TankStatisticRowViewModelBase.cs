﻿using System;
using System.Collections.Generic;
using WotDossier.Applications.Logic;
using WotDossier.Applications.ViewModel.Statistic;
using WotDossier.Common;
using WotDossier.Domain;
using WotDossier.Domain.Rating;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel.Rows
{
    public abstract class TankStatisticRowViewModelBase : StatisticViewModelBase, ITankStatisticRow
    {
        private DateTime _lastBattle;
        private bool _isFavorite;

        #region Common

        public TankDescription TankDescription { get; set; }

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

        #region [ IStatisticBattles ]

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

        #region [ IStatisticDamage ]

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

        #endregion
       
        #region [ IStatisticFrags ]

        public int Tier8Frags { get; set; }

        public int BeastFrags { get; set; }

        public int SinaiFrags { get; set; }

        public int PattonFrags { get; set; }

        public int MouseFrags { get; set; }

        #endregion

        #region [ IStatisticPerformance ]

        public int Shots { get; set; }

        public int Hits { get; set; }

        #endregion

        #region [ IStatisticRatings ]

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

        #endregion

        #region [ IStatisticTime ]
        public DateTime LastBattle
        {
            get { return _lastBattle; }
            set { _lastBattle = value; }
        }

        public TimeSpan AverageBattleTime { get; set; }
        #endregion

        public int OriginalXP { get; set; }
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
        
        private StatisticViewModelBase TypedPrevStatistic
        {
            get { return (StatisticViewModelBase)PrevStatisticSlice.Statistic; }
        }

        IEnumerable<ITankStatisticRow> ITankStatisticRow.GetAll()
        {
            return GetAllSlices<TankStatisticRowViewModelBase>();
        }

        public void SetPreviousStatistic(ITankStatisticRow model)
        {
            SetPreviousStatistic(((PeriodStatisticViewModel)model).ToStatisticSlice());
        }

        public ITankStatisticRow PreviousStatistic
        {
            get { return (ITankStatisticRow) TypedPrevStatistic; }
        }

        public double AvgDamageAssistedTrack
        {
            get
            {
                if (BattlesCount88 > 0)
                {
                    return DamageAssistedTrack / (double)BattlesCount88;
                }
                return 0;
            }
        }

        public double AvgDamageAssistedRadio
        {
            get
            {
                if (BattlesCount88 > 0)
                {
                    return DamageAssistedRadio / (double)BattlesCount88;
                }
                return 0;
            }
        }

        public double AvgDamageAssisted
        {
            get
            {
                if (BattlesCount88 > 0)
                {
                    return DamageAssisted / (double)BattlesCount88;
                }
                return 0;
            }
        }

        public double AvgOriginalXP
        {
            get
            {
                if (BattlesCount88 > 0)
                {
                    return OriginalXP / (double)BattlesCount88;
                }
                return 0;
            }
        }

        public double AvgPotentialDamageReceived
        {
            get
            {
                if (BattlesCount90 > 0)
                {
                    return PotentialDamageReceived / (double)BattlesCount90;
                }
                return 0;
            }
        }

        public double AvgDamageBlockedByArmor
        {
            get
            {
                if (BattlesCount90 > 0)
                {
                    return DamageBlockedByArmor / (double)BattlesCount90;
                }
                return 0;
            }
        }

        public int DamageAssistedDelta
        {
            get { return DamageAssisted - TypedPrevStatistic.DamageAssisted; }
        }

        public double AvgDamageAssistedForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return DamageAssistedDelta / (double)BattlesCountDelta;
                }
                return 0;
            }
        }

        public int PotentialDamageReceivedDelta
        {
            get { return PotentialDamageReceived - TypedPrevStatistic.PotentialDamageReceived; }
        }

        public double AvgPotentialDamageReceivedForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return PotentialDamageReceivedDelta / (double)BattlesCountDelta;
                }
                return 0;
            }
        }

        #region [ WN8 ]

        private double calculateWN8(WN8Type wn8type, int battleCount, double avgDmg, double avgFrag, double avgSpot, double avgDef, double avgWinRate)
        {
            if (battleCount > 0)
            {
	            var expValues = Dictionaries.Instance.FindExpectedValues(wn8type, Description);

				double expDamage = battleCount * expValues.Damage / battleCount;
                double expSpotted = battleCount * expValues.Spotted / battleCount;
                double expDef = battleCount * expValues.Defence / battleCount;
                double expWinRate = battleCount * expValues.WinRate / battleCount;
                double expFrags = battleCount * expValues.Frags / battleCount;

                return RatingHelper.Wn8(avgDmg, expDamage, avgFrag, expFrags, avgSpot, expSpotted, avgDef, expDef, avgWinRate, expWinRate);
            }
            return 0;
        }
        public override double WN8Rating => calculateWN8(WN8Type.Default, BattlesCount, AvgDamageDealt, AvgFrags, AvgSpotted, AvgDroppedCapturePoints, WinsPercent);
        public override double WN8KTTCRating => calculateWN8(WN8Type.KTTC, BattlesCount, AvgDamageDealt, AvgFrags, AvgSpotted, AvgDroppedCapturePoints, WinsPercent);
        public override double WN8XVMRating => calculateWN8(WN8Type.XVM, BattlesCount, AvgDamageDealt, AvgFrags, AvgSpotted, AvgDroppedCapturePoints, WinsPercent);

        #endregion

        public override double RBR
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return RatingHelper.PersonalRating(BattlesCount, BattlesCount88, Wins/(double)BattlesCount,
                        SurvivedBattles/(double)BattlesCount, DamageDealt/(double)BattlesCount, AvgOriginalXP, AvgDamageAssistedRadio, AvgDamageAssistedTrack);
                }
                return 0;
            }
        }

        public override double PerformanceRating
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return RatingHelper.PerformanceRating(BattlesCount, Wins,
                        BattlesCount* Dictionaries.Instance.FindExpectedValues(WN8Type.Perfomance, Description).Damage,
                        DamageDealt, Tier);
                }
                return 0;
            }
        }

        public override double PerformanceRatingForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return RatingHelper.PerformanceRating(BattlesCountDelta, WinsDelta,
                        BattlesCountDelta * Dictionaries.Instance.FindExpectedValues(WN8Type.Perfomance, Description).Damage,
                        DamageDealtDelta, Tier, false);
                }
                return 0;
            }
        }
        
        public override double WN8RatingForPeriod => calculateWN8(WN8Type.Default, BattlesCountDelta, AvgDamageDealtForPeriod, AvgFragsForPeriod, AvgSpottedForPeriod, AvgDroppedCapturePointsForPeriod, WinsPercentForPeriod);
        public override double WN8KTTCRatingForPeriod => calculateWN8(WN8Type.KTTC, BattlesCountDelta, AvgDamageDealtForPeriod, AvgFragsForPeriod, AvgSpottedForPeriod, AvgDroppedCapturePointsForPeriod, WinsPercentForPeriod);
        public override double WN8XVMRatingForPeriod => calculateWN8(WN8Type.XVM, BattlesCountDelta, AvgDamageDealtForPeriod, AvgFragsForPeriod, AvgSpottedForPeriod, AvgDroppedCapturePointsForPeriod, WinsPercentForPeriod);

        protected TankStatisticRowViewModelBase()
        {
        }

        public abstract Func<TankJson, StatisticJson> Predicate { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public TankStatisticRowViewModelBase(TankJson tank, IEnumerable<StatisticSlice> list)
            : base(Utils.UnixDateToDateTime(tank.Common.updated), list)
        {
            Description = tank.Description;
            Tier = tank.Description.Tier;
            Type = tank.Description.Type;
	        TankDescription = tank.Description;
            CountryId = tank.Description.CountryId;
            TankId = tank.Description.TankId;
            TankUniqueId = tank.UniqueId();
            Mileage = tank.Common.mileage / 1000;
            IsPremium = tank.Common.premium == 1;
            TankFrags = tank.Frags ?? new List<FragsJson>();

            OriginalXP = Predicate(tank).originalXP;
            DamageAssistedTrack = Predicate(tank).damageAssistedTrack;
            DamageAssistedRadio = Predicate(tank).damageAssistedRadio;
            ShotsReceived = Predicate(tank).shotsReceived;
            NoDamageShotsReceived = Predicate(tank).noDamageShotsReceived;
            PiercedReceived = Predicate(tank).piercedReceived;
            HeHitsReceived = Predicate(tank).heHitsReceived;
            HeHits = Predicate(tank).he_hits;
            Pierced = Predicate(tank).pierced;
            XpBefore88 = Predicate(tank).xpBefore8_8;
            BattlesCountBefore88 = Predicate(tank).battlesCountBefore8_8;
            BattlesCount88 = Predicate(tank).battlesCount - BattlesCountBefore88;
            BattlesCount90 = Predicate(tank).battlesCount - Predicate(tank).battlesCountBefore9_0;
            PotentialDamageReceived = Predicate(tank).potentialDamageReceived;
            DamageBlockedByArmor = Predicate(tank).damageBlockedByArmor;


            #region [ IStatisticBattles ]
            BattlesCount = Predicate(tank).battlesCount;
            Wins = Predicate(tank).wins;
            Losses = Predicate(tank).losses;
            SurvivedBattles = Predicate(tank).survivedBattles;
            SurvivedAndWon = Predicate(tank).winAndSurvived;
            #endregion

            #region [ IStatisticDamage ]
            DamageDealt = Predicate(tank).damageDealt;
            DamageTaken = Predicate(tank).damageReceived;
            MaxDamage = Predicate(tank).maxDamage;
            #endregion

            #region [ IStatisticPerformance ]
            Shots = Predicate(tank).shots;
            Hits = Predicate(tank).hits;
            if (Shots > 0)
            {
                HitsPercents = Hits / (double)Shots * 100.0;
            }
            CapturePoints = Predicate(tank).capturePoints;
            DroppedCapturePoints = Predicate(tank).droppedCapturePoints;
            Spotted = Predicate(tank).spotted;
            #endregion

            #region [ ITankRowXP ]
            Xp = Predicate(tank).xp;
            MaxXp = Predicate(tank).maxXP;
            #endregion

            #region [ IStatisticFrags ]
            Frags = Predicate(tank).frags;
            MaxFrags = Predicate(tank).maxFrags;
            Tier8Frags = Predicate(tank).frags8p;
            #endregion

            #region [ IStatisticTime ]
            LastBattle = tank.Common.lastBattleTimeR;
            PlayTime = new TimeSpan(0, 0, 0, tank.Common.battleLifeTime);
            if (Predicate(tank).battlesCount > 0)
            {
                AverageBattleTime = new TimeSpan(0, 0, 0, tank.Common.battleLifeTime / Predicate(tank).battlesCount);
            }
            #endregion

            #region [ IStatisticStun ]
            BattlesOnStunningVehicles = Predicate(tank).battlesOnStunningVehicles;
            StunNum = Predicate(tank).stunNum;
            DamageAssistedStun = Predicate(tank).damageAssistedStun;
            #endregion
        }

        public IEnumerable<FragsJson> TankFrags { get; set; }

        public TankDescription Description { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return TankDescription.Title;
        }

        private string _dossier;

        public string Dossier
        {
            get { return _dossier; }
            set
            {
                _dossier = value;
                OnPropertyChanged(nameof(Dossier));
            }
        }

        private string _replaysUrlOwner;

        public string ReplaysUrlOwner
        {
            get { return _replaysUrlOwner; }
            set
            {
                _replaysUrlOwner = value;
                OnPropertyChanged(nameof(ReplaysUrlOwner));
            }
        }

        private string _replaysUrl;

        public string ReplaysUrl
        {
            get { return _replaysUrl; }
            set
            {
                _replaysUrl = value;
                OnPropertyChanged(nameof(ReplaysUrl));
            }
        }
    }
}