﻿using WotDossier.Applications.Parser;

namespace WotDossier.Applications.ViewModel.Replay.Viewer
{
    public class Arena
    {
        private readonly MapGrid _coordinateConvert;
        public double period_length;
        public double clock;
        public double clock_at_period;

        public Arena(MapGrid coordinateConvert)
        {
            _coordinateConvert = coordinateConvert;
        }

        public void update(Packet packet)
        {
            
        }
    }
}
