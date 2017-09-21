using System.Collections.Generic;
using WotDossier.Domain.Dossier.TankV77;
using WotDossier.Domain.Dossier.TankV88;
using WotDossier.Domain.Dossier.TankV89;

namespace WotDossier.Domain.Dossier.TankV92
{
    /*0.9.2*/
    public class TankJson92 : TankJson89
    {
        public StatisticJson77 GlobalMapCommon { get; set; } = new StatisticJson77();

        public MaxJson77 MaxGlobalMapCommon { get; set; } = new MaxJson77();

        #region Achievements

        

        #endregion
    }
}
