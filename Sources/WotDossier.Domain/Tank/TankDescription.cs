﻿using System.Collections.Generic;
using System.Runtime.Serialization;
using WotDossier.Domain.Rating;

namespace WotDossier.Domain.Tank
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class TankDescription
    {
        private const string UNKNOWN = "Unknown";

        public static TankDescription Unknown(string iconId = null)
        {
            return new TankDescription { Title = iconId ?? UNKNOWN, Icon = TankIcon.Empty };
        }

        public static TankDescription Unknown(int compDescr)
        {
            return new TankDescription { Title = UNKNOWN, Icon = TankIcon.Empty, CompDescr = compDescr, CountryId = DossierUtils.ToCountryId(compDescr), TankId = DossierUtils.ToTankId(compDescr) };
        }

        public static TankDescription Unknown(int countryId, int tankId)
        {
            return new TankDescription { Title = UNKNOWN, Icon = TankIcon.Empty, CountryId  = countryId, TankId = tankId };
        }

        /// <summary>
        /// Gets or sets the tank id.
        /// </summary>
        [DataMember(Name = "tankid")]
        public int TankId { get; set; }

        /// <summary>
        /// Gets or sets the country id.
        /// </summary>
        [DataMember(Name = "countryid")]
        public int CountryId { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        [DataMember(Name = "type")]
        public int Type { get; set; }

        /// <summary>
        /// Gets or sets the tier.
        /// </summary>
        [DataMember(Name = "tier")]
        public int Tier { get; set; }

        /// <summary>
        /// Gets or sets the premium.
        /// </summary>
        [DataMember(Name = "premium")]
        public int Premium { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        [DataMember(Name = "title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the comp descr.
        /// </summary>
        [DataMember(Name = "compDescr")]
        public int CompDescr { get; set; }

        /// <summary>
        /// Gets or sets the health.
        /// </summary>
        [DataMember(Name = "health")]
        public int Health { get; set; }

        public LevelRange LevelRange { get; set; }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        [IgnoreDataMember]
        public TankIcon Icon { get; set; }

        [DataMember(Name = "active")]
        public bool Active { get; set; }

        private RatingExpectancy _expectancy;
        /// <summary>
        /// Gets or sets the rating expectancy.
        /// </summary>
        [IgnoreDataMember]
        public RatingExpectancy Expectancy
        {
            get { return _expectancy ?? new RatingExpectancy(); }
            set { _expectancy = value; }
        }


        private Dictionary<WN8Type, RatingExpectedValuesData> expectedValues;
        /// <summary>
        /// Gets or sets the rating expected values.
        /// </summary>
        [IgnoreDataMember]
        public Dictionary<WN8Type, RatingExpectedValuesData> ExpectedValues
        {
            get
            {
                if (expectedValues == null)
                {
                    expectedValues = new Dictionary<WN8Type, RatingExpectedValuesData>()
                    {
                        {WN8Type.Default, new RatingExpectedValuesData()},
                        { WN8Type.KTTC, new RatingExpectedValuesData() },
                        { WN8Type.XVM, new RatingExpectedValuesData() }
                    };
                }
                return expectedValues;
            }
            set => expectedValues = value;
        }

        private int _uniqueId = -1;
        /// <summary>
        /// Uniques the id.
        /// </summary>
        public int UniqueId()
        {
            if (_uniqueId == -1)
            {
                _uniqueId = DossierUtils.ToUniqueId(CountryId, TankId);
            }
            return _uniqueId;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return Title;
        }

        protected bool Equals(TankDescription other)
        {
            return TankId == other.TankId && CountryId == other.CountryId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TankDescription) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (TankId*397) ^ CountryId;
            }
        }
    }
}
