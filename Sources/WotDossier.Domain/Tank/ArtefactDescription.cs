using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WotDossier.Common.Extensions;

namespace WotDossier.Domain.Tank
{
	[DataContract]
	public class ArtefactDescription
	{
		[DataMember(Name = "icon")]
		public string iconString { get; set; }

		[DataMember(Name = "description")]
		public string descriptionString { get; set; }

		[DataMember(Name = "tags")]
		public string tagsString { get; set; }

		[DataMember(Name = "userString")]
		public string UserString { get; set; }

		[DataMember(Name = "notInShop")]
		public bool NotInShop { get; set; }

		[DataMember(Name = "id")]
		public int Id { get; set; }

		[IgnoreDataMember]
		public string Key => UserString.ParseUserString(UserStringPart.Key);

		[IgnoreDataMember]
		public string Title =>
			ResourceHelper.ResourceManager(UserString.ParseUserString(UserStringPart.Type)).GetString(Key) ?? Key;

		[IgnoreDataMember]
		public virtual string Icon
		{
			get
			{
				var parts = iconString.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
				parts[0] = parts[0].Replace("../maps/icons/artefact",
					"pack://application:,,,/WotDossier.Resources;component/Vehicles/Images/Artefact");
				if (!parts[0].Contains("/"))
					parts[0] = $"pack://application:,,,/WotDossier.Resources;component/Vehicles/Images/Artefact/{parts[0]}.png";
				return String.Join(" ", parts);
			}
		}

		[IgnoreDataMember]
		public string Description
		{
			get
			{
				var key = (string.IsNullOrEmpty(descriptionString))
					? Key
					: descriptionString.ParseUserString(UserStringPart.Key);


				return ResourceHelper.ResourceManager(UserString.ParseUserString(UserStringPart.Type)).GetString(key) ?? Key;
			}
		}
	}
}