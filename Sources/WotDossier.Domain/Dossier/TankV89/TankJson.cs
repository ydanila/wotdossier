using System.Collections.Generic;
using WotDossier.Domain.Dossier.TankV77;
using WotDossier.Domain.Dossier.TankV85;
using WotDossier.Domain.Dossier.TankV87;
using WotDossier.Domain.Dossier.TankV88;

namespace WotDossier.Domain.Dossier.TankV89
{
    public class TankJson89 : TankJson88
    {
	    public TankJson89()
	    {
		    SingleAchievements = new AchievementsSingle89();
	    }
		#region Achievements

		public new AchievementsSingle89 SingleAchievements
	    {
		    get => (AchievementsSingle89)base.SingleAchievements;
		    set => base.SingleAchievements = value;
	    }

		#endregion
	}
}
