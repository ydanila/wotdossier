using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Common.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using WotDossier.Common;
using WotDossier.Domain.Rating;
using WotDossier.Domain.Tank;
using WotDossier.Resources;

namespace WotDossier.Domain
{
    /// <summary>
    /// Common app dictionaries
    /// </summary>
    public class Dictionaries
    {
        private static readonly ILog _log = LogManager.GetLogger<Dictionaries>();

        private static readonly object _syncObject = new object();
        
        public static readonly Version VersionAll = new Version("100.0.0.0");
        public static readonly Version VersionRelease = new Version("0.9.20.1");
        public static readonly Version VersionTest = new Version("0.9.21.0");

        private static readonly List<Version> _versions = new List<Version>
        {
                VersionRelease,
				new Version("0.9.20.0"),
				new Version("0.9.19.0"),
                new Version("0.9.18.0"),
                new Version("0.9.17.0"),
                new Version("0.9.16.0"),
                new Version("0.9.15.1"),
                new Version("0.9.15.0"),
                new Version("0.9.14.0"),
                new Version("0.9.13.0"),
                new Version("0.9.12.0"),
                new Version("0.9.10.0"),
                new Version("0.9.9.0"),
                new Version("0.9.8.0"),
                new Version("0.9.7.0"),
                new Version("0.9.6.0"),
                new Version("0.9.5.0"),
                new Version("0.9.4.0"),
                new Version("0.9.3.0"),
                new Version("0.9.2.0"),
                new Version("0.9.1.0"),
                new Version("0.9.0.0"),
                new Version("0.8.11.0"), 
                new Version("0.8.10.0"),
                new Version("0.8.9.0"),
                new Version("0.8.8.0"),
                new Version("0.8.7.0"),
                new Version("0.8.6.0"),
                new Version("0.8.5.0"),
                new Version("0.8.4.0"),
                new Version("0.8.3.0"),
                new Version("0.8.2.0"),
                new Version("0.8.1.0"),
				new Version("0.8.0.0"),
				new Version("0.7.5.0"),
				new Version("0.7.4.0"),
				new Version("0.7.3.0"),
				new Version("0.7.2.0"),
				new Version("0.7.1.0"),
				new Version("0.7.0.0"),
		};

	    private static Dictionaries _instance;

		#region BattleLevels

