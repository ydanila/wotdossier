using WotDossier.Domain.Replay;

namespace WotDossier.Domain.Interfaces
{
    public interface IReplayMap
    {
        Gameplay Gameplay { get; set; }
        string MapNameId { get; set; }

	    MapDescription Map { get; }

		int Team { get; set; }
    }
}