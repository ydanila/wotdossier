﻿using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace WotDossier.Domain.Replay
{
    [DataContract]
    public class Personal : VehicleResult
    {
        [DataMember]
        public int aogasFactor10 { get; set; }
        [DataMember]
        public List<int> autoEquipCost { get; set; }
        [DataMember]
        public List<int> autoLoadCost { get; set; }
        [DataMember]
        public int? autoRepairCost { get; set; }
        [DataMember]
        public int creditsContributionIn { get; set; }
        [DataMember]
        public int creditsContributionOut { get; set; }
        [DataMember]
        public int creditsPenalty { get; set; }
        [DataMember]
        public int dailyXPFactor10 { get; set; }
        [DataMember]
        public Dictionary<string, DamagedVehicle> details { get; set; }
        [DataMember]
        public List<List<JValue>> dossierPopUps { get; set; }
        [DataMember]
        public int eventCredits { get; set; }
	    [DataMember]
	    public int boosterCredits { get; set; }

	    [DataMember]
	    public int orderCredits { get; set; }
	    [DataMember]
	    public int achievementCredits { get; set; }
		[DataMember]
        public int eventFreeXP { get; set; }
	    [DataMember]
	    public int boosterXP { get; set; }
	    [DataMember]
	    public int boosterFreeXP { get; set; }
	    [DataMember]
	    public int achievementXP { get; set; }
	    [DataMember]
	    public int achievementFreeXP { get; set; }
		[DataMember]
	    public int orderXP { get; set; }
	    [DataMember]
	    public int orderFreeXP { get; set; }
		[DataMember]
        public int eventGold { get; set; }
        [DataMember]
        public int eventTMenXP { get; set; }
        [DataMember]
        public int eventXP { get; set; }
        [DataMember]
        public bool isPremium { get; set; }
        [DataMember]
        public int markOfMastery { get; set; }
        [DataMember]
        public int originalCredits { get; set; }
        [DataMember]
        public int originalFreeXP { get; set; }
        [DataMember]
        public int originalXP { get; set; }
        [DataMember]
        public int premiumCreditsFactor10 { get; set; }
        [DataMember]
        public int premiumXPFactor10 { get; set; }
        [DataMember]
        public int tmenXP { get; set; }
        [DataMember]
        public int xpPenalty { get; set; }
      
        [DataMember]
        public int originalCrystal { get; set; }
        
        [DataMember]
        public int crystal { get; set; }
        
        [DataMember]
        public int subtotalCrystal { get; set; }
        
        [DataMember]
        public int eventCrystal { get; set; }
        
    }
}