using System.Windows.Media.Imaging;

namespace WotDossier.Applications.ViewModel.Balancer
{
    public class TankBalanceViewModel : BaseBalanceViewModel
    {
        public int Tier { get; set; }

        public int CountryId { get; set; }
        
        public string IconId { get; internal set; }

        public string Tank { get; set; }
        
    }
}
