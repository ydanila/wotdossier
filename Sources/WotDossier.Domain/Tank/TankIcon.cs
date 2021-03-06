﻿using System.Runtime.Serialization;

namespace WotDossier.Domain.Tank
{
    [DataContract]
    public class TankIcon
    {
        public static TankIcon Empty = new TankIcon{Icon = "tank", CountryId = Country.Unknown};

        /// <summary>
        /// Gets the icon id.
        /// </summary>
        public string IconId => $"{CountryId.ToString().ToLower()}-{Icon}";

	    /// <summary>
	    /// Gets the icon Key.
	    /// </summary>
	    public string IconKey => $"{CountryId.ToString().ToLower()}_{Icon.ToLower().Replace("-", "_")}";

	    public Country Country { get; set; }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        [DataMember(Name = "icon")]
        public string Icon { get; set; }

        
        public int CompDescr { get; set; }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        protected bool Equals(TankIcon other)
        {
            return CountryId == other.CountryId && string.Equals(IconOrig, other.IconOrig);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" }, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TankIcon) obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) CountryId*397) ^ (IconOrig != null ? IconOrig.GetHashCode() : 0);
            }
        }
    }
}
