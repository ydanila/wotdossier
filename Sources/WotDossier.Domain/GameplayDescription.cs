using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Xml.Linq;
using WotDossier.Common.Extensions;

namespace WotDossier.Domain
{
    public class GameplayDescription
    {
	    /*
		 <domination>
          <teamSpawnPoints>
            <team1>
              <position>459.100 1.500</position>
            </team1>
            <team2>
              <position>46.100 449.000</position>
            </team2>
          </teamSpawnPoints>
          <controlPoint>	-254,900000 -295,000000	</controlPoint>
          <teamBasePositions>
            <team1></team1>
            <team2></team2>
          </teamBasePositions>
        </domination>

		 */
		public Dictionary<int, List<Point>> TeamSpawnPoints { get; }

	    public Dictionary<int, List<Point>> TeamBasePositions { get; }

	    public Point? ControlPoint { get; }


	    public GameplayDescription(XElement element)
	    {
		    var child = element.Element("teamSpawnPoints");
		    if (child != null)
		    {
			    TeamSpawnPoints = child.Elements().ToDictionary(x => Convert.ToInt32(x.Name.LocalName.Replace("team", "")),
				    pos => pos.Elements().Select(x => x.Value.ParsePoint()).ToList());
		    }
		    else
			    TeamSpawnPoints = new Dictionary<int, List<Point>>();


		    child = element.Element("teamBasePositions");
		    if (child != null)
		    {
			    TeamBasePositions = child.Elements().ToDictionary(x => Convert.ToInt32(x.Name.LocalName.Replace("team", "")),
				    pos => pos.Elements().Select(x => x.Value.ParsePoint()).ToList());
		    }
		    else
			    TeamBasePositions = new Dictionary<int, List<Point>>();

		    child = element.Element("controlPoint");
		    if (child != null)
		    {
			    ControlPoint = child.Value.ParsePoint();
		    }
	    }
    }
}