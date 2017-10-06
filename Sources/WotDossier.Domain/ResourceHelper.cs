using System.Collections.Generic;
using System.Resources;
using System.Text;
using WotDossier.Resources;
using WotDossier.Resources.Strings;

namespace WotDossier.Domain
{
	public enum UserStringPart
	{
		Type = 0,
		Key = 1
	}
	public static class ResourceHelper
	{
		public static string ParseUserString(this string userString, UserStringPart part, bool full = true)
		{
			var parts = userString.Split(':');
			if (part == UserStringPart.Type)
			{
				if(!full)
					return parts[0].Split('_')[0].Substring(1);
				return parts[0].Substring(1);
			}
			if (!full)
				return parts[1].Split('/')[0];
			return parts[1];
		}


		public static string CorrectResourceName(string key)
		{
			var capit = true;
			var sb = new StringBuilder();
			foreach (var ch in key)
			{
				if (ch == '_')
				{
					capit = true;
					continue;
				}
				var c = ch.ToString();
				if (capit)
					c = c.ToUpper();
				sb.Append(c);
				capit = false;
			}
			return sb.ToString();
		}

		private static Dictionary<string, ResourceManager> resourceManagers = new Dictionary<string, ResourceManager>();

		public static ResourceManager ResourceManager(Country country)
		{
			var prefix = country.ToString();
			if (country == Country.Uk)
				prefix = "Gb";
			return ResourceManager(prefix + "Vehicles");
		}

		public static ResourceManager ResourceManager(string key)
		{
			if (resourceManagers.TryGetValue(key, out var rm))
				return rm;

			rm = new ResourceManager($"{typeof(BattleResults).Namespace}.{CorrectResourceName(key)}", typeof(BattleResults).Assembly);
			resourceManagers.Add(key, rm);
			return rm;
		}
	}
}
