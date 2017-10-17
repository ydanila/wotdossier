using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Windows;
using System.Xml.Linq;
using WotDossier.Common.Extensions;

namespace WotDossier.Domain
{
    public class MapConfig
    {
	    public Rect BoundingBox { get; set; }

        public Dictionary<string, GameplayDescription> GameplayTypes { get; } = new Dictionary<string, GameplayDescription>();

		public MapConfig(XElement boundBox, XElement gameplay)
	    {
		    var bottomLeft = boundBox.Element("bottomLeft").Value.ParsePoint();
			var upperRight = boundBox.Element("upperRight").Value.ParsePoint();

		    BoundingBox = new Rect(bottomLeft.X, bottomLeft.Y, upperRight.X - bottomLeft.X, upperRight.Y - bottomLeft.Y);

		    if (gameplay != null)
		    {
			    foreach (var gm in gameplay.Elements())
			    {
				    GameplayTypes.Add(gm.Name.LocalName, new GameplayDescription(gm));
			    }
		    }

		}
	}
}