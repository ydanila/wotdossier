using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WotDossier.Domain.Tank
{
    [DataContract]
    public class RatingExpectancy
    {
        [DataMember(Name = "comp_descr")]
        public int CompDescr { get; set; }

        [DataMember(Name = "tank_title")]
        public string TankTitle { get; set; }

        [DataMember(Name = "icon")]
        public string Icon { get; set; }

        [DataMember(Name = "pr_nominal_damage")]
        public double PRNominalDamage { get; set; }

        [DataMember(Name = "wn8_nominal_damage")]
        public double Wn8NominalDamage { get; set; }

        [DataMember(Name = "wn8_nominal_win_rate")]
        public double Wn8NominalWinRate { get; set; }

        [DataMember(Name = "wn8_nominal_spotted")]
        public double Wn8NominalSpotted { get; set; }

        [DataMember(Name = "wn8_nominal_frags")]
        public double Wn8NominalFrags { get; set; }

        [DataMember(Name = "wn8_nominal_defence")]
        public double Wn8NominalDefence { get; set; }

        [DataMember(Name = "tank_level")]
        public int TankLevel { get; set; }

        [DataMember(Name = "tank_type")]
        public TankType TankType { get; set; }
    }


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
    }
}