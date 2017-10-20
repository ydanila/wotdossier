using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using Castle.Components.DictionaryAdapter;
using Ionic.Zip;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHibernate.Linq.Functions;
using NHibernate.Util;
using NUnit.Framework;
using WotDossier.Applications;
using WotDossier.Applications.Logic;
using WotDossier.Applications.Logic.Export;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Applications.ViewModel.Statistic;
using WotDossier.Common;
using WotDossier.Common.Extensions;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Tank;
using Formatting = Newtonsoft.Json.Formatting;

namespace WotDossier.Test
{
    [TestFixture]
    public class CommonTestFixture : TestFixtureBase
    {
		private string clientPath = @"S:\WorldOfTanks";
	    private string patchVer = "0.9.20.1";


	    private string processedPatch = "";

	    private string ResourcePath = @"f:\Games\WotDossierPrepare";


		public class ClientInfo
	    {
		    public string ClientPath { get; set; }
		    public string PatchVer { get; set; }
		    public bool PackedScripts { get; set; }
			public bool PackedImages { get; set; }
			public string EnglishClientPath { get; set; }
		}

	    private List<ClientInfo> clients = new List<ClientInfo>
	    {
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.7.0", PatchVer="0.7.0", PackedScripts=false, PackedImages=false, EnglishClientPath=@"c:\66\World_of_Tanks - 0.7.0"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.7.1.1", PatchVer="0.7.1", PackedScripts=false, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.7.1.1"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.7.2", PatchVer="0.7.2", PackedScripts=false, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.7.2"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.7.3", PatchVer="0.7.3", PackedScripts=false, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.7.3"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.7.4", PatchVer="0.7.4", PackedScripts=false, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.7.4"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.7.5", PatchVer="0.7.5", PackedScripts=false, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.7.5"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.8.0", PatchVer="0.8.0", PackedScripts=false, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.8.0"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.8.1", PatchVer="0.8.1", PackedScripts=false, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.8.1"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.8.2", PatchVer="0.8.2", PackedScripts=false, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.8.2"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.8.3", PatchVer="0.8.3", PackedScripts=false, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.8.3"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.8.4", PatchVer="0.8.4", PackedScripts=false, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.8.4"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.8.5", PatchVer="0.8.5", PackedScripts=false, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.8.5"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.8.6", PatchVer="0.8.6", PackedScripts=false, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.8.6"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.8.7", PatchVer="0.8.7", PackedScripts=false, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.8.7"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.8.8", PatchVer="0.8.8", PackedScripts=false, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.8.8"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.8.9", PatchVer="0.8.9", PackedScripts=false, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.8.9"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.8.10", PatchVer="0.8.10", PackedScripts=false, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.8.10"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.8.11", PatchVer="0.8.11", PackedScripts=false, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.8.11"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.9.0", PatchVer="0.9.0", PackedScripts=false, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.9.0"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.9.1", PatchVer="0.9.1", PackedScripts=false, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.9.1"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.9.2", PatchVer="0.9.2", PackedScripts=false, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.9.2"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.9.3", PatchVer="0.9.3", PackedScripts=false, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.9.3"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.9.4", PatchVer="0.9.4", PackedScripts=false, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.9.4"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.9.5", PatchVer="0.9.5", PackedScripts=false, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.9.5"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.9.6", PatchVer="0.9.6", PackedScripts=false, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.9.6"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.9.7", PatchVer="0.9.7", PackedScripts=false, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.9.7"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.9.8.1", PatchVer="0.9.8", PackedScripts=false, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.9.8.1"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.9.9", PatchVer="0.9.9", PackedScripts=false, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.9.9"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.9.10", PatchVer="0.9.10", PackedScripts=false, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.9.10"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.9.12", PatchVer="0.9.12", PackedScripts=false, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.9.12"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.9.13", PatchVer="0.9.13", PackedScripts=false, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.9.13"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.9.14", PatchVer="0.9.14", PackedScripts=false, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.9.14"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.9.15.1.1", PatchVer="0.9.15", PackedScripts=false, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.9.15.1.1"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.9.16", PatchVer="0.9.16", PackedScripts=false, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.9.16"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.9.17.1", PatchVer="0.9.17", PackedScripts=true, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.9.17.1"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.9.18", PatchVer="0.9.18", PackedScripts=true, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.9.18"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.9.19.1.2", PatchVer="0.9.19", PackedScripts=true, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.9.19.1.2"},
			new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.9.20", PatchVer="0.9.20", PackedScripts=true, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.9.20"},
			new ClientInfo{ClientPath= @"s:\WorldOfTanks", PatchVer="0.9.20.1", PackedScripts=true, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.9.20.1"},
		};


	    [Test]
        public void MultipleUploadTest()
        {
            FileInfo info = new FileInfo(@"Replays\20140325_2258_ussr-Object_140_84_winter.wotreplay");
            ReplayUploader uploader = new ReplayUploader();
            WotReplaysSiteResponse response = uploader.Upload(info, 10800699, "_rembel_");
            Console.WriteLine(response.Error);
        }

        [Test]
        public void NominalDamageTest()
        {
            //JObject expectedValues;
            //using (StreamReader re = new StreamReader(@"Data\expected_tank_values.json"))
            //{
            //    JsonTextReader reader = new JsonTextReader(re);
            //    JsonSerializer se = new JsonSerializer();
            //    expectedValues = se.Deserialize<JObject>(reader);
            //}

            //List<RatingExpectancy> xmlTanks = new List<RatingExpectancy>();
            //foreach (var expectedValue in expectedValues["data"])
            //{
            //    int idNum = (int) expectedValue["IDNum"];

            //    if (Dictionaries.Instance.Tanks.Values.FirstOrDefault(x => x.CompDescr == idNum) == null)
            //    {
            //        expectedValue["tankId"] = idNum >> 8 & 65535;
            //        expectedValue["countryId"] = idNum >> 4 & 15;
            //        Console.WriteLine(expectedValue);
            //        continue;
            //    }

            //    var tankDescription = Dictionaries.Instance.Tanks.Values.First(x => x.CompDescr == idNum);

            //    RatingExpectancy ratingExpectancy = new RatingExpectancy();
            //    double prNominalDamage = (double) expectedValue["expDamage"];
            //    ratingExpectancy.PRNominalDamage = tankDescription.Expectancy.PRNominalDamage;
            //    ratingExpectancy.TankLevel = tankDescription.Tier;
            //    ratingExpectancy.TankType = (TankType) tankDescription.Type;
            //    ratingExpectancy.Wn8NominalDamage = prNominalDamage;
            //    ratingExpectancy.Wn8NominalWinRate = (double)expectedValue["expWinRate"];
            //    ratingExpectancy.Wn8NominalSpotted = (double)expectedValue["expSpot"];
            //    ratingExpectancy.Wn8NominalFrags = (double)expectedValue["expFrag"];
            //    ratingExpectancy.Wn8NominalDefence = (double)expectedValue["expDef"];
            //    ratingExpectancy.Icon = tankDescription.Icon.IconOrig;
            //    ratingExpectancy.TankTitle = tankDescription.Title;
            //    ratingExpectancy.CompDescr = tankDescription.CompDescr;

            //    xmlTanks.Add(ratingExpectancy);
            //}

            //foreach (TankDescription description in Dictionaries.Instance.Tanks.Values)
            //{
            //    if (xmlTanks.FirstOrDefault(x => string.Equals(x.Icon, description.Icon.IconOrig, StringComparison.InvariantCultureIgnoreCase)) == null)
            //    {
            //        Console.WriteLine(description.Icon.IconOrig);
            //    }
            //}

            //Console.WriteLine(JsonConvert.SerializeObject(xmlTanks.OrderBy(x => x.TankTitle), Formatting.Indented));
	        throw new NotImplementedException();
        }

        //[Test]
        //public void CsvExportProviderTest()
        //{
        //    CsvExportProvider provider = new CsvExportProvider();
        //    FileInfo cacheFile = CacheTestFixture.GetCacheFile("_rembel__ru", @"\CacheFiles\0.8.9\");
        //    List<TankJson> tanks = CacheFileHelper.ReadTanksCache(CacheFileHelper.BinaryCacheToJson(cacheFile));
        //    List<RandomBattlesTankStatisticRowViewModel> list = tanks.Select(x => new RandomBattlesTankStatisticRowViewModel(x, new List<StatisticSlice>())).ToList();
        //    Console.WriteLine(provider.Export(list, new List<Type>{typeof(IStatisticBattles), typeof(IStatisticFrags)}));
        //}

