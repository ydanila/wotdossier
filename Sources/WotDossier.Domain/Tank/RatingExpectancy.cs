using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WotDossier.Domain.Tank
{
    [DataContract]
    public class RatingExpectedValues
    {
        [DataMember(Name = "header")]
        public RatingExpectedValuesHeader Header { get; set; }

        [DataMember(Name = "data")]
        public List<RatingExpectedValuesData> Data { get; set; }
    }

    [DataContract]
    public class RatingExpectedValuesHeader
    {
        [DataMember(Name = "version")]
        public double Version { get; set; }
    }

    [DataContract]
    public class RatingExpectedValuesData
    {
        [DataMember(Name = "IDNum")]
        public int CompDescr { get; set; }

        [DataMember(Name = "expFrag")]
        public double Frags { get; set; }

        [DataMember(Name = "expDamage")]
        public double Damage { get; set; }

        [DataMember(Name = "expSpot")]
        public double Spotted { get; set; }

        [DataMember(Name = "expDef")]
        public double Defence { get; set; }

        [DataMember(Name = "expWinRate")]
        public double WinRate { get; set; }

        [IgnoreDataMember]
        public int TankLevel { get; set; }

        [IgnoreDataMember]
        public int TankType { get; set; }

	    [IgnoreDataMember]
	    public int CountryId { get; set; }
	}
}