using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WotDossier.Common.Extensions
{
	public enum UserStringPart
	{
		Type = 0,
		Key = 1
	}

	public static class XmlParseExtensions
	{
		public static string ParseUserString(this string userString, UserStringPart part, bool full = true)
		{
			var parts = userString.Split(':');
			if (part == UserStringPart.Type)
			{
				if (!full)
					return parts[0].Split('_')[0].Substring(1);
				return parts[0].Substring(1);
			}
			if (!full)
				return parts[1].Split('/')[0];
			return parts[1];
		}

		public static Point ParsePoint(this string value)
		{
			var values = value.Split(' ');
			Point point = new Point(ConvertToDouble(values[0]), ConvertToDouble(values[1]));
			return point;
		}

		private static double ConvertToDouble(string value)
		{
			return Double.Parse(value.Replace(".", ",").TrimAll(), CultureInfo.CurrentCulture);
		}
	}
}
