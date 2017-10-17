using System.Collections.Generic;
using System.Resources;
using System.Text;
using WotDossier.Common.Extensions;
using WotDossier.Resources;
using WotDossier.Resources.Strings;

namespace WotDossier.Domain
{
	public static class ResourceHelper
	{
		

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
	}
}
