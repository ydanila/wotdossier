namespace WotDossier.Applications.ViewModel.Balancer
{
    public class BaseBalanceViewModel
    {
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

        public double TopPercent => Top357Percent + Top510Percent;

        public double OneLevelPercent => CountPercent(OneLevel, BattlesCount);

        public double Bottom510Percent => CountPercent(Bottom510, BattlesCount);

        public double Bottom357Percent => CountPercent(Bottom357, BattlesCount);

        public int Type { get; internal set; }

        private static double CountPercent(int val, int total)
        {
            if (total == 0)
            {
                return 0;
            }

            return val * 10000 / total / 100.0d;
        }

        internal void Add(BaseBalanceViewModel tanksEntry)
        {
            BattlesCount += tanksEntry.BattlesCount;
            Victory += tanksEntry.Victory;
            Defeat += tanksEntry.Defeat;
            Draw += tanksEntry.Draw;
            Top357 += tanksEntry.Top357;
            Top510 += tanksEntry.Top510;
            OneLevel += tanksEntry.OneLevel;
            Bottom510 += tanksEntry.Bottom510;
            Bottom357 += tanksEntry.Bottom357;
        }
    }
}
