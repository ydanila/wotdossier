using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WotDossier.Common.Extensions
{
	public static class StringExtensions
	{
		public static string TrimAll(this string value)
		{
			return string.IsNullOrEmpty(value) ? value : value.Trim(' ', '\r', '\n', '\t');
		}
	}
}
