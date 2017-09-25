using System;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel.Balancer
{
    public class TankBalanceViewModel : BaseBalanceViewModel
    {
        public int Tier { get; set; }

        public int CountryId { get; set; }

        public TankIcon Icon { get; set; }

        public string Tank { get; set; }        
    }
}
