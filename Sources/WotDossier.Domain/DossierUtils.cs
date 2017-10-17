using System;
using WotDossier.Domain.Tank;

namespace WotDossier.Domain
{
    public static class DossierUtils
    {
	    private const int UNKNOWN_DESCR = 65521;

		public static int ToUniqueId(int typeCompDescr)
		{
			if (typeCompDescr == UNKNOWN_DESCR) return -10001;
            int tankId = ToTankId(typeCompDescr);
            int countryId = ToCountryId(typeCompDescr);

            return ToUniqueId(countryId, tankId);
        }

        public static int ToCountryId(int typeCompDescr)
        {
	        if (typeCompDescr == UNKNOWN_DESCR) return -1;
			return typeCompDescr >> 4 & 15;
        }

        public static int ToTankId(int typeCompDescr)
        {
	        if (typeCompDescr == UNKNOWN_DESCR) return -1;
			return typeCompDescr >> 8 & 65535;
        }

        public static int ToTypeId(int typeCompDescr)
        {
            return typeCompDescr & 15;
        }

        public static int TypeCompDesc(int countryId, int tankId, int type = 1)
        {
            return (type & 15) | (countryId << 4 & 255) | (tankId << 8 & 65535);
        }

	    public static int TypeCompDesc(int uniqueId)
	    {
		    var tankid = uniqueId % 10000;
		    var countryid = (uniqueId - tankid) / 10000;
			return TypeCompDesc(countryid, tankid);
	    }

		public static int ToUniqueId(int countryId, int tankId)
        {
            return countryId * 10000 + tankId;
        }

	    public static int ToUniqueId(Country country, int tankId)
	    {
		    return ToUniqueId((int)country, tankId);
	    }

		public static Country ToCountry(this string country)
	    {
		    return (Country) Enum.Parse(typeof(Country), char.ToUpper(country[0]) + country.Substring(1));

	    }
    }
}
