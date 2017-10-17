using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using WotDossier.Common.Extensions;
using WotDossier.Domain.Rating;

namespace WotDossier.Domain.Tank
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class TankDescription : IEquatable<TankDescription>, IComparable<TankDescription>
	{
        public const string UNKNOWN = "Unknown";

        public static TankDescription Unknown(string iconId = null)
        {
	        return new TankDescription { Title = iconId ?? UNKNOWN, Key = "tank", CompDescr = 65521, CountryId = -1, TankId = -1, Hidden = true };
		}

        public static TankDescription Total(string rowHeader)
	    {
		    return new TankDescription { Title = rowHeader, Key = String.Empty, CompDescr = 65521, CountryId  = -1, TankId = -1 };
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
	    /// Gets country.
	    /// </summary>
	    public Country Country => (Country) CountryId;

	    /// <summary>
		/// Gets or sets the type.
		/// </summary>
		[DataMember(Name = "type")]
        public int Type { get; set; }

        public int Tier { get; set; }

        public bool Premium { get; set; }

		private string _title = "";

		public string Title
		{
			get
			{
				if(string.IsNullOrEmpty(_title))
					_title = ResourceHelper.ResourceManager(userString.ParseUserString(UserStringPart.Type)).GetString(userString.ParseUserString(UserStringPart.Key)) ?? Key;
				return _title;
			}
			set => _title = value;
		}

		private string description = "";
		public string Description
		{
			get
			{
				if (string.IsNullOrEmpty(description))
					description = ResourceHelper.ResourceManager(descriptionString.ParseUserString(UserStringPart.Type)).GetString(descriptionString.ParseUserString(UserStringPart.Key)) ?? Key;
				return description;
			}
		}

	    public string Key { get; set; }

	    public bool Secret { get; set; }

		public bool Hidden { get; set; }

		public string userString { get; set; } = "";


		public string descriptionString { get; set; } = "";


	    public string IconId => $"{Country.ToString().ToLower()}-{Dictionaries.Instance.AllVehicles[UniqueId].Key}";

		public string CountryKey => $"{Country.ToString().ToLower()}-{Key}";

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


		public Version Version { get; set; }

		private int _uniqueId = -1;

        /// <summary>
        /// Uniques the id.
        /// </summary>
        public int UniqueId
        {
            get
            {
                if (_uniqueId == -1)
                {
                    _uniqueId = DossierUtils.ToUniqueId(CountryId, TankId);
                }
                return _uniqueId;
            }
        }

		public int CompareTo(TankDescription other)
		{
			return UniqueId.CompareTo(other.UniqueId);
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


		public bool Equals(TankDescription other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return CompDescr == other.CompDescr;
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
			return CompDescr;
		}
	}
}
