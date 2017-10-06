using System.Runtime.Serialization;
using WotDossier.Resources;

namespace WotDossier.Domain.Tank
{
	[DataContract]
	public class ShellDescription : ArtefactDescription
    {
	    [DataMember(Name = "kind")]
		public string Kind { get; set; }

		[IgnoreDataMember]
	    public override string Icon => $@"pack://application:,,,/WotDossier.Resources;component/Vehicles/Images/Shell/{Kind}.png";

	    [IgnoreDataMember]
	    public Country Country { get; set; }
		
	}
}