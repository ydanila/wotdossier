using WotDossier.Domain.Tank;

namespace WotDossier.Domain.Interfaces
{
    public interface ITankRowBase
    {
        double Tier { get; set; }
        TankDescription TankDescription { get;}
    }
}