        [Test]
        public void appSpotTest()
        {
            AppSpotUploader uploader = new AppSpotUploader();
            FileInfo cacheFile = CacheTestFixture.GetCacheFile("_rembel__ru", @"\CacheFiles\0.8.9\");
            long id = uploader.Upload(cacheFile);
            uploader.Update(cacheFile, id);
        }

        [Test]
        public void MedalsResourcesTest()
        {
            var dictionary = GetResourcesDictionary();
            
            foreach (var medal in Dictionaries.Instance.Medals)
            {
                var localizedString = Resources.Resources.ResourceManager.GetString(medal.Value.NameResourceId);
                Assert.IsNotNull(localizedString, "Resource not found: {0}", medal.Value.NameResourceId);

                string key = string.Format("images/medals/{0}.png", medal.Value.Icon).ToLowerInvariant();
                Assert.IsTrue(dictionary.ContainsKey(key), "Image resource not found: {0}", medal.Value.Icon);
            }
        }

        private static Dictionary<string, object> GetResourcesDictionary()
        {
            var assembly = Assembly.Load("WotDossier.Resources");
            Stream fs = assembly.GetManifestResourceStream("WotDossier.Resources.g.resources");
            var rr = new System.Resources.ResourceReader(fs);

            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            foreach (DictionaryEntry entry in rr)
            {
                dictionary.Add(entry.Key.ToString().ToLowerInvariant(), entry.Value);
            }
            return dictionary;
        }

        [Test]
        public void EncodeCacheFileName()
        {
            string server = "login.p7.worldoftanks.net";
            string playerName = "LayneksII";
            var encodFileName = CacheFileHelper.EncodFileName(server, playerName);
            Console.WriteLine(encodFileName);
            Console.WriteLine(CacheFileHelper.DecodFileName(encodFileName));
            Console.WriteLine(CacheFileHelper.DecodFileName("NRXWO2LOFZYDOLTXN5ZGYZDPMZ2GC3TLOMXG4ZLUHIZDAMB.dat"));
            
        }

        [Test]
        public void ImportTanksComponentsXmlTest()
        {
            var strings = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, $@"Output\Patch\{patchVer}\Tanks"), "shells.xml",
                SearchOption.AllDirectories);

            List<JObject> result = new List<JObject>();

