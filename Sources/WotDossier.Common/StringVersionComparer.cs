using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WotDossier.Common
{
	public class StringVersionComparer : IComparer<string>
	{
		public static StringVersionComparer Default { get; } = new StringVersionComparer();
		public int Compare(string x, string y)
		{
			if (x == null || !Version.TryParse(x, out var xVer)) return -1;
			if (y == null || !Version.TryParse(y, out var yVer)) return 1;

			return xVer.CompareTo(yVer);
		}

		public int Compare(string x, Version y)
		{
			return Compare(x, y.ToString());
		}
	}
}
