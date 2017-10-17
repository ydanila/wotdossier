using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WotDossier.Common.Extensions;
using WotDossier.Domain.Tank;

namespace WotDossier.Domain
{
	public class MapDescription: IComparable<MapDescription>
	{
		public Version Version { get; }

		private string UserString { get;}

		public int MapId { get; }

		private string descriptionString { get; }

		public string ImageId { get; }

		public string Key { get; }

		public string Title { get; }

		public string MinimapImage
		{
			get
			{
				return $"pack://application:,,,/WotDossier.Resources;component/Maps/Images/Minimap/{ImageId}.png";
			}
		}

		public string StatImage
		{
			get
			{
				return $"pack://application:,,,/WotDossier.Resources;component/Maps/Images/Stats/{Key}.png";
			}
		}

		public string Description { get; }

		public MapConfig Config { get; }

		internal MapDescription(XElement element, Version version)
		{
			Version = version;
			ImageId = element.Attribute("image").Value;
			Key = element.Attribute("key").Value;
			UserString = element.Element("name").Value;
			descriptionString = element.Element("description").Value;

			Title = ResourceHelper.ResourceManager(UserString.ParseUserString(UserStringPart.Type)).GetString(UserString.ParseUserString(UserStringPart.Key)) ?? Key;
			Description = ResourceHelper.ResourceManager(UserString.ParseUserString(UserStringPart.Type)).GetString((string.IsNullOrEmpty(descriptionString))? Key : descriptionString.ParseUserString(UserStringPart.Key)) ?? Key;
			Version = new Version(element.Parent.Attribute("version").Value);

			Config = new MapConfig(element.Element("boundingBox"), element.Element("gameplayTypes"));
		}

		public int CompareTo(MapDescription other)
		{
			return Key.CompareTo(other.Key);
		}
	}
}
