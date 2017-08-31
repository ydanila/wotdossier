using WotDossier.Domain.Interfaces;

namespace WotDossier.Domain.Tank
{
    public class AchievementsFallout : IFalloutAchievements
    {
        public int ShoulderToShoulder { get; set; }
        public int AloneInTheField { get; set; }
        public int FallenFlags { get; set; }
        public int EffectiveSupport { get; set; }
        public int StormLord { get; set; }
        public int WinnerLaurels { get; set; }
        public int Predator { get; set; }
        public int Unreachable { get; set; }
        public int Champion { get; set; }
        public int Bannerman { get; set; }
        public int FalloutDieHard { get; set; }
    }
}