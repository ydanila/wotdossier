﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using WotDossier.Applications.Logic;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Replay;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel.Replay
{
    public class TeamMember : INotifyPropertyChanged
    {
        private List<int> _achievements;

        private TankDescription _tankDescription;
        public TeamMember(KeyValuePair<long, Player> player, KeyValuePair<long, VehicleResult> vehicleResult, KeyValuePair<long, Vehicle> vehicle, int replayPlayerTeam, string regionCode, Dictionary<long, Player> players, Dictionary<long, VehicleResult> vehicles)
        {
            Id = vehicle.Key;

            TankDescription = Dictionaries.Instance.GetTankDescription(vehicleResult.Value.typeCompDescr);

            TankIcon = TankDescription.Icon;

            LevelRange = TankDescription.LevelRange ?? LevelRange.All;
            Tank = !string.IsNullOrEmpty(TankDescription.Title) ? TankDescription.Title : vehicle.Value.vehicleType;
            ClanAbbrev = vehicle.Value.clanAbbrev;
            Name = vehicle.Value.name;
            FullName = string.Format("{0}{1}", Name, GetClanAbbrev(ClanAbbrev));
            VehicleType = vehicle.Value.vehicleType;
            Team = vehicle.Value.team;
            IsAlive = vehicle.Value.isAlive;

            ClanDBID = player.Value.clanDBID;
            platoonID = player.Value.platoonID;

            AccountDBID = vehicleResult.Value.accountDBID;
            achievements = vehicleResult.Value.achievements;
            BattleMedals = Dictionaries.Instance.GetMedals(vehicleResult.Value.achievements);
            CapturePoints = vehicleResult.Value.capturePoints;
            Credits = vehicleResult.Value.credits;
            DamageAssisted = vehicleResult.Value.damageAssisted;
            DamageDealt = vehicleResult.Value.damageDealt;
            DamageReceived = vehicleResult.Value.damageReceived;
            Damaged = vehicleResult.Value.damaged;
            DroppedCapturePoints = vehicleResult.Value.droppedCapturePoints;
            FreeXp = vehicleResult.Value.freeXP;
            Gold = vehicleResult.Value.gold;
            HEHits = vehicleResult.Value.he_hits;
            Health = vehicleResult.Value.health;
            Hits = vehicleResult.Value.hits;
            IsTeamKiller = vehicleResult.Value.isTeamKiller;
            KillerId = vehicleResult.Value.killerID;
            Kills = vehicleResult.Value.kills;
            LifeTime = vehicleResult.Value.lifeTime;
            Mileage = vehicleResult.Value.mileage;
            Pierced = vehicleResult.Value.pierced;
            PotentialDamageReceived = vehicleResult.Value.potentialDamageReceived;
            DamageBlockedByArmor = vehicleResult.Value.damageBlockedByArmor;
            Repair = vehicleResult.Value.repair;
            Shots = vehicleResult.Value.shots;
            ShotsReceived = vehicleResult.Value.shotsReceived;
            Spotted = vehicleResult.Value.spotted;
            StunNum = vehicleResult.Value.stunNum;
            DamageAssistedStun = vehicleResult.Value.damageAssistedStun;
            TDamage = $"{vehicleResult.Value.tkills}/{vehicleResult.Value.tdamageDealt}";
            TypeCompDescr = vehicleResult.Value.typeCompDescr;
            Xp = vehicleResult.Value.xp;
	        SniperDamageDealt = vehicleResult.Value.sniperDamageDealt;

			TeamMate = Team == replayPlayerTeam;
            
            StatisticLink = string.Format(RatingHelper.NOOBMETER_STATISTIC_LINK_FORMAT, GetServer(regionCode), GetName(regionCode));

	        DeathReason = (DeathReason) vehicleResult.Value.deathReason;

			if(vehicles.TryGetValue(KillerId, out var vehKiller))
			{
				if (players.TryGetValue(vehKiller.accountDBID, out var killer))
					Killer = killer;
			}
            NoDamageDirectHitsReceived = vehicleResult.Value.noDamageShotsReceived;
            PiercingsReceived = vehicleResult.Value.piercedReceived;
            HEHitsReceived = vehicleResult.Value.heHitsReceived;
        }

        private string GetName(string regionCode)
        {
            //if replay from common test
            if ("ct".Equals(regionCode, StringComparison.InvariantCultureIgnoreCase))
            {
                return Name.Substring(0, Name.Length - 3);
            }
            return Name;
        }

        private string GetServer(string regionCode)
        {
            if (!string.IsNullOrEmpty(regionCode) && Dictionaries.Instance.GameServers.ContainsKey(regionCode.ToLowerInvariant()))
            {
                return regionCode.ToLowerInvariant();
            }

            string server = Name.Substring(Name.Length - 2).ToLowerInvariant();

            if (Dictionaries.Instance.GameServers.ContainsKey(server))
            {
                return server;
            }

            return SettingsReader.Get().Server;
        }

        public LevelRange LevelRange { get; set; }
        public string StatisticLink { get; set; }

        private string GetClanAbbrev(string abbrev)
        {
            if (!string.IsNullOrEmpty(abbrev))
            {
                return string.Format("[{0}]", abbrev);
            }
            return string.Empty;
        }

        public List<Medal> BattleMedals { get; set; }

        public string Tank { get; set; }

        public TankIcon TankIcon { get; set; }

        public long Id { get; set; }
        public string FullName { get; set; }
        
        public string ClanAbbrev { get; set; }
        public long ClanDBID { get; set; }
        public string Name { get; set; }
        public long platoonID { get; set; }
        public int Team { get; set; }

        public long AccountDBID { get; set; }
		public List<int> achievements
		{
			get { return _achievements ?? new List<int>(); }
			set { _achievements = value; }
		}

		public int CapturePoints { get; set; }
        public int Credits { get; set; }
        public int DamageAssisted { get; set; }
        public int DamageDealt { get; set; }
        public int DamageReceived { get; set; }
        public int Damaged { get; set; }
        public int DroppedCapturePoints { get; set; }
        public int FreeXp { get; set; }
        public int Gold { get; set; }
        public int HEHits { get; set; }
        public int HEHitsReceived { get; set; }
        public int Health { get; set; }
        public int Hits { get; set; }
	    public string HitsPenetrations => $"{Hits}/{Pierced}";
        public string DamagedDestroyed => $"{Damaged}/{Kills}";

        public string CaptureDefensePoints => $"{CapturePoints}/{DroppedCapturePoints}";
        public bool IsTeamKiller { get; set; }
        public int KillerId { get; set; }
        public int Kills { get; set; }
        public int LifeTime { get; set; }
        public int Mileage { get; set; }
        public int Pierced { get; set; }
        public int PotentialDamageReceived { get; set; }
        public int DamageBlockedByArmor { get; set; }
        public int Repair { get; set; }
	    public int SniperDamageDealt { get; set; }
		public int Shots { get; set; }
        public int ShotsReceived { get; set; }
        public int PiercingsReceived { get; set; }
        public int NoDamageDirectHitsReceived { get; set; }
        public int Spotted { get; set; }
        public int StunNum { get; set; }
        public int DamageAssistedStun { get; set; }
        public string TDamage { get; set; }
        //public int team { get; set; }
        public int? TypeCompDescr { get; set; }
        public int Xp { get; set; }


        //public string clanAbbrev { get; set; }
        //        "events": {}, 
        public bool IsAlive { get; set; }
        //public bool isTeamKiller { get; set; }
        //public string name { get; set; }
        //public int team { get; set; }
        public string VehicleType { get; set; }

        public int Squad { get; set; }

	    public DeathReason DeathReason { get; set; }

	    public Player Killer { get; set; }

		public int AchievementsCount => BattleMedals.Count;

	    public string AchievementsList => string.Join("\r\n", BattleMedals.Select(m => m.Name));


	    public bool TeamMate { get; set; }

        public TankDescription TankDescription
        {
            get { return _tankDescription; }
            set { _tankDescription = value; }
        }

        public override string ToString()
        {
            return LevelRange.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

	    public string DeathType
	    {
		    get
		    {
			   return Resources.Resources.ResourceManager.GetEnumResource(DeathReason) + ((Killer != null)
					    ? (" (" + Killer.name + ((!string.IsNullOrEmpty(Killer.clanAbbrev)) ? "[" + Killer.clanAbbrev + "]" : "") + ")")
					    : "");

		    }
	    }
    }
}