		/// <summary>
		/// http://forum.worldoftanks.ru/index.php?/topic/41221-
		/// </summary>
		private readonly Dictionary<int, Dictionary<TankType, LevelRange>> _tankLevelsMap = new Dictionary<int, Dictionary<TankType, LevelRange>>
        {
            {
                1, new Dictionary<TankType, LevelRange>
                {
                    {TankType.LT, new LevelRange {Min = 1, Max = 2}},
                    {TankType.MT, new LevelRange {Min = 1, Max = 2}},
                }
            },
            {
                2, new Dictionary<TankType, LevelRange>
                {
                    {TankType.LT, new LevelRange{Min = 2, Max = 3}},
                    {TankType.MT, new LevelRange{Min = 2, Max = 3}},
                    {TankType.SPG, new LevelRange{Min = 2, Max = 3}},
                    {TankType.TD, new LevelRange{Min = 2, Max = 3}},
                }
            },
            {
                3, new Dictionary<TankType, LevelRange>
                {
                    {TankType.LT, new LevelRange{Min = 3, Max = 4}},
                    {TankType.MT, new LevelRange{Min = 3, Max = 4}},
                    {TankType.SPG, new LevelRange{Min = 3, Max = 4}},
                    {TankType.TD, new LevelRange{Min = 3, Max = 4}},
                }
            },
            {
                4, new Dictionary<TankType, LevelRange>
                {
                    {TankType.LT, new LevelRange{Min = 4, Max = 6}},
                    {TankType.MT, new LevelRange{Min = 4, Max = 6}},
                    {TankType.HT, new LevelRange{Min = 4, Max = 5}},
                    {TankType.SPG, new LevelRange{Min = 4, Max = 6}},
                    {TankType.TD, new LevelRange{Min = 4, Max = 6}},
                }
            },
            {
                5, new Dictionary<TankType, LevelRange>
                {
                    {TankType.LT, new LevelRange{Min = 5, Max = 7}},
                    {TankType.MT, new LevelRange{Min = 5, Max = 7}},
                    {TankType.HT, new LevelRange{Min = 5, Max = 7}},
                    {TankType.SPG, new LevelRange{Min = 5, Max = 7}},
                    {TankType.TD, new LevelRange{Min = 5, Max = 7}},
                }
            },
            {
                6, new Dictionary<TankType, LevelRange>
                {
                    {TankType.LT, new LevelRange{Min = 6, Max = 8}},
                    {TankType.MT, new LevelRange{Min = 6, Max = 8}},
                    {TankType.HT, new LevelRange{Min = 6, Max = 8}},
                    {TankType.SPG, new LevelRange{Min = 6, Max = 8}},
                    {TankType.TD, new LevelRange{Min = 6, Max = 8}},
                }
            },
            {
                7, new Dictionary<TankType, LevelRange>
                {
                    {TankType.LT, new LevelRange{Min = 7, Max = 9}},
                    {TankType.MT, new LevelRange{Min = 7, Max = 9}},
                    {TankType.HT, new LevelRange{Min = 7, Max = 9}},
                    {TankType.SPG, new LevelRange{Min = 7, Max = 9}},
                    {TankType.TD, new LevelRange{Min = 7, Max = 9}},
                }
            },
            {
                8, new Dictionary<TankType, LevelRange>
                {
                    {TankType.LT, new LevelRange{Min = 8, Max = 10}},
                    {TankType.MT, new LevelRange{Min = 8, Max = 10}},
                    {TankType.HT, new LevelRange{Min = 8, Max = 10}},
                    {TankType.SPG, new LevelRange{Min = 8, Max = 10}},
                    {TankType.TD, new LevelRange{Min = 8, Max = 10}},
                }
            },
            {
                9, new Dictionary<TankType, LevelRange>
                {
                    {TankType.MT, new LevelRange{Min = 9, Max = 11}},
                    {TankType.HT, new LevelRange{Min = 9, Max = 11}},
                    {TankType.SPG, new LevelRange{Min = 9, Max = 11}},
                    {TankType.TD, new LevelRange{Min = 9, Max = 11}},
                }
            },
            {
                10, new Dictionary<TankType, LevelRange>
                {
                    {TankType.MT, new LevelRange{Min = 10, Max = 11}},
                    {TankType.HT, new LevelRange{Min = 10, Max = 11}},
                    {TankType.SPG, new LevelRange{Min = 10, Max = 11}},
                    {TankType.TD, new LevelRange{Min = 10, Max = 11}},
                }
            },
        };

        #endregion

        private AppSettings AppSettings { get; }

        public List<Version> Versions
        {
            get { return _versions; }
        }

	    /// <summary>
	    /// Tanks dictionary
	    /// KEY - tankid, countryid
	    /// </summary>
	    public Dictionary<int, TankDescription> AllVehicles { get; } = MakeFullTankDescriptionList();

	    private Dictionary<WN8Type, RatingExpectedValues> expectedValues { get; set; }
	    private Dictionary<Version, Dictionary<int, TankDescription>> tanksList { get; } = new Dictionary<Version, Dictionary<int, TankDescription>>();
	    private Dictionary<Version, Dictionary<string, MapDescription>> mapsList { get; } = new Dictionary<Version, Dictionary<string, MapDescription>>();

	    public Dictionary<string, MapDescription> AllMaps { get; } = MakeFullMapDescriptionList();

        /// <summary>
        /// The game servers
        /// </summary>
        public readonly Dictionary<string, string> GameServers = new Dictionary<string, string>
        {
            {"ru", "worldoftanks.net"},
            {"eu", "worldoftanks.eu"},
            {"com", "worldoftanks.com"},
            {"kr", "worldoftanks.kr"},
            {"asia", "worldoftanks.asia"},
        };

        

