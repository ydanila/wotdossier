using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Dal;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Rating;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.Logic.Adapter
{
    public abstract class AbstractStatisticAdapter<T> : IStatisticAdapter<T> where T : StatisticEntity
    {
        public int BattlesCount { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int SurvivedBattles { get; set; }
        public int Xp { get; set; }
        public double BattleAvgXp { get; set; }
        public int MaxXp { get; set; }
        public int Frags { get; set; }
        public int MaxFrags { get; set; }
        public int Spotted { get; set; }
        public double HitsPercents { get; set; }
        public int DamageDealt { get; set; }
        public int DamageTaken { get; set; }
        public int MaxDamage { get; set; }
        public int CapturePoints { get; set; }
        public int DroppedCapturePoints { get; set; }
        public double AvgLevel { get; set; }
        public double RBR { get; set; }
        public double WN8Rating { get; set; }
        public double WN8KTTCRating { get; set; }
        public double WN8XVMRating { get; set; }
        public double PerformanceRating { get; set; }
        public DateTime Updated { get; set; }
        public DateTime Created { get; set; }
        public int MarkOfMastery { get; set; }


        public int BattlesOnStunningVehicles { get; set; }
        public int StunNum { get; set; }
        public int DamageAssistedStun { get; set; }

        protected AbstractStatisticAdapter()
        {
        }

        protected AbstractStatisticAdapter(List<TankJson> tanks)
        {
            BattlesCount = tanks.Sum(x => Predicate(x).battlesCount);
            Wins = tanks.Sum(x => Predicate(x).wins);
            Losses = tanks.Sum(x => Predicate(x).losses);
            SurvivedBattles = tanks.Sum(x => Predicate(x).survivedBattles);
            Xp = tanks.Sum(x => Predicate(x).xp);
            if (BattlesCount > 0)
            {
                BattleAvgXp = Xp / (double)BattlesCount;
            }
            MaxXp = tanks.Max(x => Predicate(x).maxXP);
            Frags = tanks.Sum(x => Predicate(x).frags);
            MaxFrags = tanks.Max(x => Predicate(x).maxFrags);
            Spotted = tanks.Sum(x => Predicate(x).spotted);
            HitsPercents = tanks.Sum(x => Predicate(x).hits) / ((double)tanks.Sum(x => Predicate(x).shots)) * 100.0;
            DamageDealt = tanks.Sum(x => Predicate(x).damageDealt);
            DamageTaken = tanks.Sum(x => Predicate(x).damageReceived);
            MaxDamage = tanks.Max(x => Predicate(x).maxDamage);
            CapturePoints = tanks.Sum(x => Predicate(x).capturePoints);
            DroppedCapturePoints = tanks.Sum(x => Predicate(x).droppedCapturePoints);
            MarkOfMastery = tanks.Count(x => (x.Achievements?.MarkOfMastery ?? 0) == (int)Domain.MarkOfMastery.Master);
            Updated = tanks.Max(x => x.Common.lastBattleTimeR);
            if (BattlesCount > 0)
            {
                AvgLevel = tanks.Where(x => x.Common.tier > 0).Sum(x => x.Common.tier * Predicate(x).battlesCount) / (double)BattlesCount;
            }

            PerformanceRating = RatingHelper.PerformanceRating(tanks, Predicate);
            WN8Rating = RatingHelper.Wn8(tanks, WN8Type.Default, Predicate);
            WN8KTTCRating = RatingHelper.Wn8(tanks, WN8Type.KTTC, Predicate);
            WN8XVMRating = RatingHelper.Wn8(tanks, WN8Type.XVM, Predicate);
            RBR = RatingHelper.PersonalRating(tanks, Predicate);

            BattlesOnStunningVehicles = tanks.Max(x => Predicate(x).battlesOnStunningVehicles);
            StunNum = tanks.Max(x => Predicate(x).stunNum);
            DamageAssistedStun = tanks.Max(x => Predicate(x).damageAssistedStun);
        }

        public virtual void Update(T entity)
        {
            entity.BattlesCount = BattlesCount;
            entity.Wins = Wins;
            entity.Losses = Losses;
            entity.SurvivedBattles = SurvivedBattles;
            entity.Xp = Xp;
            entity.BattleAvgXp = BattleAvgXp;
            entity.MaxXp = MaxXp;
            entity.MaxDamage = MaxDamage;
            entity.Frags = Frags;
            entity.MaxFrags = MaxFrags;
            entity.Spotted = Spotted;
            entity.HitsPercents = HitsPercents;
            entity.DamageDealt = DamageDealt;
            entity.DamageTaken = DamageTaken;
            entity.CapturePoints = CapturePoints;
            entity.DroppedCapturePoints = DroppedCapturePoints;
            entity.Updated = Updated;
            entity.AvgLevel = AvgLevel;
            entity.RBR = RBR;
            entity.WN8Rating = WN8Rating;
            entity.WN8KTTCRating = WN8KTTCRating;
            entity.WN8XVMRating = WN8XVMRating;
            entity.PerformanceRating = PerformanceRating;
            entity.MarkOfMastery = MarkOfMastery;

            entity.BattlesOnStunningVehicles = BattlesOnStunningVehicles;
            entity.StunNum = StunNum;
            entity.DamageAssistedStun = DamageAssistedStun;
        }

        public abstract Func<TankJson, StatisticJson> Predicate { get; }
    }
}