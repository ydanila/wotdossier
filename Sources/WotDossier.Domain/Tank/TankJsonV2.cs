using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using WotDossier.Domain.Interfaces;

namespace WotDossier.Domain.Tank
{
    /// <summary>
    /// 
    /// </summary>
    public class TankJson : ITankDescription
    {
        public static readonly Lazy<TankJson> Initial = new Lazy<TankJson>(()=>new TankJson
        {
            A15x15 = new StatisticJson(),
            Clan = new StatisticJson(),
            Company = new StatisticJson(),
            A7x7 = new StatisticJson(),
            Rated7x7 = new StatisticJson(),
            GlobalMapCommon = new StatisticJson(),
            Fallout = new StatisticJsonFallout(),
            Ranked = new StatisticJson(),
            A30x30 = new StatisticJson(),

            AchievementsClan = new AchievementsClan(),
            
            
            Achievements = new AchievementsJson(),
            Common = new CommonJson
            {
	            countryid = -1,
				tankid = -1,
				compactDescr = DossierUtils.TypeCompDesc(-1,-1)
            },
            Frags = new List<FragsJson>(),
            Achievements7x7 = new Achievements7x7(),
            AchievementsHistorical = new AchievementsHistorical(),
            AchievementsFallout = new AchievementsFallout(),
	        AchievementsRanked = new AchievementsRanked(),
			Historical = new StatisticJson(),
            FortBattles = new StatisticJsonFort(),
            FortAchievements = new AchievementsFort(),
            FortSorties = new StatisticJsonSortie()
        });

        /// <summary>
        /// Gets or sets the clan achievements.
        /// </summary>
        public AchievementsClan AchievementsClan { get; set; }

	    private TankDescription description;
		/// <summary>
		/// Gets or sets the description.
		/// </summary>

		[IgnoreDataMember]
	    public TankDescription Description
	    {
		    get
		    {
			    if (description == null)
			    {
				    description = Dictionaries.Instance.GetTankDescription(Common.compactDescr);
			    }
			    return description;
		    }
	    }

		/// <summary>
		/// Gets or sets the common stat.
		/// </summary>
		public CommonJson Common { get; set; }

        /// <summary>
        /// Gets or sets the a15x15 stat.
        /// </summary>
        public StatisticJson A15x15 { get; set; }

        /// <summary>
        /// Gets or sets the a7x7 stat.
        /// </summary>
        public StatisticJson A7x7 { get; set; }

        /// <summary>
        /// Gets or sets the historical stat.
        /// </summary>
        public StatisticJson Historical { get; set; }

        /// <summary>
        /// Gets or sets the fort battles stat.
        /// </summary>
        public StatisticJsonFort FortBattles { get; set; }

        /// <summary>
        /// Gets or sets the fort sorties.
        /// </summary>
        public StatisticJsonSortie FortSorties { get; set; }

        /// <summary>
        /// Gets or sets the fort achievements.
        /// </summary>
        public AchievementsFort FortAchievements { get; set; }

        /// <summary>
        /// Gets or sets the clan stat.
        /// </summary>
        public StatisticJson Clan { get; set; }

        /// <summary>
        /// Gets or sets the company stat.
        /// </summary>
        public StatisticJson Company { get; set; }

        public StatisticJson Rated7x7 { get; set; }

        public StatisticJson GlobalMapCommon { get; set; }

        public StatisticJsonFallout Fallout { get; set; }

        public StatisticJson Ranked { get; set; }

        public StatisticJson A30x30 { get; set; }

        /// <summary>
        /// Gets or sets the achievements.
        /// </summary>
        public AchievementsJson Achievements { get; set; }

        /// <summary>
        /// Gets or sets the achievements7x7.
        /// </summary>
        public Achievements7x7 Achievements7x7 { get; set; }

        public AchievementsFallout AchievementsFallout { get; set; }

	    public AchievementsRanked AchievementsRanked { get; set; }

		/// <summary>
		/// Gets or sets the achievements historical.
		/// </summary>
		public AchievementsHistorical AchievementsHistorical { get; set; }

        /// <summary>
        /// Gets or sets the frags list.
        /// </summary>
        public IList<IList<string>> FragsList { get; set; }

        /// <summary>
        /// Gets or sets the frags.
        /// </summary>
        public IEnumerable<FragsJson> Frags { get; set; }

        /// <summary>
        /// Gets or sets the raw.
        /// </summary>
        public byte[] Raw { get; set; }

        private int _uniqueId = -1;
        

        public int UniqueId()
        {
            if (_uniqueId == -1)
            {
                _uniqueId = DossierUtils.ToUniqueId(Common.countryid, Common.tankid);
            }
            return _uniqueId;
        }

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        ///     A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return Common.tanktitle;
        }
    }
}