using System.Collections.Generic;
using WotDossier.Domain.Dossier.TankV77;
using WotDossier.Domain.Dossier.TankV85;

namespace WotDossier.Domain.Dossier.TankV87
{
    /*0.9.2*/
    public class TankJson87 : TankJson85
    {
	    public TankJson87()
	    {
		    Achievements = new Achievements_87();
		    FortAchievements = new AchievementsFort_87();
		}

	    #region Achievements

	    public new Achievements_87 Achievements
	    {
		    get => (Achievements_87)base.Achievements;
		    set => base.Achievements = value;
	    }

	    public new AchievementsFort_87 FortAchievements
	    {
		    get => (AchievementsFort_87) base.FortAchievements;
		    set => base.FortAchievements = value;
	    }

	    #endregion
    }
}
