﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WotDossier.Domain.Tank
{
    public class StatisticJsonFort : StatisticJson
    {
        public int reservedInt32;
        public int attackCount;
        public int defenceCount;
        public int enemyBaseCaptureCount;
        public int ownBaseLossCount;
        public int ownBaseLossCountInDefence;
        public int enemyBaseCaptureCountInAttack;
        public int combatCount;
        public int combatWins;
        public int successDefenceCount;
        public int successAttackCount;
        public int captureEnemyBuildingTotalCount;
        public int lossOwnBuildingTotalCount;
        public int resourceCaptureCount;
        public int resourceLossCount;
    }
}
