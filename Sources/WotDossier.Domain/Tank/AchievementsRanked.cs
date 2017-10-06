using System.Runtime.Serialization;
using WotDossier.Domain.Interfaces;

namespace WotDossier.Domain.Tank
{
    [DataContract]
    public class AchievementsRanked : IRankedBattlesAchievements
    {
        [DataMember(Name = "rank")]
        public int Rank { get; set; }
    }
}