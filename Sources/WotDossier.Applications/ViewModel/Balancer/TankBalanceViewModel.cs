using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel.Balancer
{
    public class TankBalanceViewModel
    {
        public int Tier { get; set; }

        public int CountryId { get; set; }

        public TankIcon Icon { get; set; }

        public string Tank { get; set; }

        public int BattlesCount { get; set; }

        public int Victory { get; set; }

        public int Defeat { get; set; }

        public int Draw { get; set; }

        public int FinishedBattles
        {
            get { return Victory + Draw + Defeat; }
        }

        public double WinsPercent => CountPercent(Victory, FinishedBattles);
        
        public int Top357 { get; set; }

        public int Top510 { get; set; }

        public int OneLevel { get; set; }

        public int Bottom510 { get; set; }

        public int Bottom357 { get; set; }

        public double Top357Percent => CountPercent(Top357, BattlesCount);

        public double Top510Percent => CountPercent(Top510, BattlesCount);

        public double OneLevelPercent => CountPercent(OneLevel, BattlesCount);

        public double Bottom510Percent => CountPercent(Bottom510, BattlesCount);

        public double Bottom357Percent => CountPercent(Bottom357, BattlesCount);

        private static double CountPercent(int val, int total)
        {
            if (total == 0)
            {
                return 0;
            }

            return val * 10000 / total / 100.0d;
        }
    }
}
