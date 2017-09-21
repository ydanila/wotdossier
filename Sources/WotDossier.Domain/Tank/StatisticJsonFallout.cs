using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WotDossier.Domain.Tank
{
    public class StatisticJsonFallout : StatisticJson
    {
        public int winPoints;
        public int flagCapture;
        public int soloFlagCapture;
        public int coins;
        public int avatarDamageDealt;
        public int avatarKills;
        public int resourceAbsorbed;
        public int deathCount;

        public int maxWinPoints;
        public int maxCoins;
    }
}
