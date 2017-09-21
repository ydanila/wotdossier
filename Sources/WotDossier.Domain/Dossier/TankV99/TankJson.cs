﻿using System.Collections.Generic;
using WotDossier.Domain.Dossier.TankV77;
using WotDossier.Domain.Dossier.TankV95;
using WotDossier.Domain.Dossier.TankV97;
using WotDossier.Domain.Dossier.TankV98;

namespace WotDossier.Domain.Dossier.TankV99
{
    /// <summary>
    /// 0.9.20.0
    /// </summary>
    public class TankJson99 : TankJson98
    {
        public StatisticJson95 A30x30 { get; set; } = new StatisticJson95();

        public MaxJson77 Max30x30 { get; set; } = new MaxJson77();
    }
}
