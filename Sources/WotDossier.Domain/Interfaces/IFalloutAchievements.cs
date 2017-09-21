namespace WotDossier.Domain.Interfaces
{
    public interface IFalloutAchievements
    {
        int ShoulderToShoulder { get; set; }
        int AloneInTheField { get; set; }
        int FallenFlags { get; set; }
        int EffectiveSupport { get; set; }
        int StormLord { get; set; }
        int WinnerLaurels { get; set; }
        int Predator { get; set; }
        int Unreachable { get; set; }
        int Champion { get; set; }
        int Bannerman { get; set; }
        int FalloutDieHard { get; set; }
    }
}