            foreach (var xml in strings)
            {
                BigWorldXmlReader reader = new BigWorldXmlReader();
                FileInfo info = new FileInfo(xml);

                using (BinaryReader br = new BinaryReader(info.OpenRead()))
                {
                    var xmlContent = reader.DecodePackedFile(br, "shell");
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlContent);
                    string jsonText = JsonConvert.SerializeXmlNode(doc, Formatting.Indented);

                    var dictionary = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(jsonText);
                    dictionary = dictionary["shell"].ToObject<Dictionary<string, JObject>>();
                    dictionary.Remove("icons");

                    jsonText = JsonConvert.SerializeObject(dictionary, Formatting.Indented);

                    var path = Path.Combine(Environment.CurrentDirectory, @"Output\Externals\shells");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    path = Path.Combine(path, info.Directory.Parent.Name + "_" + info.Name.Replace(info.Extension, ".json"));

                    var stream = File.OpenWrite(path.ToLower());
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(jsonText);
                    }

                }
            }
        }

 	    public void ImportTanksXml(ClientInfo client)
		{
			var scriptsPath = Path.Combine(ResourcePath, $@"{client.PatchVer}\scripts");
			var destination = Path.Combine(Environment.CurrentDirectory, $@"Output\Vehicles");
			var source = Path.Combine(scriptsPath, @"item_defs\vehicles");

			if(!Directory.Exists(destination))
				Directory.CreateDirectory(destination);
			if(!Directory.Exists(Path.Combine(destination, $@"Images\Vehicle")))
				Directory.CreateDirectory(Path.Combine(destination, $@"Images\Vehicle"));

			var resultDoc = new XDocument();
			if (File.Exists(Path.Combine(destination, "Vehicles.xml")))
				resultDoc = XDocument.Load(Path.Combine(destination, "Vehicles.xml"));
			else
			{
				var mapNode = new XElement("vehicles");
				resultDoc.Add(mapNode);
			}

			var patchNode = resultDoc.Root.Elements("patch").FirstOrDefault(e => e.Attribute("version").Value == client.PatchVer);
			if (patchNode == null)
			{
				patchNode = new XElement("patch", new XAttribute("version", client.PatchVer));
				resultDoc.Root.Add(patchNode);
			}

			var strings = Directory.GetFiles(source, "list.xml", SearchOption.AllDirectories);

			BigWorldXmlReader reader = new BigWorldXmlReader();

			foreach (var xml in strings)
			{
				FileInfo info = new FileInfo(xml);
				using (BinaryReader br = new BinaryReader(info.OpenRead()))
				{
					var xmlContent = reader.DecodePackedFile(br, "vehicles");
					var doc = XDocument.Parse(xmlContent);

					foreach (var element in doc.Root.Elements())
					{
						var id = Convert.ToInt32(element.Element("id").Value.Trim(' ', '\t'));
						var userString = element.Element("userString").Value.Trim(' ', '\t');
						var description = element.Element("description").Value.Trim(' ', '\t');
						var notInShop = (element.Element("notInShop") != null && element.Element("notInShop").Value.Trim(' ', '\t') == "true");
						var goldPrice = element.Element("price").Elements("gold").Any();
						var level = Convert.ToInt32(element.Element("level").Value.Trim(' ', '\t'));
						var countryName = info.Directory.Name.Substring(0, 1).ToUpper() + info.Directory.Name.Substring(1);
						
						var country = (Country)Enum.Parse(typeof(Country), countryName);
						var countryid = (int)country;
						var typeCompDesc = DossierUtils.TypeCompDesc(countryid, id);
						var uniqueId = DossierUtils.ToUniqueId(countryid, id);
						var tags = element.Element("tags").Value.Trim(' ', '\t');
						var tankType = GetVehicleTypeByTag(tags);
						var secret = tags.Contains("secret");
						//var key = userString.Split(':')[1];
						var key = element.Name.LocalName;

						var tankDef = LoadTankDefinition(countryid, key);
						int health = 0;
						if (tankDef != null)
						{
							health = Convert.ToInt32(tankDef.Element("hull").Element("maxHealth").Value.Trim(' ', '\t')) +
							         Convert.ToInt32(tankDef.Element("turrets0").Elements().First().Element("maxHealth").Value
								         .Trim(' ', '\t'));
							//Console.WriteLine(tankDef.ToString(Formatting.Indented));
							//var health = tankDef.SelectToken("$.vehicles.hull.maxHealth").Value<int>() + tankDef.SelectTokens("$.vehicles.turrets0..maxHealth").First().Value<int>();
							//tankDescription["health"] = health;
						}
						patchNode.Element(element.Name.LocalName)?.Remove();
						patchNode.Add(new XElement(element.Name.LocalName, 
							new XAttribute("id", id),
							new XAttribute("countryid", countryid),
							new XAttribute("compDescr", typeCompDesc),
							new XAttribute("type", (int)tankType),
							new XAttribute("secret", secret),
							new XAttribute("premium", goldPrice),
							new XAttribute("tier", level),
							new XAttribute("key", key),
							new XAttribute("userString", userString),
							new XAttribute("description", description),
							new XAttribute("health", health)
							));

						//var icon = countryName.ToLower() + "-" + key + ".png";
						//var tgticon = countryName.ToLower() + "-" + key + "." + typeCompDesc + ".png";
						//if (!File.Exists(Path.Combine(ResourcePath, $@"{client.PatchVer}\gui\maps\icons\vehicle",
						//	icon)))
						//{
						//	if (File.Exists(Path.Combine(Environment.CurrentDirectory, $@"Patch\Images\vehicle", icon)))
						//		File.Copy(Path.Combine(Environment.CurrentDirectory, $@"Patch\Images\vehicle", icon), Path.Combine(destination, $@"Images\Vehicle", tgticon), true);
						//	else
						//		Console.WriteLine(Path.Combine(ResourcePath, $@"\{client.PatchVer}\gui\maps\icons\vehicle", icon));
						//}
						//else
						//	File.Copy(Path.Combine(ResourcePath, $@"{client.PatchVer}\gui\maps\icons\vehicle", icon), Path.Combine(destination, $@"Images\Vehicle", tgticon), true);

					}

				}
			}
			resultDoc.Save(Path.Combine(destination, "Vehicles.xml"));
		}

		private void ImportActionTanksXml()
	    {
		    var destination = Path.Combine(Environment.CurrentDirectory, $@"Output\Vehicles");
		    if (!Directory.Exists(destination))
			    Directory.CreateDirectory(destination);
		    if (!Directory.Exists(Path.Combine(destination, $@"Images\Vehicle")))
			    Directory.CreateDirectory(Path.Combine(destination, $@"Images\Vehicle"));

		    var resultDoc = new XDocument();
		    if (File.Exists(Path.Combine(destination, "Vehicles.xml")))
			    resultDoc = XDocument.Load(Path.Combine(destination, "Vehicles.xml"));
		    else
		    {
			    var mapNode = new XElement("vehicles");
			    resultDoc.Add(mapNode);
		    }

		    var patchNode = resultDoc.Root.Elements("actionVehicles").FirstOrDefault();
		    if (patchNode == null)
		    {
			    patchNode = new XElement("actionVehicles");
			    resultDoc.Root.Add(patchNode);
		    }
			//Add Action tanks

		    #region Unknown
		    patchNode.Element("tank")?.Remove();
		    patchNode.Add(new XElement("tank",
			    new XAttribute("id", -1),
			    new XAttribute("countryid", -1),
			    new XAttribute("compDescr", 65521),
			    new XAttribute("type", 5),
			    new XAttribute("secret", true),
			    new XAttribute("premium", false),
			    new XAttribute("tier", 1),
			    new XAttribute("key", "tank"),
			    new XAttribute("userString", "unknown"),
			    new XAttribute("description", "unknown"),
			    new XAttribute("health", 1)
		    ));

		    resultDoc.Save(Path.Combine(destination, "Vehicles.xml"));

		    var icon = "unknown-tank.png";
		    var tgticon = "unknown-tank.png";
		    if (File.Exists(Path.Combine(Environment.CurrentDirectory, $@"Patch\Images\vehicle", icon)))
			    File.Copy(Path.Combine(Environment.CurrentDirectory, $@"Patch\Images\vehicle", icon), Path.Combine(destination, $@"Images\Vehicle", tgticon), true);
		    else
			    Console.WriteLine(icon);
		    #endregion

			#region Lanchester
			patchNode.Element("GB90_Lanchester_Armored_Car")?.Remove();
		    patchNode.Add(new XElement("GB90_Lanchester_Armored_Car",
			    new XAttribute("id", 222),
			    new XAttribute("countryid", 5),
			    new XAttribute("compDescr", 56913),
			    new XAttribute("type", 5),
			    new XAttribute("secret", true),
			    new XAttribute("premium", true),
			    new XAttribute("tier", 1),
			    new XAttribute("key", "GB90_Lanchester_Armored_Car"),
			    new XAttribute("userString", "#gb_vehicles:GB90_Lanchester_Armored_Car"),
			    new XAttribute("description", "#gb_vehicles:GB90_Lanchester_Armored_Car_descr"),
			    new XAttribute("health", 1)
		    ));

		    resultDoc.Save(Path.Combine(destination, "Vehicles.xml"));

			icon = "uk-GB90_Lanchester_Armored_Car.png";
		    tgticon = "uk-GB90_Lanchester_Armored_Car.png";
		    if (File.Exists(Path.Combine(Environment.CurrentDirectory, $@"Patch\Images\vehicle", icon)))
				File.Copy(Path.Combine(Environment.CurrentDirectory, $@"Patch\Images\vehicle", icon), Path.Combine(destination, $@"Images\Vehicle", tgticon), true);
			else
				Console.WriteLine(icon);
			#endregion
		}

	    private void CopyVehicleImages()
	    {
			var destination = Path.Combine(Environment.CurrentDirectory, $@"Output\Vehicles");

		    if (!File.Exists(Path.Combine(destination, "Vehicles.xml")))
			    return;
		    var docElem = XDocument.Load(Path.Combine(destination, "Vehicles.xml")).Root;

		    var result = new Dictionary<int, TankDescription>();

			foreach (var element in docElem.Elements("patch").OrderByDescending(e => e.Attribute("version").Value, StringVersionComparer.Default).Elements()
			    .Concat(docElem.Elements("actionVehicles").Elements()))
		    {
			    var uniqueId = DossierUtils.ToUniqueId(Convert.ToInt32(element.Attribute("compDescr").Value));
			    if (!result.TryGetValue(uniqueId, out var prevValue))
			    {
					result.Add(uniqueId, Dictionaries.FromXElement(element, element.Parent.Attribute("version") == null ? new Version(clients.Last().PatchVer) : new Version(element.Parent.Attribute("version").Value)));
					continue;
			    }
			    //если когда-то танк был доступен, до сделать его доступным
			    var secret = Convert.ToBoolean(element.Attribute("secret").Value);
			    if (prevValue.Secret && !secret)
				    prevValue.Hidden = false;

		    }

		    foreach (var description in result.Values)
		    {
				var icon = description.CountryKey;
			    //var tgticon = countryName.ToLower() + "-" + key + "." + typeCompDesc + ".png";
			    if (!File.Exists(Path.Combine(ResourcePath, $@"{description.Version}\gui\maps\icons\vehicle", icon + ".png")))
			    {
				    if (File.Exists(Path.Combine(Environment.CurrentDirectory, $@"Patch\Images\vehicle", icon + ".png")))
					    File.Copy(Path.Combine(Environment.CurrentDirectory, $@"Patch\Images\vehicle", icon + ".png"),
						    Path.Combine(destination, $@"Images\Vehicle", icon + ".png"), true);
				    else
				    {
						//Description exists, image not, try to find in prev releases
					    var found = false;
					    foreach (var element in docElem.Elements("patch").OrderByDescending(e => e.Attribute("version").Value, StringVersionComparer.Default).Where(e=>e.Attribute("version").Value != description.Version.ToString())
							.Elements().Where(x=>x.Attribute("compDescr").Value == description.CompDescr.ToString()))
					    {
						    var vs = element.Parent.Attribute("version").Value;
						    if (File.Exists(Path.Combine(ResourcePath, $@"{vs}\gui\maps\icons\vehicle", icon + ".png")))
						    {
							    File.Copy(Path.Combine(ResourcePath, $@"{vs}\gui\maps\icons\vehicle", icon + ".png"), Path.Combine(destination, $@"Images\Vehicle", icon + ".png"), true);
							    found = true;
							    break;
						    }
						}
						if(!found)
							Console.WriteLine(Path.Combine(ResourcePath, $@"\{description.Version}\gui\maps\icons\vehicle", icon + ".png"));
				    }
			    }
			    else
			    	File.Copy(Path.Combine(ResourcePath, $@"{description.Version}\gui\maps\icons\vehicle", icon + ".png"), Path.Combine(destination, $@"Images\Vehicle", icon + ".png"), true);
			}

		}

		private static void CheckTankImages()
	    {
		    var destination = Path.Combine(Environment.CurrentDirectory, $@"Output\Vehicles");

			if (!File.Exists(Path.Combine(destination, "Vehicles.xml")))
			    return;
			var resultDoc = XDocument.Load(Path.Combine(destination, "Vehicles.xml"));

		    foreach (var element in resultDoc.Root.Elements("patch").Elements().Union(resultDoc.Elements("actionVehicles").Elements()))
		    {
			    var key = element.Name.LocalName;
			    var country = (Country) Convert.ToInt32(element.Attribute("countryid").Value);
			    var compDescr = Convert.ToInt32(element.Attribute("compDescr").Value);

			    var tgticon = country.ToString().ToLower() + "-" + key + "." + compDescr + ".png";
			    if (!File.Exists(Path.Combine(destination, $@"Images\Vehicle", tgticon)))
					Console.WriteLine(tgticon);
			}
		}


		private string GetString(string key)
        {
            var value = ResourceManagers.Select(x => x.GetString(key, CultureInfo.InvariantCulture)).FirstOrDefault(x => x != null);
            value = (value ?? key).Trim();
            if (value.Contains(@"PC방"))
            {
                value = value.Replace(@"PC방 ", "") + " IGR";
            }
            return value.Replace("ä", "a").Replace("ö", "o").Replace("ß", "ss").Replace("â", "a").Replace("ä", "a");
        }

        private TankType GetVehicleTypeByTag(string tags)
        {
            if (tags.StartsWith("lightTank", StringComparison.InvariantCultureIgnoreCase))
            {
                return TankType.LT;
            }
            if (tags.StartsWith("mediumTank", StringComparison.InvariantCultureIgnoreCase))
            {
                return TankType.MT;
            }
            if (tags.StartsWith("heavyTank", StringComparison.InvariantCultureIgnoreCase))
            {
                return TankType.HT;
            }
            if (tags.StartsWith("SPG", StringComparison.InvariantCultureIgnoreCase))
            {
                return TankType.SPG;
            }
            if (tags.StartsWith("AT-SPG", StringComparison.InvariantCultureIgnoreCase))
            {
                return TankType.TD;
            }
            return TankType.Unknown;
        }

	    private XElement LoadTankDefinition(int countryid, string tankName)
	    {
		    var fileName = Path.Combine(ResourcePath, $@"{patchVer}\scripts\item_defs\vehicles", ((Country)countryid).ToString(), tankName + ".xml");
		    if (File.Exists(fileName))
		    {
			    var file = new FileInfo(fileName);
			    BigWorldXmlReader reader = new BigWorldXmlReader();
			    using (BinaryReader br = new BinaryReader(file.OpenRead()))
			    {
				    var xmlContent = reader.DecodePackedFile(br, "vehicles");
					return XElement.Parse(xmlContent);
			    }
		    }
		    return null;
	    }

        private int GetOrder(int value)
        {
            switch ((Country)value)
            {
                case Country.China:
                    return 0;
                case Country.Germany:
                    return 1;
                case Country.France:
                    return 2;
                case Country.Ussr:
                    return 3;
                case Country.Usa:
                    return 4;
                case Country.Uk:
                    return 5;
                case Country.Japan:
                    return 6;
                case Country.Czech:
                    return 7;
                case Country.Sweden:
                    return 8;
	            case Country.Poland:
		            return 9;
			}
            return -1;
        }

	    public void ImportMapsXml(ClientInfo client)
	    {
		    var configsPath = Path.Combine(ResourcePath, $@"{client.PatchVer}\scripts\arena_defs");

		    if (!Directory.Exists(configsPath))
		    {
			    Assert.Fail("Folder not exists - [{0}]", configsPath);
		    }

		    var path = Path.Combine(Environment.CurrentDirectory, $@"Output\Maps");

		    if (!Directory.Exists(path))
		    {
			    Directory.CreateDirectory(path);
		    }



		    var reader = new BigWorldXmlReader();

		    var resultDoc = new XDocument();
		    if (File.Exists(Path.Combine(path, "Maps.xml")))
			    resultDoc = XDocument.Load(Path.Combine(path, "Maps.xml"));
		    else
		    {
			    var mapNode = new XElement("maps");
			    resultDoc.Add(mapNode);
		    }

		    var patchNode = resultDoc.Root.Elements("patch").FirstOrDefault(e => e.Attribute("version").Value == client.PatchVer);
		    if (patchNode == null)
		    {
			    patchNode = new XElement("patch", new XAttribute("version", client.PatchVer));
			    resultDoc.Root.Add(patchNode);

			}

			foreach (var configFile in Directory.GetFiles(configsPath, "*.xml", SearchOption.AllDirectories))
		    {
			    var file = new FileInfo(configFile);


			    using (var stream = new FileStream(configFile, FileMode.Open, FileAccess.Read))
			    {
				    using (var br = new BinaryReader(stream))
				    {
					    var xml = reader.DecodePackedFile(br, "map");
					    var doc = XElement.Parse(xml);
					    if (doc.Name != "map" || doc.Elements("name").FirstOrDefault() == null)
						    continue;

					    var mapKey = file.Name.Replace(file.Extension, string.Empty);

						if (mapKey.Contains("hangar")) continue;

					    doc.Add(new XAttribute("key", mapKey));
					    patchNode.Elements().Where(x => x.Attribute("key").Value == mapKey).ForEach(x => x.Remove());
						patchNode.Add(doc);
				    }
			    }
		    }
		    resultDoc.Save(Path.Combine(path, "Maps.xml"));
			if(Directory.Exists(Path.Combine(ResourcePath, $@"{client.PatchVer}\gui\maps\icons\map\stats")))
				DirectoryCopy(Path.Combine(ResourcePath, $@"{client.PatchVer}\gui\maps\icons\map\stats"), Path.Combine(path, @"Images\Stats"), false, "*.png");
		    if (Directory.Exists(Path.Combine(ResourcePath, $@"{client.PatchVer}\gui\maps\icons\map\small")))
			    DirectoryCopy(Path.Combine(ResourcePath, $@"{client.PatchVer}\gui\maps\icons\map\small"), Path.Combine(path, @"Images\Stats"), false, "*.png");

		}


	    public void CopyMinimapImages()
	    {
		    var destination = Path.Combine(Environment.CurrentDirectory, $@"Output\Maps");
		    var path = Path.Combine(destination, $@"Images\Minimap\PNG");

		    if (!Directory.Exists(path))
			    Directory.CreateDirectory(path);

		    var result = new List<(string patch, string name, string image)>();


		    if (!File.Exists(Path.Combine(destination, "Maps.xml")))
			    return;
		    var docElem = XDocument.Load(Path.Combine(destination, "Maps.xml"));

		    foreach (var patch in docElem.Root.Elements("patch"))
		    {
			    var version = patch.Attribute("version").Value;
			    foreach (var map in patch.Elements("map"))
			    {
				    var name = map.Attribute("key").Value;

				    var mm = (map.Element("minimap") ?? map.Element("defaultMinimap")).Value;

				    //if (dds)
				    //{
				    //	var parts = mm.Split('/');
				    //	if (client.PackedImages)
				    //	{
				    //		string filepath = Path.Combine(client.ClientPath, $@"res\packages\{name}.pkg");
				    //		using (var zip = new ZipFile(filepath, Encoding.GetEncoding((int) CodePage.CyrillicDOS)))
				    //		{
				    //			zip.ExtractSelectedEntries($"name = {parts[2]}", $@"{parts[0]}/{parts[1]}", path,
				    //				ExtractExistingFileAction.OverwriteSilently);
				    //		}
				    //	}
				    //	else
				    //	{
				    //		if (!Directory.Exists(Path.Combine(path, $@"{parts[0]}/{parts[1]}")))
				    //			Directory.CreateDirectory(Path.Combine(path, $@"{parts[0]}/{parts[1]}"));

				    //		File.Copy(Path.Combine(client.ClientPath, $@"res", mm.Replace("/", @"\")),
				    //			Path.Combine(path, mm.Replace("/", @"\")), true);
				    //	}
				    //}

				    var ext = ".png";
				    var fn = $@"{name}{ext}";
				    var tgtFile = Path.Combine(path, fn);
				    var srcFile = Path.Combine(Path.Combine(ResourcePath, $@"{version}\gui\maps\icons\map"), fn);

				    //if (dds)
				    //{
				    //	srcFile = Path.Combine(path, mm.Replace("/", @"\"));
				    //	ext = ".dds";
				    //	fn = $@"{name}{ext}";
				    //	tgtFile = Path.Combine(path, fn);
				    //}



				    if (!File.Exists(tgtFile))
				    {
					    File.Copy(srcFile, tgtFile);
					    result.Add((patch:version, name:name, image:name));
					    continue;
				    }
				    if (File.ReadAllBytes(srcFile).Compare(File.ReadAllBytes(tgtFile)))
				    {
					    result.Add((patch: version, name: name, image: name));
					    continue;
				    }

				    var prevItem = Enumerable.Reverse(result).FirstOrDefault(l => l.name == name && l.image == l.name);
				    var nfn = $@"{name}-{prevItem.patch}";
				    for (int i = 0; i < result.Count; i++)
				    {
					    if (result[i].name == name && result[i].image == name)
						    result[i] = (result[i].patch, name, nfn);
				    }
				    result.Add((patch: version, name: name, image: name));
				    File.Move(tgtFile, Path.Combine(path, nfn + ext));
				    File.Copy(srcFile, tgtFile);
			    }
		    }

		    foreach (var item in result)
		    {
			    docElem.Root.Elements("patch").First(x => x.Attribute("version").Value == item.patch)
				    .Elements("map").First(x => x.Attribute("key").Value == item.name).Add(new XAttribute("image", item.image));
		    }
		    docElem.Save(Path.Combine(destination, "Maps.xml"));

		    path = Path.Combine(destination, $@"Images\Minimap\DDS");

		    if (!Directory.Exists(path))
			    Directory.CreateDirectory(path);


			foreach (var patch in docElem.Root.Elements("patch"))
		    {
			    foreach (var map in patch.Elements("map"))
			    {
				    var name = map.Attribute("key").Value;
					var image = map.Attribute("image").Value;
				    
				    var fn = $@"{image}.dds";
				    var tgtFile = Path.Combine(path, fn);

					if (File.Exists(tgtFile)) continue;

				    var client = (name == image) ? clients.Last() : clients.First(c => c.PatchVer == image.Split('-')[1]);

					var mm = (map.Element("minimap") ?? map.Element("defaultMinimap")).Value;

					var parts = mm.Split('/');
					if (client.PackedImages)
					{
						string filepath = find(client, name);
						using (var zip = new ZipFile(filepath, Encoding.GetEncoding((int)CodePage.CyrillicDOS)))
						{
							zip.ExtractSelectedEntries($"name = {parts[2]}", $@"{parts[0]}/{parts[1]}", path,
								ExtractExistingFileAction.OverwriteSilently);
						}
					}
					else
					{
						if (!Directory.Exists(Path.Combine(path, $@"{parts[0]}/{parts[1]}")))
							Directory.CreateDirectory(Path.Combine(path, $@"{parts[0]}/{parts[1]}"));

						File.Copy(Path.Combine(client.ClientPath, $@"res", mm.Replace("/", @"\")),
							Path.Combine(path, mm.Replace("/", @"\")), true);
					}
				    
				    var srcFile = Path.Combine(path, mm.Replace("/", @"\"));
				    File.Copy(srcFile, tgtFile);
				}

			    string find(ClientInfo cl, string pkg)
			    {
					for (var i = clients.IndexOf(cl); i >= 0; i--)
				    {
						var ps = Path.Combine(clients[i].ClientPath, $@"res\packages\{pkg}.pkg");

					    if (File.Exists(ps)) return ps;
					}
				    return null;

			    }
			}
	    }

	    public void ImportShellsXml(ClientInfo client)
	    {
		    var path = Path.Combine(Environment.CurrentDirectory, $@"Output\Vehicles");

		    if (!Directory.Exists(path))
		    {
			    Directory.CreateDirectory(path);
		    }

		    var resultDoc = new XDocument();
		    if (File.Exists(Path.Combine(path, "Shells.xml")))
			    resultDoc = XDocument.Load(Path.Combine(path, "Shells.xml"));
		    else
		    {
			    var mapNode = new XElement("shells");
			    resultDoc.Add(mapNode);
		    }

			var strings = Directory.GetFiles(Path.Combine(ResourcePath, $@"{client.PatchVer}\scripts"), "shells.xml",
			    SearchOption.AllDirectories);


		    foreach (var xml in strings)
		    {
			    var arr = Path.GetDirectoryName(xml).Split(Path.DirectorySeparatorChar);
			    var country = arr[arr.Length - 2].ToLower();

			    var countryNode = resultDoc.Root.Elements(country).FirstOrDefault();
			    if (countryNode == null)
			    {
				    countryNode = new XElement(country);
				    resultDoc.Root.Add(countryNode);
			    }

				BigWorldXmlReader reader = new BigWorldXmlReader();
			    FileInfo info = new FileInfo(xml);
			    using (BinaryReader br = new BinaryReader(info.OpenRead()))
			    {
				    var xmlContent = reader.DecodePackedFile(br, "shell");
					var doc = XDocument.Parse(xmlContent);
				    doc.Root.Element("icons")?.Remove();

				    foreach (var elem in doc.Root.Elements())
				    {
					    countryNode.Element(elem.Name.LocalName)?.Remove();

						//Косяк ВГ с двумя элементами в "Development_Shot"
						if (elem.Elements("notInShop").Count() > 1)
							elem.Elements("notInShop").Last().Remove();

						countryNode.Add(elem);
					}
			    }
		    }
		    resultDoc.Save(Path.Combine(path, "Shells.xml"));

		    DirectoryCopy(Path.Combine(ResourcePath, $@"{client.PatchVer}\gui\maps\icons\shell"), Path.Combine(path, @"Images\Shell"), false, "*.png");
			
	    }

		public void ImportArtefactXml(ClientInfo client)
		{
			var path = Path.Combine(Environment.CurrentDirectory, $@"Output\Vehicles");

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			var equDoc = new XDocument();
			if (File.Exists(Path.Combine(path, "Equipments.xml")))
				equDoc = XDocument.Load(Path.Combine(path, "Equipments.xml"));
			else
			{
				var nd = new XElement("equipments");
				equDoc.Add(nd);
			}

			BigWorldXmlReader reader = new BigWorldXmlReader();

			var fn = Path.Combine(ResourcePath,
				$@"{client.PatchVer}\scripts\item_defs\vehicles\common\equipments.xml");
			if (File.Exists(fn))
			{
				var info = new FileInfo(fn);
				using (BinaryReader br = new BinaryReader(info.OpenRead()))
				{
					var xmlContent = reader.DecodePackedFile(br, "equipment");
					var doc = XDocument.Parse(xmlContent);

					foreach (var elem in doc.Root.Elements())
					{
						equDoc.Root.Element(elem.Name.LocalName)?.Remove();
						equDoc.Root.Add(elem);
					}
				}
			}

			equDoc.Save(Path.Combine(path, "Equipments.xml"));

			var optDoc = new XDocument();
			if (File.Exists(Path.Combine(path, "OptionalDevices.xml")))
				optDoc = XDocument.Load(Path.Combine(path, "OptionalDevices.xml"));
			else
			{
				var nd = new XElement("optionalDevices");
				optDoc.Add(nd);
			}

			fn = Path.Combine(ResourcePath,
				$@"{client.PatchVer}\scripts\item_defs\vehicles\common\optional_devices.xml");
			if (File.Exists(fn))
			{
				var info = new FileInfo(fn);
				using (BinaryReader br = new BinaryReader(info.OpenRead()))
				{
					var xmlContent = reader.DecodePackedFile(br, "optionalDevices");
					var doc = XDocument.Parse(xmlContent);

					foreach (var elem in doc.Root.Elements())
					{
						optDoc.Root.Element(elem.Name.LocalName)?.Remove();
						optDoc.Root.Add(elem);
					}
				}
			}

			optDoc.Save(Path.Combine(path, "OptionalDevices.xml"));

			DirectoryCopy(Path.Combine(ResourcePath, $@"{client.PatchVer}\gui\maps\icons\artefact"), Path.Combine(path, @"Images\Artefact"), false, "*.png");
		}

		public void ImportAchievementsXml(ClientInfo client)
		{
			var xmlPath = Path.Combine(ResourcePath, $@"{client.PatchVer}\scripts\item_defs\achievements.xml");
			if (!File.Exists(xmlPath)) return;

			var path = Path.Combine(Environment.CurrentDirectory, $@"Output\Achievements");

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			var equDoc = new XDocument();

			var reader = new BigWorldXmlReader();
			var info = new FileInfo(xmlPath);
			using (BinaryReader br = new BinaryReader(info.OpenRead()))
			{
				var xmlContent = reader.DecodePackedFile(br, "achievements");
				var doc = XElement.Parse(xmlContent);
				equDoc.Add(doc);
			}
			equDoc.Save(Path.Combine(path, "Achievements.xml"));

			DirectoryCopy(Path.Combine(ResourcePath, $@"{client.PatchVer}\gui\maps\icons\achievement"), Path.Combine(path, @"Images\Achievement"), false, "*.png");
			
		}

	    [Test]
	    public void ImportAchievementsImages(ClientInfo client)
	    {
		    var destination = Path.Combine(Environment.CurrentDirectory, $@"Output\{client.PatchVer}\Achievements\Images");
		    Directory.CreateDirectory(destination);
			string filepath = Path.Combine(client.ClientPath, @"res\packages\gui.pkg");
		    using (var zip = new ZipFile(filepath, Encoding.GetEncoding((int) CodePage.CyrillicDOS)))
		    {
			    var achievement = @"gui/maps/icons/achievement";
			    zip.ExtractSelectedEntries("name = *.*", achievement, destination, ExtractExistingFileAction.OverwriteSilently);
		    }
	    }

	    public void EnsureImportImages(ClientInfo client)
	    {
			var destination = Path.Combine(ResourcePath, $@"{client.PatchVer}");

		    if (Directory.Exists(Path.Combine(destination, "gui")))
			    return;
		    else
			    Directory.CreateDirectory(Path.Combine(destination, "gui"));

		    if (client.PackedImages)
		    {
			    string filepath = Path.Combine(client.ClientPath, @"res\packages\gui.pkg");

			    using (var zip = new ZipFile(filepath, Encoding.GetEncoding((int) CodePage.CyrillicDOS)))
			    {
				    zip.ExtractSelectedEntries("name = *.*", @"gui/maps/icons/map", destination,
					    ExtractExistingFileAction.OverwriteSilently);
				    zip.ExtractSelectedEntries("name = *.*", @"gui/maps/icons/map/stats", destination,
					    ExtractExistingFileAction.OverwriteSilently);
				    zip.ExtractSelectedEntries("name = *.*", @"gui/maps/icons/map/small", destination,
					    ExtractExistingFileAction.OverwriteSilently);
					zip.ExtractSelectedEntries("name = *.*", @"gui/maps/icons/vehicle", destination,
					    ExtractExistingFileAction.OverwriteSilently);
				    zip.ExtractSelectedEntries("name = *.*", @"gui/maps/icons/achievement", destination,
					    ExtractExistingFileAction.OverwriteSilently);
				    zip.ExtractSelectedEntries("name = *.*", @"gui/maps/icons/artefact", destination,
					    ExtractExistingFileAction.OverwriteSilently);
				    zip.ExtractSelectedEntries("name = *.*", @"gui/maps/icons/shell", destination,
					    ExtractExistingFileAction.OverwriteSilently);
			    }
		    }
		    else
		    {
			    throw new NotImplementedException("0.7.0 - ручками!!!!!!!!!!!!!!");
		    }
	    }

	    [Test]
        public void UpdateToPatch()
        {
            string destination;
            string source;

	        var prevVer = "";
	        foreach (var client in clients.Where(c => string.IsNullOrEmpty(processedPatch) || c.PatchVer == processedPatch))
	        {
				//CopyScriptsForDecompileCopied(client);
				
		        EnshureScriptsCopied(client);
		        EnshureGameTextResources(client);
				EnsureImportImages(client);

				Console.WriteLine("Generate string resources");
				GenerateStringResources(client);

				Console.WriteLine("Copy achievements definitions");
				ImportAchievementsXml(client);

				Console.WriteLine("Copy maps");
				//ImportMapsXml(client);

				Console.WriteLine("Copy tanks components");
				ImportShellsXml(client);
				ImportArtefactXml(client);

				Console.WriteLine("Copy tanks definitions");
				ImportTanksXml(client);
				ImportActionTanksXml();

				prevVer = client.PatchVer;
	        }


			CopyVehicleImages();
	        CheckTankImages();

	        //CopyMinimapImages();

			//ProcessExpectedValues();


        }

	    public void GenerateStringResources(ClientInfo client)
	    {
		    var destination = Path.Combine(Environment.CurrentDirectory, $@"Output\Strings");
		    if (!Directory.Exists(destination))
			    Directory.CreateDirectory(destination);

		    foreach (var src in Directory.GetFiles(
			    Path.Combine(ResourcePath, $@"{client.PatchVer}\Resources\Text"), "*.resx"))
		    {
			    string locale = "";
			    string fileName = Path.GetFileNameWithoutExtension(src);

			    if (fileName.Contains(".ru"))
			    {
				    locale = ".ru";
				    fileName = fileName.Replace(".ru", "");
			    }
			    var dict = new Dictionary<string, string>();

				var resultName = ResourceHelper.CorrectResourceName($"{fileName}{locale}.resx");
				
			    if (resultName.Contains(".en."))
			    {
				    resultName = resultName.Replace(".en.", ".");
			    }

				if (File.Exists(Path.Combine(destination, resultName)))
			    {
				    using (var rr = new ResXResourceReader(Path.Combine(destination, resultName)))
				    {
					    foreach (DictionaryEntry d in rr)
					    {
						    dict.Add(d.Key.ToString(), d.Value.ToString());
					    }
				    }
			    }
			    using (var rr = new ResXResourceReader(src))
			    {
				    foreach (DictionaryEntry d in rr)
				    {
					    if (dict.ContainsKey(d.Key.ToString()))
						    dict[d.Key.ToString()] = d.Value.ToString();
						else
							dict.Add(d.Key.ToString(), d.Value.ToString());
				    }
			    }
				using (ResXResourceWriter strs = new ResXResourceWriter(Path.Combine(destination, resultName)))
			    {
				    foreach (var pair in dict)
				    {
					    strs.AddResource(pair.Key, pair.Value);
					}
					strs.Generate();   
				}
		    }
	    }


		private void EnshureScriptsCopied()
        {
            var destination = Path.Combine(Environment.CurrentDirectory, @"Output\Patch");
            var scriptsPath = Path.Combine(destination, @"scripts");

            if (!Directory.Exists(scriptsPath))
            {
                string filepath = Path.Combine(clientPath, @"res\packages\scripts.pkg");
                using (var zip = new ZipFile(filepath, Encoding.GetEncoding((int)CodePage.CyrillicDOS)))
                {
                    zip.ExtractAll(destination, ExtractExistingFileAction.OverwriteSilently);
                }
            }
        }


	    private void EnshureScriptsCopied(ClientInfo client)
	    {
			var destination = Path.Combine(ResourcePath, $@"{client.PatchVer}");
		    var scriptsPath = Path.Combine(destination, @"scripts");

		    if (!Directory.Exists(scriptsPath))
		    {
				Directory.CreateDirectory(scriptsPath);
			    if (client.PackedScripts)
			    {
				    string filepath = Path.Combine(client.ClientPath, @"res\packages\scripts.pkg");
				    using (var zip = new ZipFile(filepath, Encoding.GetEncoding((int)CodePage.CyrillicDOS)))
				    {
					    zip.ExtractAll(destination, ExtractExistingFileAction.OverwriteSilently);
				    }
			    }
			    else
			    {
				    string filepath = Path.Combine(client.ClientPath, @"res\scripts");
				    DirectoryCopy(filepath, scriptsPath, true);
			    }
			}
		}



	    private void CopyScriptsForDecompileCopied(ClientInfo client)
	    {
		    var destination = Path.Combine(Environment.CurrentDirectory, $@"C:\55\{client.PatchVer}");
		    var scriptsPath = Path.Combine(destination, @"res\scripts");

		    if (!Directory.Exists(scriptsPath))
		    {
			    Directory.CreateDirectory(scriptsPath);

				File.Copy(Path.Combine(client.ClientPath,"version.xml"), Path.Combine(destination, "version.xml"));
			    File.Copy(Path.Combine(client.ClientPath, "paths.xml"), Path.Combine(destination, "paths.xml"));

				if(Directory.Exists(Path.Combine(client.ClientPath, "res_bw")))
					DirectoryCopy(Path.Combine(client.ClientPath, "res_bw"), Path.Combine(destination, "res_bw"), true);

			    if (Directory.Exists(Path.Combine(client.ClientPath, "res")))
				    DirectoryCopy(Path.Combine(client.ClientPath, "res"), Path.Combine(destination, "res"), false, "*.xml");

				if (client.PackedScripts)
			    {
				    string filepath = Path.Combine(client.ClientPath, @"res\packages\scripts.pkg");
				    using (var zip = new ZipFile(filepath, Encoding.GetEncoding((int)CodePage.CyrillicDOS)))
				    {
					    zip.ExtractAll(destination, ExtractExistingFileAction.OverwriteSilently);
				    }
			    }
			    else
			    {
				    string filepath = Path.Combine(client.ClientPath, @"res\scripts");
				    DirectoryCopy(filepath, scriptsPath, true);
			    }
		    }
	    }

		private void EnshureGameTextResources(ClientInfo client)
	    {
		    string destination;
		    string source;
		    Console.WriteLine("Copy resources");

		    foreach(var lc in new List<(string path, string locale)> { (client.ClientPath, "ru"), (client.EnglishClientPath, "en") })
			{
				destination = Path.Combine(ResourcePath, $@"{client.PatchVer}\text\{lc.locale}");
				source = Path.Combine(lc.path, @"res\text\lc_messages");

				if (!Directory.Exists(destination))
				{
					Directory.CreateDirectory(destination);

					var strings = Directory.GetFiles(source);

					foreach (var resourceFile in strings)
					{
						FileInfo info = new FileInfo(resourceFile);
						info.CopyTo(Path.Combine(destination, info.Name), true);
					}

					var res = Path.Combine(ResourcePath, $@"{client.PatchVer}\Resources\Text\{lc.locale}");
					if (!Directory.Exists(res))
					{
						Directory.CreateDirectory(res);
					}

					string result;
					using (var proc = new Process())
					{
						proc.StartInfo.CreateNoWindow = true;
						proc.StartInfo.UseShellExecute = false;
						proc.StartInfo.RedirectStandardOutput = true;
						proc.StartInfo.FileName = Path.Combine(Environment.CurrentDirectory, @"Tools\convert.cmd");
						proc.StartInfo.StandardOutputEncoding = Encoding.ASCII;
						proc.StartInfo.Arguments = Path.Combine(ResourcePath, $@"{client.PatchVer}") + " " +
						                           Path.Combine(Environment.CurrentDirectory, @"Tools") + " " + lc.locale;


						proc.StartInfo.WorkingDirectory = destination;

						proc.Start();

						result = proc.StandardOutput.ReadToEnd();

						Console.WriteLine(result);

						//write log
						proc.WaitForExit();
					}
				}

			}		    
	    }


		private void CopyGameTextResources()
        {
            string destination;
            string source;
            Console.WriteLine("Copy resources");

            destination = Path.Combine(Environment.CurrentDirectory, $@"Output\Patch\{patchVer}\Resources");
            source = Path.Combine(clientPath, @"res\text\lc_messages");

            Directory.CreateDirectory(destination);

            var strings = Directory.GetFiles(source, "*_vehicles.mo");

            foreach (var resourceFile in strings)
            {
                FileInfo info = new FileInfo(resourceFile);
                info.CopyTo(Path.Combine(destination, info.Name), true);
            }

            string result;
            using (var proc = new Process())
            {
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.FileName = Path.Combine(destination, Path.Combine(Environment.CurrentDirectory, @"Tools\convert.bat"));

                proc.StartInfo.WorkingDirectory = destination;

                proc.Start();

                result = proc.StandardOutput.ReadToEnd();

                Console.WriteLine(result);

                //write log
                proc.WaitForExit();
            }
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, string filemask = "*.*", bool overwrite = true)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles(filemask);
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, overwrite);
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, CultureInfo.CurrentCulture.TextInfo.ToTitleCase(subdir.Name));
                    DirectoryCopy(subdir.FullName, temppath, true, filemask, overwrite);
                }
            }
        }

		[Test]
	    public static void ProcessExpectedValues()
	    {
		    #region Subst

		    var subst = new List<(string from, string to)>
		    {
			    (from: "G92_VK7201P", to: "G134_PzKpfw_VII"),
			    (from: "E-100", to: "G56_E-100"),
			    (from: "IS-4", to: "R90_IS_4M"),
			    (from: "IS-7", to: "R45_IS-7"),
			    (from: "T57_58", to: "A67_T57_58"),
			    (from: "A84_M48A1", to: "A120_M48A5"),
			    (from: "Object_907A", to: "R95_Object_907A"),
			    (from: "Object_430", to: "R96_Object_430"),
			    (from: "Bat_Chatillon155", to: "F38_Bat_Chatillon155_58"),
			    (from: "Waffentrager_E100", to: "G98_Waffentrager_E100"),
			    (from: "M46_Patton", to: "A63_M46_Patton"),

			    (from: "JagdTiger", to: "G44_JagdTiger"),
			    (from: "T95", to: "GB87_Chieftain_T95_turret"),
			    (from: "T110E5", to: "A69_T110E5"),
			    (from: "Object_907", to: "R95_Object_907"),
			    (from: "M48A1", to: "A120_M48A5"),
			    (from: "T62A", to: "R87_T62A"),
			    (from: "T92", to: "A38_T92"),
			    (from: "VK4502P", to: "G58_VK4502P"),
			    (from: "IS-3", to: "R19_IS-3"),
			    (from: "KV4", to: "R73_KV4"),
			    (from: "PzVIB_Tiger_II", to: "G16_PzVIB_Tiger_II"),
			    (from: "Object252", to: "R61_Object252"),
			    (from: "T23", to: "A08_T23"),
			    (from: "Pershing", to: "A35_Pershing"),
			    (from: "T34_hvy", to: "A13_T34_hvy"),

			    (from: "T28", to: "A39_T28"),
			    (from: "PzVI_IGR", to: "G04_PzVI_Tiger_I_IGR"),
			    (from: "IS", to: "R01_IS"),
			    (from: "PzVI", to: "G04_PzVI_Tiger_I"),
			    (from: "T71_IGR", to: "A103_T71E1_IGR"),

			    (from: "T71", to: "A103_T71E1"),
			    (from: "PzV_IGR", to: "G03_PzV_Panther_IGR"),
			    (from: "T44_85", to: "R98_T44_85"),
			    (from: "A44", to: "R59_A44"),

			    (from: "PzV", to: "G03_PzV_Panther"),
			    (from: "VK3002DB", to: "G24_VK3002DB"),
			    (from: "SU-152_IGR", to: "R18_SU-152_IGR"),
			    (from: "JagdPanther", to: "G18_JagdPanther"),
			    (from: "SU-152", to: "R18_SU-152"),
			    (from: "M6", to: "A10_M6"),
			    (from: "T_50_2", to: "R70_T_50_2"),
			    (from: "PzV_PzIV", to: "G32_PzV_PzIV"),
			    (from: "M4A3E8_Sherman", to: "A06_M4A3E8_Sherman"),
			    (from: "T-34-85", to: "R07_T-34-85"),
			    (from: "PzV_PzIV_ausf_Alfa", to: "G32_PzV_PzIV_ausf_Alfa"),
			    (from: "SU-8", to: "R26_SU-8"),
			    (from: "R38_KV-220_action", to: "R38_KV-220_beta"),

			    (from: "KV", to: "R05_KV"),
			    (from: "KV-220-2", to: "R38_KV-220"),
			    (from: "KV-220", to: "R38_KV-220_beta"),
			    (from: "KV-220_action", to: "R38_KV-220_beta"),
			    (from: "M24_Chaffee", to: "A34_M24_Chaffee"),
			    (from: "PzIV", to: "G32_PzV_PzIV_ausf_Alfa"),
			    (from: "T-34", to: "R04_T-34"),
			    (from: "Chi_Nu", to: "J08_Chi_Nu"),
			    (from: "Grille", to: "G23_Grille"),
			    (from: "M41", to: "A18_M41"),

			    (from: "StuGIII", to: "G05_StuG_40_AusfG"),
			    (from: "B1", to: "F04_B1"),
			    (from: "PzIII_AusfJ", to: "G10_PzIII_AusfJ"),
			    (from: "PzIII", to: "G10_PzIII_AusfJ"),
			    (from: "A43", to: "R57_A43"),

			    (from: "T40", to: "A29_T40"),
			    (from: "G14_PzIII_A", to: "G102_Pz_III"),
			    (from: "PzIII_A", to: "G102_Pz_III"),
			    (from: "Ke_Ni", to: "J04_Ke_Ni"),
			    (from: "Chi_Ha", to: "Ch08_Type97_Chi_Ha"),
			    (from: "SU-76", to: "R24_SU-76"),
			    (from: "PzI", to: "G53_PzI"),
			    (from: "PzII", to: "G06_PzII"),
			    (from: "T-26", to: "R09_T-26"),
			    (from: "T2_med", to: "A24_T2_med"),
			    (from: "T57", to: "A107_T1_HMC"),
			    (from: "T18", to: "A46_T3"),
			    (from: "Ltraktor", to: "G12_Ltraktor"),
			    (from: "MS-1", to: "R11_MS-1"),
			    (from: "RenaultFT", to: "F01_RenaultFT"),
			    (from: "T1_Cunningham", to: "A01_T1_Cunningham"),
			    (from: "VK7201P", to: "G134_PzKpfw_VII"),
			    (from: "NC27", to: "J01_NC27"),
			};

		    #endregion

			var htmldata = new Uri("http://tanks.noobmeter.com/tankList").Download();

		    var pos = htmldata.IndexOf(@"<table class=""tablesorter");
		    var endpos = htmldata.IndexOf("</table>", pos);

		    var data = htmldata.Substring(pos, endpos - pos + "</table>".Length).Replace("&nbsp;", " ");
		    var xdoc = XDocument.Parse(data);
		    var list = Dictionaries.Instance.AllVehicles.Values;
		    var result = new RatingExpectedValues()
		    {
			    Header = new RatingExpectedValuesHeader {Version = Convert.ToDouble(DateTime.Now.ToString("yyyyMMdd"))},
			    Data = new List<RatingExpectedValuesData>()
		    };

			foreach (var tr in xdoc.Root.Element("tbody").Elements("tr"))
		    {
			    var hlink = tr.Elements().Skip(1).First();
			    var link = hlink.Elements("a").Attributes("href").First().Value.Replace("/tank/ru/", "").Replace("/tank/eu/", "").Replace("/tank/na/", "");
			    var nt = subst.FirstOrDefault(s => s.from.Equals(link));
			    if (nt.to != null)
			    {
				    link = nt.to;
			    }
				var tank = list.FirstOrDefault(x => x.Key.Equals(link, StringComparison.InvariantCultureIgnoreCase));
			    if (tank == null)
			    {
				    var count = list.Count(x => x.Key.Contains(link) &&
				                                ((link.Contains("IGR") && x.Key.Contains("IGR")) ||
				                                 (!link.Contains("IGR") && !x.Key.Contains("IGR"))) &&
				                                ((link.Contains("fallout") && x.Key.Contains("fallout")) ||
				                                 (!link.Contains("fallout") && !x.Key.Contains("fallout"))) &&
				                                ((link.Contains("bootcamp") && x.Key.Contains("bootcamp")) ||
				                                 (!link.Contains("bootcamp") && !x.Key.Contains("bootcamp")))
				    );
					if(count == 1)
				    {
					    tank = list.First(x => x.Key.Contains(link) &&
					                           ((link.Contains("IGR") && x.Key.Contains("IGR")) || (!link.Contains("IGR") && !x.Key.Contains("IGR"))) &&
					                           ((link.Contains("fallout") && x.Key.Contains("fallout")) || (!link.Contains("fallout") && !x.Key.Contains("fallout"))) &&
					                           ((link.Contains("bootcamp") && x.Key.Contains("bootcamp")) || (!link.Contains("bootcamp") && !x.Key.Contains("bootcamp"))));
				    }
					else if (count == 0)
					{
						count = list.Count(x => x.userString.Contains(link) &&
						                        ((link.Contains("IGR") && x.userString.Contains("IGR")) ||
						                         (!link.Contains("IGR") && !x.userString.Contains("IGR"))) &&
						                        ((link.Contains("fallout") && x.userString.Contains("fallout")) ||
						                         (!link.Contains("fallout") && !x.userString.Contains("fallout"))) &&
						                        ((link.Contains("bootcamp") && x.userString.Contains("bootcamp")) ||
						                         (!link.Contains("bootcamp") && !x.userString.Contains("bootcamp")))
						);
						if (count == 1)
						{
							tank = list.First(x => x.userString.Contains(link) &&
							                       ((link.Contains("IGR") && x.userString.Contains("IGR")) || (!link.Contains("IGR") && !x.userString.Contains("IGR"))) &&
							                       ((link.Contains("fallout") && x.userString.Contains("fallout")) || (!link.Contains("fallout") && !x.userString.Contains("fallout"))) &&
							                       ((link.Contains("bootcamp") && x.userString.Contains("bootcamp")) || (!link.Contains("bootcamp") && !x.userString.Contains("bootcamp"))));
						}
					}
				    if(tank == null)
				    {
					    var title = hlink.Elements("a").First().Value.Trim(new[] {' ', '\r', '\n', '\t'});
					    tank = list.FirstOrDefault(x => x.Key.Equals(title, StringComparison.InvariantCultureIgnoreCase));
					    if (tank == null)
					    {
						    nt = subst.FirstOrDefault(s => s.from.Equals(title));
						    if (nt.to == null)
						    {
							    if (list.Count(x => x.Key.Contains(title) &&
							                        ((title.Contains("IGR") && x.Key.Contains("IGR")) || (!title.Contains("IGR") && !x.Key.Contains("IGR"))) &&
							                        ((title.Contains("fallout") && x.Key.Contains("fallout")) || (!title.Contains("fallout") && !x.Key.Contains("fallout"))) &&
							                        ((title.Contains("bootcamp") && x.Key.Contains("bootcamp")) || (!title.Contains("bootcamp") && !x.Key.Contains("bootcamp")))) != 1)
							    {
								    throw new NotImplementedException();
							    }
							    else
								    tank = list.First(x => x.Key.Contains(title) &&
								                           ((title.Contains("IGR") && x.Key.Contains("IGR")) || (!title.Contains("IGR") && !x.Key.Contains("IGR"))) &&
								                           ((title.Contains("fallout") && x.Key.Contains("fallout")) || (!title.Contains("fallout") && !x.Key.Contains("fallout"))) &&
								                           ((title.Contains("bootcamp") && x.Key.Contains("bootcamp")) || (!title.Contains("bootcamp") && !x.Key.Contains("bootcamp"))));
						    }
						    else
							    tank = list.FirstOrDefault(x => x.Key.Equals(nt.to, StringComparison.InvariantCultureIgnoreCase));
					    }
				    }
			    }
				if(tank == null)
					throw new NotImplementedException();

				if (result.Data.Any(d=>d.CompDescr == tank.CompDescr)) continue;
			    var dmg = tr.Elements().Skip(5).First().Value.Trim().Replace(" ", "").Replace(",", "");
			    if(!Double.TryParse(dmg, out var value))continue;
				result.Data.Add(new RatingExpectedValuesData
				{
					CompDescr = tank.CompDescr,
					Damage = value,
					
				});
			}

		    var path = Path.Combine(Environment.CurrentDirectory, $@"Output\ExpectedValues");
		    if (!Directory.Exists(path))
			    Directory.CreateDirectory(path);

			File.WriteAllText(Path.Combine(path, "expectedValues_PR.json"), JsonConvert.SerializeObject(result, Formatting.Indented));

		    var jsonData = new Uri("http://www.wnefficiency.net/exp/expected_tank_values_30.json").Download();
		    File.WriteAllText(Path.Combine(path, "expectedValues_Base.json"), jsonData);

		    jsonData = new Uri("https://kttc.ru/wn8kttc/expected_kttc_3.json").Download();
		    File.WriteAllText(Path.Combine(path, "expectedValues_KTTC.json"), jsonData);

		    jsonData = new Uri("http://stat.modxvm.com/wn8.json").Download();
		    File.WriteAllText(Path.Combine(path, "expectedValues_XVM.json"), jsonData);

		}

		[Test]
        public void GetClanInfoTest()
        {
            var appSettings = SettingsReader.Get();
            WotApiClient.Instance.GetClanMemberInfo(3016489, appSettings);
        }
/*
        [Test]
        public void GenMapsImagesWithBases()
        {
            foreach (var map in Dictionaries.Instance.Maps)
            {
                var mapDescription = map.Value;
                var outFileName = mapDescription.LocalizedMapName + ".png";

                var replayMap = new ReplayMap
                {
                    Gameplay = Gameplay.ctf,
                    MapId = mapDescription.MapId,
                    MapName = mapDescription.MapName,
                    MapNameId = mapDescription.MapNameId,
                    Team = 1
                };

                var mapImage = (BitmapImage)MapToMinimapImageConverter.Default.Convert(replayMap, null, null, CultureInfo.InvariantCulture);

                MapElementContext elementContext = new MapElementContext(mapDescription, replayMap.Gameplay.ToString(), 1, 300, 300);

                var mapImageElements = elementContext.GetMapImageElements();

                Bitmap bitmap = new Bitmap(Convert.ToInt32(300), Convert.ToInt32(300), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Graphics mainImage = Graphics.FromImage(bitmap);

                mainImage.DrawImage(GetImage(mapImage),  new Point(0, 0));

                foreach (var mapImageElement in mapImageElements)
                {
                    var baseImage = (BitmapImage)MapImageElementToIconConverter.Default.Convert(mapImageElement, null, null, CultureInfo.InvariantCulture);

                    mainImage.DrawImage(GetImage(baseImage), new Point((int) mapImageElement.X, (int) mapImageElement.Y));
                }

                bitmap.Save(outFileName, ImageFormat.Png);
            }
        }

        private Graphics ToGraphics(BitmapImage bitmapImage)
        {
            var image = GetImage(bitmapImage);

            return Graphics.FromImage(image);
        }

        private static Image GetImage(BitmapImage bitmapImage)
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

            MemoryStream stream = new MemoryStream();
            encoder.Save(stream);
            stream.Position = 0;

            var image = System.Drawing.Image.FromStream(stream);
            return image;
        }

        private class ReplayMap : IReplayMap
        {
            public Gameplay Gameplay { get; set; }
            public string MapName { get; set; }
            public int MapId { get; set; }
            public string MapNameId { get; set; }
            public int Team { get; set; }
        }*/
    }
}