        /// <summary>
        /// Gets or sets the medals dictionary.
        /// </summary>
        /// <value>
        /// The medals.
        /// </value>
        public Dictionary<int, Medal> Medals { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        private Dictionaries()
        {
            AppSettings = SettingsReader.Get();
            Init();
        }

        public void Init()
        {
	        expectedValues = new Dictionary<WN8Type, RatingExpectedValues>
	        {
				{ WN8Type.Default, ReadRatingExpectedValues(WN8Type.Default)},
		        { WN8Type.KTTC, ReadRatingExpectedValues(WN8Type.KTTC)},
		        { WN8Type.XVM, ReadRatingExpectedValues(WN8Type.XVM)},
		        { WN8Type.Perfomance, ReadRatingExpectedValues(WN8Type.Perfomance)},
			};

            Medals = ReadMedals();
        }

	    public bool TryGetShellDescription(Country country, int id, out ShellDescription shell)
	    {
		    shell = Shells.Value.OrderBy(vl=>vl.NotInShop).FirstOrDefault(vl => vl.Id == id && vl.Country == country);
		    return shell != null;
	    }

	    public bool TryGetArtefactDescription<T>(int id, out T item) where T: ArtefactDescription
		{
		    List<ArtefactDescription> list;

			if (typeof(T) == typeof(DeviceDescription)) list = DeviceDescriptions.Value;
			else if (typeof(T) == typeof(ConsumableDescription)) list = ConsumableDescriptions.Value;
			else throw new ArgumentOutOfRangeException(nameof(item));

			item = (T)list.OrderBy(vl => vl.NotInShop).FirstOrDefault(vl => vl.Id == id);
		    return item != null;
	    }

		private Lazy<List<ShellDescription>> Shells { get; } = new Lazy<List<ShellDescription>>(() =>
	    {
			var result = new List<ShellDescription>();

		    foreach (var element in XDocument.Load(
			    typeof(ImageCache).Assembly.GetManifestResourceStream(
				    $"{typeof(ImageCache).Namespace}.Vehicles.Shells.xml")).Root.Elements().Elements())
		    {

			    var jsonText = JsonConvert.SerializeXNode(element, Formatting.Indented);

			    try
			    {
				    var item = JsonConvert.DeserializeObject<JObject>(jsonText.Replace("\\t", "").Trim()).First.First
					    .ToObject<ShellDescription>();
				    item.Country = element.Parent.Name.LocalName.ToCountry();
				    result.Add(item);
				}
			    catch (Exception e)
			    {
				    Console.WriteLine(e);
			    }
			    
		    }

		    return result;
		});

	    private Lazy<List<ArtefactDescription>> DeviceDescriptions { get; } = new Lazy<List<ArtefactDescription>>(() =>
	    {
		    return XDocument.Load(typeof(ImageCache).Assembly.GetManifestResourceStream($"{typeof(ImageCache).Namespace}.Vehicles.OptionalDevices.xml"))
			.Root.Elements()
			.Select(element => JsonConvert.SerializeXNode(element, Formatting.Indented))
			.Select(jsonText => JsonConvert.DeserializeObject<JObject>(jsonText.Replace("\\t", "")).First.First.ToObject<DeviceDescription>())
			.Cast<ArtefactDescription>().ToList();
	    });

	    private Lazy<List<ArtefactDescription>> ConsumableDescriptions { get; } = new Lazy<List<ArtefactDescription>>(() =>
	    {
			return XDocument.Load(typeof(ImageCache).Assembly.GetManifestResourceStream($"{typeof(ImageCache).Namespace}.Vehicles.Equipments.xml"))
				.Root.Elements()
				.Select(element => JsonConvert.SerializeXNode(element, Formatting.Indented))
				.Select(jsonText => JsonConvert.DeserializeObject<JObject>(jsonText.Replace("\\t", "")).First.First.ToObject<ConsumableDescription>())
				.Cast<ArtefactDescription>().ToList();
		});

	    

		/// <summary>
		/// Gets the instance.
		/// </summary>
		public static Dictionaries Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new Dictionaries();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Gets the tank icon.
        /// </summary>
        /// <param name="playerVehicle">The player vehicle.</param>
        /// <param name="clientVersion"></param>
        /// <returns></returns>
        public TankDescription GetReplayTankDescription(string playerVehicle, Version clientVersion)
        {
	        var list = GetTanksList(clientVersion);
	        var result = list.Values.FirstOrDefault(t =>
		        string.Equals(t.CountryKey, playerVehicle, StringComparison.InvariantCultureIgnoreCase));
	        if (result != null)
	        {
		        return result;
	        }

			result = AllVehicles.Values.FirstOrDefault(t =>
		        string.Equals(t.CountryKey, playerVehicle, StringComparison.InvariantCultureIgnoreCase));

			if (result != null)
	        {
		        return result;
	        }
			//Try to get tank from previous version
	        var prevVersion = Versions.Where(v => v.CompareTo(clientVersion) < 0).OrderByDescending(v=>v).FirstOrDefault();
	        if (prevVersion != null) return GetReplayTankDescription(playerVehicle, prevVersion);

			throw new ArgumentOutOfRangeException(nameof(playerVehicle), $"Cannot find tank for name {playerVehicle}");
        }


	    private Dictionary<int, TankDescription> GetTanksList(Version version)
	    {
		    if (tanksList.TryGetValue(version, out var dict))
			    return dict;

		    dict = ReadTanksDictionary(version);
			tanksList.Add(version, dict);
		    return dict;
	    }

	    public TankDescription GetTankDescription(int? typeCompDescr, Version clientVersion = null)
	    {

		    if (typeCompDescr == null)
		    {
			    throw new ArgumentOutOfRangeException(nameof(typeCompDescr), "Cannot find tank with null compdescr");
		    }

		    TankDescription result;
		    if (clientVersion != null)
		    {
			    var list = GetTanksList(clientVersion);
			    if (list.TryGetValue(DossierUtils.ToUniqueId(typeCompDescr.Value), out result))
				    return result;
		    }

			if (AllVehicles.TryGetValue(DossierUtils.ToUniqueId(typeCompDescr.Value), out result))
			    return result;

		    throw new ArgumentOutOfRangeException(nameof(typeCompDescr), $"Cannot find tank with compdescr={typeCompDescr.Value}");
	    }

	    public static TankDescription FromXElement(XElement element, Version version)
	    {
		    return new TankDescription
		    {
			    TankId = Convert.ToInt32(element.Attribute("id").Value),
			    CountryId = Convert.ToInt32(element.Attribute("countryid").Value),
			    CompDescr = Convert.ToInt32(element.Attribute("compDescr").Value),
			    Type = Convert.ToInt32(element.Attribute("type").Value),
			    Tier = Convert.ToInt32(element.Attribute("tier").Value),
				Secret = Convert.ToBoolean(element.Attribute("secret").Value),
			    Hidden = Convert.ToBoolean(element.Attribute("secret").Value),
				Premium = Convert.ToBoolean(element.Attribute("premium").Value),
			    Key = element.Attribute("key").Value,
			    userString = element.Attribute("userString").Value,
			    descriptionString = element.Attribute("description").Value,
			    Health = Convert.ToInt32(element.Attribute("health").Value),
				Version = version,
		    };
	    }

	    private static Dictionary<int, TankDescription> MakeFullTankDescriptionList()
	    {
		    var result = new Dictionary<int, TankDescription>();
			var docElem = XDocument.Load(
			    typeof(ImageCache).Assembly.GetManifestResourceStream(
				    $"{typeof(ImageCache).Namespace}.Vehicles.Vehicles.xml")).Root;

		    foreach (var element in docElem.Elements("patch").OrderByDescending(e=> e.Attribute("version").Value, StringVersionComparer.Default).Elements()
			    .Concat(docElem.Elements("actionVehicles").Elements()))
		    {
			    var uniqueId = DossierUtils.ToUniqueId(Convert.ToInt32(element.Attribute("compDescr").Value));
			    if (!result.TryGetValue(uniqueId, out var prevValue))
			    {
				    result.Add(uniqueId, FromXElement(element, element.Parent.Attribute("version") == null ? VersionRelease : new Version(element.Parent.Attribute("version").Value)));
					continue;
			    }
				//если когда-то танк был доступен, до сделать его доступным
			    var secret = Convert.ToBoolean(element.Attribute("secret").Value);
			    if (prevValue.Secret && !secret)
				    prevValue.Hidden = false;

		    }
		    return result;
	    }

	    private Dictionary<int, TankDescription> ReadTanksDictionary(Version version)
        {
			//var patchVer = new Version(version.Major, version.Minor, version.Build);
	        var docElem = XDocument.Load(
		        typeof(ImageCache).Assembly.GetManifestResourceStream(
			        $"{typeof(ImageCache).Namespace}.Vehicles.Vehicles.xml")).Root;

	        return docElem.Elements("patch")
				.OrderByDescending(e=> e.Attribute("version").Value, StringVersionComparer.Default)
				.First(e => StringVersionComparer.Default.Compare(e.Attribute("version").Value, version) <= 0)
				.Elements()
		        .Union(docElem.Elements("actionVehicles").Elements())
		        .Select(x=>FromXElement(x, version))
		        .ToDictionary(tank => tank.UniqueId);
        }

	    private static Dictionary<string, MapDescription> MakeFullMapDescriptionList()
	    {
		    var result = new Dictionary<string, MapDescription>();
		    var docElem = XDocument.Load(
			    typeof(ImageCache).Assembly.GetManifestResourceStream(
				    $"{typeof(ImageCache).Namespace}.Maps.Maps.xml")).Root;

		    foreach (var element in docElem.Elements("patch").OrderByDescending(e => e.Attribute("version").Value, StringVersionComparer.Default).Elements())
		    {
			    var key = element.Attribute("key").Value;
			    if (!result.TryGetValue(key, out var prevValue))
			    {
				    result.Add(key, new MapDescription(element, element.Parent.Attribute("version") == null ? VersionRelease : new Version(element.Parent.Attribute("version").Value)));
			    }
		    }
		    return result;
	    }

		private Dictionary<string, MapDescription> GetMapsList(Version version)
	    {
		    if (mapsList.TryGetValue(version, out var dict))
			    return dict;

		    dict = ReadMapsDictionary(version);
		    mapsList.Add(version, dict);
		    return dict;
	    }

	    private Dictionary<string, MapDescription> ReadMapsDictionary(Version version)
	    {
		    var patchVer = new Version(version.Major, version.Minor, version.Build);
		    var docElem = XDocument.Load(
			    typeof(ImageCache).Assembly.GetManifestResourceStream(
				    $"{typeof(ImageCache).Namespace}.Maps.Maps.xml")).Root;

		    return docElem.Elements("patch")
			    .OrderByDescending(e => e.Attribute("version").Value, StringVersionComparer.Default)
			    .First(e => StringVersionComparer.Default.Compare(e.Attribute("version").Value, version) <= 0)
				.Elements()
			    .Select(x => new MapDescription(x, version))
			    .ToDictionary(map => map.Key);
	    }

	    public MapDescription GetMapDescription(string mapKey, Version clientVersion = null)
	    {
		    MapDescription result;
			if (clientVersion != null)
			{
				var list = GetMapsList(clientVersion);
				if (list.TryGetValue(mapKey, out result))
					return result;
			}

		    if (AllMaps.TryGetValue(mapKey, out result))
			    return result;

		    throw new ArgumentOutOfRangeException(nameof(mapKey), $"Cannot find map for key {mapKey}, version {clientVersion}");
		}

		public RatingExpectedValuesData FindExpectedValues(WN8Type wn8Type, TankDescription tank)
		{
			var expValues = expectedValues[wn8Type].Data;

			var exp = expValues.FirstOrDefault(d => d.CompDescr == tank.CompDescr);

			if (exp == null && AppSettings.TryFindTankAnalog)
			{
				exp = expValues.FirstOrDefault(x => x.CountryId == tank.CountryId && x.TankLevel == tank.Tier && x.TankType == tank.Type) ??
				      expValues.FirstOrDefault(x => x.TankLevel == tank.Tier && x.TankType == tank.Type);
			}
			return exp ?? new RatingExpectedValuesData();
		}
		private RatingExpectedValues ReadRatingExpectedValues(WN8Type wn8Type)
        {
            var result = new RatingExpectedValues();
            try
            {
                var filename = string.Empty;
                switch (wn8Type)
                {
                    case WN8Type.Default:
                        filename = "expectedValues_Base.json";
                        break;
                    case WN8Type.KTTC:
                        filename = "expectedValues_KTTC.json";
                        break;
                    case WN8Type.XVM:
                        filename = "expectedValues_XVM.json";
                        break;
	                case WN8Type.Perfomance:
		                filename = "expectedValues_PR.json";
		                break;
					default:
                        throw new ArgumentOutOfRangeException(nameof(wn8Type), wn8Type, null);
                }
	            
                using (StreamReader re = new StreamReader(typeof(ImageCache).Assembly.GetManifestResourceStream($"{typeof(ImageCache).Namespace}.ExpectedValues.{filename}")))
                {
                    JsonTextReader reader = new JsonTextReader(re);
                    JsonSerializer se = new JsonSerializer();
                    result = se.Deserialize<RatingExpectedValues>(reader);
                }

                foreach (var data in result.Data)
                {
	                if(AllVehicles.TryGetValue(DossierUtils.ToUniqueId(data.CompDescr), out var tank))
                    {
                        data.TankLevel = tank.Tier;
                        data.TankType = tank.Type;
	                    data.CountryId = tank.CountryId;
					}

                }
            }
            catch (Exception e)
            {
                _log.Error(e);
            }
            return result;
        }

        public int GetBattleLevel(List<LevelRange> members)
        {
            int max = members.Max(x => x.Max);

            int level = -1;

            for (int i = max; i > 0; i--)
            {
                if (members.All(x => x.Min <= i && x.Max >= i))
                {
                    level = i;
                    break;
                }
            }

            return level;
        }

        /*
         218-max damage
         2-max xp
         */

        /// <summary>
        /// Gets the medals by identifiers.
        /// </summary>
        /// <param name="achievements">The achievements.</param>
        /// <returns></returns>
        public List<Medal> GetMedals(List<int> achievements)
        {
            List<Medal> list = new List<Medal>();

            foreach (int achievement in achievements)
            {
                if (Medals.ContainsKey(achievement))
                {
                    list.Add(Medals[achievement]);
                }
            }
            return list;
        }

        /// <summary>
        /// Reads the medals.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, Medal> ReadMedals()
        {
            var doc = XDocument.Load(File.OpenRead(Path.Combine(Environment.CurrentDirectory, @"Data\Medals.xml")));

            //XmlNodeList nodes = doc.SelectNodes("Medals/node()/medal");

            Dictionary<int, Medal> medals = new Dictionary<int, Medal>();

	        foreach (var node in doc.Root.Descendants("medal"))
	        {
				var medal = new Medal();
				medal.Id = Convert.ToInt32(node.Attribute("id").Value);
				var attribute = node.Attribute("name");
				medal.Name = Resources.Resources.ResourceManager.GetString(attribute.Value) ?? attribute.Value;
				medal.NameResourceId = attribute.Value;
				medal.Icon = node.Attribute("icon").Value;
				medal.Type = int.Parse(node.Attribute("type").Value);
				var xmlAttribute = node.Attribute("showribbon");
				if (xmlAttribute != null)
				{
					medal.ShowRibbon = bool.Parse(xmlAttribute.Value);
				}
				medal.Group = new MedalGroup();

				attribute = node.Parent.Attribute("filter");
				medal.Group.Filter = attribute != null && bool.Parse(attribute.Value);
				attribute = node.Parent.Attribute("name");
				if (attribute != null)
				{
					medal.Group.Name = Resources.Resources.ResourceManager.GetString(attribute.Value) ?? attribute.Value;
				}
				else
				{
					medal.Group.Name = node.Parent.Name.LocalName;
				}

				medals.Add(medal.Id, medal);
			}
			return medals;

        }

        /// <summary>
        /// Gets the achiev medals.
        /// </summary>
        /// <param name="dossierPopUps">The dossier pop ups.</param>
        /// <returns></returns>
        public List<Medal> GetAchievMedals(List<List<JValue>> dossierPopUps)
        {
            List<Medal> list = new List<Medal>();

            foreach (List<JValue> achievement in dossierPopUps)
            {
                int id = achievement[0].Value<int>();
                int value = 0;
                if (achievement[1].Type == JTokenType.Integer)
                {
                    value = achievement[1].Value<int>();
                }
                else
                {
                    value = -1;
                }

                int exId = id * 100 + value;

                if (Medals.ContainsKey(id))
                {
                    list.Add(Medals[id]);
                }
                else if (Medals.ContainsKey(exId))
                {
                    list.Add(Medals[exId]);
                }
            }
            return list;
        }
    }
}
