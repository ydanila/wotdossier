using System;
using System.Runtime.Serialization;
using WotDossier.Domain.Interfaces;

namespace WotDossier.Domain.Tank
{
    public class FragsJson : ITankFilterable
    {
        public int CountryId { get; set; }

        public int TankId { get; set; }

        private TankDescription description;

	    [IgnoreDataMember]
	    public TankDescription TankDescription
	    {
		    get
		    {
			    if (description == null)
			    {
				    int? compDescr = null;
				    if (TankUniqueId != 0)
					    compDescr = DossierUtils.TypeCompDesc(TankUniqueId);
				    else
					    compDescr = DossierUtils.TypeCompDesc(CountryId, TankId);
				    description = Dictionaries.Instance.GetTankDescription(compDescr);
				}
			    return description;
		    }
	    }

		public int TankUniqueId { get; set; }

        public int KilledByTankUniqueId { get; set; }

        public int Count { get; set; }

        public int Type { get; set; }

        public double Tier { get; set; }

	    [IgnoreDataMember]
	    public bool IsPremium => TankDescription.Premium;
        
        public bool IsFavorite { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return $"{TankDescription} - {Count}";
        }
    }
}
