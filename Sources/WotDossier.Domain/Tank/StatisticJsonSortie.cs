using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WotDossier.Domain.Tank
{
    public class StatisticJsonSortie : StatisticJson
    {
        public int middleBattlesCount;
        public int championBattlesCount;
        public int absoluteBattlesCount;
        public int middleWins;
        public int championWins;
        public int absoluteWins;
        public int fortResourceInMiddle;
        public int fortResourceInChampion;
        public int fortResourceInAbsolute;
    }
}
