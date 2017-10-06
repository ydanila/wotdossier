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
using Ionic.Zip;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
	    private string patchVer = "0.9.20";


	    private string processedPatch = "";

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
		    new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.7.1.1", PatchVer="0.7.1", PackedScripts=false, PackedImages=false, EnglishClientPath=@"c:\66\World_of_Tanks - 0.7.1.1"},
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
		    //new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.8.4", PatchVer="0.8.4", Packed=false, EnglishClientPath="ru-RU"},
		    new ClientInfo{ClientPath= @"s:\WorldOfTanks", PatchVer="0.9.20", PackedScripts=true, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.9.20"},
		    new ClientInfo{ClientPath= @"f:\Games\WorldOfTanks_0.9.20.1.CT", PatchVer="0.9.20.1", PackedScripts=true, PackedImages=true, EnglishClientPath=@"c:\66\World_of_Tanks - 0.9.20.1.CT"},
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
            JObject expectedValues;
            using (StreamReader re = new StreamReader(@"Data\expected_tank_values.json"))
            {
                JsonTextReader reader = new JsonTextReader(re);
                JsonSerializer se = new JsonSerializer();
                expectedValues = se.Deserialize<JObject>(reader);
            }

            List<RatingExpectancy> xmlTanks = new List<RatingExpectancy>();
            foreach (var expectedValue in expectedValues["data"])
            {
                int idNum = (int) expectedValue["IDNum"];

                if (Dictionaries.Instance.Tanks.Values.FirstOrDefault(x => x.CompDescr == idNum) == null)
                {
                    expectedValue["tankId"] = idNum >> 8 & 65535;
                    expectedValue["countryId"] = idNum >> 4 & 15;
                    Console.WriteLine(expectedValue);
                    continue;
                }

                var tankDescription = Dictionaries.Instance.Tanks.Values.First(x => x.CompDescr == idNum);

                RatingExpectancy ratingExpectancy = new RatingExpectancy();
                double prNominalDamage = (double) expectedValue["expDamage"];
                ratingExpectancy.PRNominalDamage = tankDescription.Expectancy.PRNominalDamage;
                ratingExpectancy.TankLevel = tankDescription.Tier;
                ratingExpectancy.TankType = (TankType) tankDescription.Type;
                ratingExpectancy.Wn8NominalDamage = prNominalDamage;
                ratingExpectancy.Wn8NominalWinRate = (double)expectedValue["expWinRate"];
                ratingExpectancy.Wn8NominalSpotted = (double)expectedValue["expSpot"];
                ratingExpectancy.Wn8NominalFrags = (double)expectedValue["expFrag"];
                ratingExpectancy.Wn8NominalDefence = (double)expectedValue["expDef"];
                ratingExpectancy.Icon = tankDescription.Icon.IconOrig;
                ratingExpectancy.TankTitle = tankDescription.Title;
                ratingExpectancy.CompDescr = tankDescription.CompDescr;

                xmlTanks.Add(ratingExpectancy);
            }

            foreach (TankDescription description in Dictionaries.Instance.Tanks.Values)
            {
                if (xmlTanks.FirstOrDefault(x => string.Equals(x.Icon, description.Icon.IconOrig, StringComparison.InvariantCultureIgnoreCase)) == null)
                {
                    Console.WriteLine(description.Icon.IconOrig);
                }
            }

            Console.WriteLine(JsonConvert.SerializeObject(xmlTanks.OrderBy(x => x.TankTitle), Formatting.Indented));
        }

        [Test]
        public void CsvExportProviderTest()
        {
            CsvExportProvider provider = new CsvExportProvider();
            FileInfo cacheFile = CacheTestFixture.GetCacheFile("_rembel__ru", @"\CacheFiles\0.8.9\");
            List<TankJson> tanks = CacheFileHelper.ReadTanksCache(CacheFileHelper.BinaryCacheToJson(cacheFile));
            List<RandomBattlesTankStatisticRowViewModel> list = tanks.Select(x => new RandomBattlesTankStatisticRowViewModel(x, new List<StatisticSlice>())).ToList();
            Console.WriteLine(provider.Export(list, new List<Type>{typeof(IStatisticBattles), typeof(IStatisticFrags)}));
        }

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

        [Test]
        public void MapsResourcesTest()
        {
            var dictionary = GetResourcesDictionary();

            foreach (var map in Dictionaries.Instance.Maps)
            {
                var localizedString = Resources.Resources.ResourceManager.GetString("Map_" + map.Value.MapNameId);
                Assert.IsNotNull(localizedString, "Resource not found: {0}", map.Value.MapNameId);

                var key = string.Format("images/maps/{0}.png", map.Value.MapNameId).ToLowerInvariant();
                //Assert.IsTrue(dictionary.ContainsKey(key), "Image resource not found: {0}", map.Value.mapidname);
                if (!dictionary.ContainsKey(key))
                {
                    Console.WriteLine("Image resource not found: {0}", map.Value.MapNameId);
                }
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
        public void TanksResourcesTest()
        {
            var dictionary = GetResourcesDictionary();

            foreach (var tank in Dictionaries.Instance.Tanks)
            {
                var key = string.Format("images/tanks/{0}.png", tank.Value.Icon.IconId).ToLowerInvariant();
                //Assert.IsTrue(dictionary.ContainsKey(key), "Image resource not found: {0}", tank.Value.Icon.IconId);
                if (!dictionary.ContainsKey(key))
                {
                    Console.WriteLine("Image resource not found: {0}", tank.Value.Icon.IconId);
                }
                else
                {
                    File.Copy(Environment.CurrentDirectory + "\\..\\..\\..\\WotDossier.Resources\\" + key.Replace("/", "\\"), string.Format(@"d:\1\{0}.png", tank.Value.Icon.IconId));
                }
            }
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

   //     [Test]
   //     public void ImportTanksXmlTest()
   //     {
   //         EnshureScriptsCopied();

   //         CopyGameTextResources();

   //         Console.WriteLine("Copy tanks definitions");

   //         var scriptsPath = Path.Combine(Environment.CurrentDirectory, @"Output\Patch\Scripts");
   //         var destination = Path.Combine(Environment.CurrentDirectory, $@"Output\Patch\{patchVer}\Tanks");
   //         var source = Path.Combine(scriptsPath, @"item_defs\vehicles");

   //         Directory.CreateDirectory(destination);

   //         DirectoryCopy(source, destination, true);

   //         var strings = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, $@"Output\Patch\{patchVer}\Tanks"), "list.xml", SearchOption.AllDirectories);

   //         List<JObject> result = new List<JObject>();

   //         StringBuilder codegen = new StringBuilder();

   //         Dictionary<string, string> resources = new Dictionary<string, string>();

   //         foreach (var xml in strings)
   //         {
   //             BigWorldXmlReader reader = new BigWorldXmlReader();
   //             FileInfo info = new FileInfo(xml);
   //             using (BinaryReader br = new BinaryReader(info.OpenRead()))
   //             {
   //                 var xmlContent = reader.DecodePackedFile(br, "vehicles");
   //                 XmlDocument doc = new XmlDocument();
   //                 doc.LoadXml(xmlContent);
   //                 string jsonText = JsonConvert.SerializeXmlNode(doc, Formatting.Indented);

   //                 var dictionary = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(jsonText);
   //                 dictionary = dictionary["vehicles"].ToObject<Dictionary<string, JObject>>();

			//		//{"tankid": 0, "countryid": 0, "compDescr": 1, "active": 1, "type": 2, 
			//		//"type_name": "MT", "tier": 5, "premium": 0, "title": "T-34", "icon": "r04_t_34", "icon_orig": "R04_T-34"},
					
	  //              List<JObject> tanks = new List<JObject>();
   //                 foreach (var tank in dictionary)
   //                 {
   //                     JObject tankDescription = new JObject();
   //                     var tankid = tank.Value["id"].Value<int>();
   //                     tankDescription["tankid"] = tankid;
   //                     Country country = (Country)Enum.Parse(typeof(Country), info.Directory.Name);
	  //                  var countryid = (int)country;
   //                     tankDescription["countryid"] = countryid;
   //                     var typeCompDesc = Utils.TypeCompDesc(countryid, tankid);
   //                     tankDescription["compDescr"] = typeCompDesc;
   //                     var uniqueId = Utils.ToUniqueId(countryid, tankid);
   //                     tankDescription["active"] = Dictionaries.Instance.Tanks.ContainsKey(uniqueId) && !Dictionaries.Instance.Tanks[uniqueId].Active ? 0 : 1;
   //                     var tankType = GetVehicleTypeByTag(tank.Value["tags"].Value<string>());
   //                     tankDescription["type"] = (int)tankType;
   //                     tankDescription["type_name"] = tankType.ToString();
   //                     tankDescription["tier"] = tank.Value["level"].Value<int>();
   //                     tankDescription["premium"] = Dictionaries.Instance.Tanks.ContainsKey(uniqueId) ? Dictionaries.Instance.Tanks[uniqueId].Premium :
   //                     tank.Value["notInShop"] == null ? 0 : 1;
   //                     var key = tank.Value["userString"].Value<string>().Split(':')[1];
   //                     var value = GetString(key);
   //                     tankDescription["title"] = value;
   //                     var titleShort = tank.Value["shortUserString"];
   //                     if (titleShort != null)
   //                     {
   //                         tankDescription["title_short"] = GetString(titleShort.Value<string>().Split(':')[1]);
   //                     }
   //                     var icon = tank.Key.Replace("-", "_").ToLower();

   //                     if (countryid == 0)
   //                     {
   //                         resources.Add(icon, value);
   //                     }

   //                     tankDescription["icon"] = icon;
   //                     tankDescription["icon_orig"] = tank.Key;

   //                     if (!Dictionaries.Instance.Tanks.ContainsKey(uniqueId))
   //                     {
   //                         Console.WriteLine(tank.Value);
   //                     }
   //                     else
   //                     {
   //                         var description = Dictionaries.Instance.Tanks[uniqueId];
   //                         if (description.Icon.Icon != (string)tankDescription["icon"])
   //                         {
   //                             string f = @"else if (iconId == ""{0}_{1}"")
   //                             {{
   //                                 //{4} replay tank name changed to {2}
   //                                 tankDescription = tankDescriptions[{3}];
   //                             }}";
   //                             codegen.AppendFormat(f, country.ToString().ToLower(), description.Icon.Icon, tankDescription["icon"], uniqueId, Dictionaries.VersionRelease);
   //                             codegen.AppendLine();
   //                         }
   //                     }

   //                     JObject tankDef = GetTankDefinition(countryid, tank.Key);

   //                     if (tankDef != null)
   //                     {
   //                         //Console.WriteLine(tankDef.ToString(Formatting.Indented));
   //                         var health = tankDef.SelectToken("$.vehicles.hull.maxHealth").Value<int>() + tankDef.SelectTokens("$.vehicles.turrets0..maxHealth").First().Value<int>();
   //                         tankDescription["health"] = health;
   //                     }

   //                     tanks.Add(tankDescription);
   //                 }
   //                 result.AddRange(tanks);
   //             }
   //         }
			////Add Action tanks
			//AddActionTanks(result);

			//using (ResXResourceWriter writer = new ResXResourceWriter(Path.Combine(Environment.CurrentDirectory, @"Output\Patch\Resources\ussr_vehicles_out.resx")))
   //         {
   //             foreach (var resource in resources)
   //             {
   //                 writer.AddResource(resource.Key, resource.Value);
   //             }
   //         }

   //         var serializeObject = JsonConvert.SerializeObject(result
   //             .OrderBy(x => GetOrder(x["countryid"].Value<int>()))
   //             .ThenBy(x => x["tankid"].Value<int>()));
   //         var tanksJson = serializeObject.Replace("{", "\n{").Replace(",\"", ", \"").Replace(":", ": ");

   //         var path = Path.Combine(Environment.CurrentDirectory, @"Output\Externals");

   //         if (!Directory.Exists(path))
   //         {
   //             Directory.CreateDirectory(path);
   //         }

   //         path = Path.Combine(path, "tanks.json");

   //         var stream = File.OpenWrite(path);
   //         using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
   //         {
   //             writer.Write(tanksJson);
   //         }

   //         Console.WriteLine(tanksJson);
   //         Console.WriteLine(codegen);
   //     }

	    public void ImportTanksXml(ClientInfo client)
		{
			var scriptsPath = Path.Combine(Environment.CurrentDirectory, $@"Output\{client.PatchVer}\scripts");
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
						var typeCompDesc = Utils.TypeCompDesc(countryid, id);
						var uniqueId = Utils.ToUniqueId(countryid, id);
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

						//tankDescription["type"] = (int)tankType;
						//tankDescription["type_name"] = tankType.ToString();
						var icon = countryName.ToLower() + "-" + key + ".png";
						var tgticon = countryName.ToLower() + "-" + key + "." + typeCompDesc + ".png";
						if (!File.Exists(Path.Combine(Environment.CurrentDirectory, $@"Output\{client.PatchVer}\gui\maps\icons\vehicle",
							icon)))
						{
							if (File.Exists(Path.Combine(Environment.CurrentDirectory, $@"Patch\Images\vehicle", icon)))
								File.Copy(Path.Combine(Environment.CurrentDirectory, $@"Patch\Images\vehicle", icon), Path.Combine(destination, $@"Images\Vehicle", tgticon), true);
							else
								Console.WriteLine(Path.Combine(Environment.CurrentDirectory, $@"Output\{client.PatchVer}\gui\maps\icons\vehicle", icon));
						}
						else
							File.Copy(Path.Combine(Environment.CurrentDirectory, $@"Output\{client.PatchVer}\gui\maps\icons\vehicle", icon), Path.Combine(destination, $@"Images\Vehicle", tgticon), true);

					}

					//{"tankid": 0, "countryid": 0, "compDescr": 1, "active": 1, "type": 2, 
					//"type_name": "MT", "tier": 5, "premium": 0, "title": "T-34", "icon": "r04_t_34", "icon_orig": "R04_T-34"},

					//List<JObject> tanks = new List<JObject>();
					//foreach (var tank in dictionary)
					//{
						
						
					//	tankDescription["active"] = Dictionaries.Instance.Tanks.ContainsKey(uniqueId) && !Dictionaries.Instance.Tanks[uniqueId].Active ? 0 : 1;
						
					//	tankDescription["premium"] = Dictionaries.Instance.Tanks.ContainsKey(uniqueId) ? Dictionaries.Instance.Tanks[uniqueId].Premium :
					//	tank.Value["notInShop"] == null ? 0 : 1;
					//	var key = tank.Value["userString"].Value<string>().Split(':')[1];
					//	var value = GetString(key);
					//	tankDescription["title"] = value;
					//	var titleShort = tank.Value["shortUserString"];
					//	if (titleShort != null)
					//	{
					//		tankDescription["title_short"] = GetString(titleShort.Value<string>().Split(':')[1]);
					//	}
					//	var icon = tank.Key.Replace("-", "_").ToLower();

					//	if (countryid == 0)
					//	{
					//		resources.Add(icon, value);
					//	}

					//	tankDescription["icon"] = icon;
					//	tankDescription["icon_orig"] = tank.Key;

					//	if (!Dictionaries.Instance.Tanks.ContainsKey(uniqueId))
					//	{
					//		Console.WriteLine(tank.Value);
					//	}
					//	else
					//	{
					//		var description = Dictionaries.Instance.Tanks[uniqueId];
					//		if (description.Icon.Icon != (string)tankDescription["icon"])
					//		{
					//			string f = @"else if (iconId == ""{0}_{1}"")
     //                           {{
     //                               //{4} replay tank name changed to {2}
     //                               tankDescription = tankDescriptions[{3}];
     //                           }}";
					//			codegen.AppendFormat(f, country.ToString().ToLower(), description.Icon.Icon, tankDescription["icon"], uniqueId, Dictionaries.VersionRelease);
					//			codegen.AppendLine();
					//		}
					//	}

					//	JObject tankDef = GetTankDefinition(countryid, tank.Key);

					//	if (tankDef != null)
					//	{
					//		//Console.WriteLine(tankDef.ToString(Formatting.Indented));
					//		var health = tankDef.SelectToken("$.vehicles.hull.maxHealth").Value<int>() + tankDef.SelectTokens("$.vehicles.turrets0..maxHealth").First().Value<int>();
					//		tankDescription["health"] = health;
					//	}

					//	tanks.Add(tankDescription);
					//}
					//result.AddRange(tanks);
				}
			}
			resultDoc.Save(Path.Combine(destination, "Vehicles.xml"));
			//Add Action tanks
			//AddActionTanks(result);

			//using (ResXResourceWriter writer = new ResXResourceWriter(Path.Combine(Environment.CurrentDirectory, @"Output\Patch\Resources\ussr_vehicles_out.resx")))
			//{
			//	foreach (var resource in resources)
			//	{
			//		writer.AddResource(resource.Key, resource.Value);
			//	}
			//}

			//var serializeObject = JsonConvert.SerializeObject(result
			//	.OrderBy(x => GetOrder(x["countryid"].Value<int>()))
			//	.ThenBy(x => x["tankid"].Value<int>()));
			//var tanksJson = serializeObject.Replace("{", "\n{").Replace(",\"", ", \"").Replace(":", ": ");

			//var path = Path.Combine(Environment.CurrentDirectory, @"Output\Externals");

			//if (!Directory.Exists(path))
			//{
			//	Directory.CreateDirectory(path);
			//}

			//path = Path.Combine(path, "tanks.json");

			//var stream = File.OpenWrite(path);
			//using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
			//{
			//	writer.Write(tanksJson);
			//}

			//Console.WriteLine(tanksJson);
			//Console.WriteLine(codegen);
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

			#region Lanchester
			patchNode.Element("GB90_Lanchester_Armored_Car")?.Remove();
		    patchNode.Add(new XElement("GB90_Lanchester_Armored_Car",
			    new XAttribute("id", 234),
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

			var icon = "uk-GB90_Lanchester_Armored_Car.png";
		    var tgticon = "uk-GB90_Lanchester_Armored_Car." + 56913 + ".png";
		    if (File.Exists(Path.Combine(Environment.CurrentDirectory, $@"Patch\Images\vehicle", icon)))
				File.Copy(Path.Combine(Environment.CurrentDirectory, $@"Patch\Images\vehicle", icon), Path.Combine(destination, $@"Images\Vehicle", tgticon), true);
			else
				Console.WriteLine(icon);
			#endregion
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
		    var fileName = Path.Combine(Environment.CurrentDirectory, $@"Output\{patchVer}\scripts\item_defs\vehicles", ((Country)countryid).ToString(), tankName + ".xml");
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

		private JObject GetTankDefinition(int countryid, string tankName)
        {
            var fileName = Path.Combine(Environment.CurrentDirectory, $@"Output\Patch\{patchVer}\Tanks", ((Country)countryid).ToString(), tankName + ".xml");
            if (File.Exists(fileName))
            {
                var file = new FileInfo(fileName);
                BigWorldXmlReader reader = new BigWorldXmlReader();
                using (BinaryReader br = new BinaryReader(file.OpenRead()))
                {
                    var xmlContent = reader.DecodePackedFile(br, "vehicles");
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlContent);
                    var value = JsonConvert.SerializeXmlNode(doc, Formatting.Indented);
                    return JsonConvert.DeserializeObject<JObject>(value);
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
		    var configsPath = Path.Combine(Environment.CurrentDirectory, $@"Output\{client.PatchVer}\scripts\arena_defs");

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
						doc.Add(new XAttribute("key", mapKey));
						patchNode.Add(doc);
				    }
			    }
		    }
		    resultDoc.Save(Path.Combine(path, "Maps.xml"));
			DirectoryCopy(Path.Combine(Environment.CurrentDirectory, $@"Output\{client.PatchVer}\gui\maps\icons\map\stats"), Path.Combine(path, @"Images\Stats"), false, "*.png");

		}


	    public void ImportMapMinimapsXml(ClientInfo client, string prev)
	    {
		    var path = Path.Combine(Environment.CurrentDirectory, $@"Output\Maps\Images\Minimap");

		    if (!Directory.Exists(path))
			    Directory.CreateDirectory(path);

		    var srcPath = Path.Combine(Environment.CurrentDirectory, $@"Output\{client.PatchVer}\gui\maps\icons\map");

		    foreach (var srcFile in Directory.GetFiles(srcPath, "*.png", SearchOption.TopDirectoryOnly))
		    {
			    var fn = Path.GetFileName(srcFile);
				var tgtFile = Path.Combine(path, fn);
			    if (!File.Exists(tgtFile))
			    {
				    File.Copy(srcFile, tgtFile);
					continue;
			    }
			    if(File.ReadAllBytes(srcFile).Compare(File.ReadAllBytes(tgtFile))) continue;

				File.Move(tgtFile, Path.Combine(path, Path.GetFileNameWithoutExtension(srcFile) + $"-{prev}.png"));
			    File.Copy(srcFile, tgtFile);
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

			var strings = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, $@"Output\{client.PatchVer}\scripts"), "shells.xml",
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
						countryNode.Add(elem);
					}
			    }
		    }
		    resultDoc.Save(Path.Combine(path, "Shells.xml"));

		    DirectoryCopy(Path.Combine(Environment.CurrentDirectory, $@"Output\{client.PatchVer}\gui\maps\icons\shell"), Path.Combine(path, @"Images\Shell"), false, "*.png");
			
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

			var fn = Path.Combine(Environment.CurrentDirectory,
				$@"Output\{client.PatchVer}\scripts\item_defs\vehicles\common\equipments.xml");
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

			fn = Path.Combine(Environment.CurrentDirectory,
				$@"Output\{client.PatchVer}\scripts\item_defs\vehicles\common\optional_devices.xml");
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

			DirectoryCopy(Path.Combine(Environment.CurrentDirectory, $@"Output\{client.PatchVer}\gui\maps\icons\artefact"), Path.Combine(path, @"Images\Artefact"), false, "*.png");
		}

		public void ImportAchievementsXml(ClientInfo client)
		{
			var xmlPath = Path.Combine(Environment.CurrentDirectory, $@"Output\{client.PatchVer}\scripts\item_defs\achievements.xml");
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

			DirectoryCopy(Path.Combine(Environment.CurrentDirectory, $@"Output\{client.PatchVer}\gui\maps\icons\achievement"), Path.Combine(path, @"Images\Achievement"), false, "*.png");
			
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
			var destination = Path.Combine(Environment.CurrentDirectory, $@"Output\{client.PatchVer}");

		    if (Directory.Exists(Path.Combine(destination, "gui")))
			    return;
			
		    string filepath = Path.Combine(client.ClientPath, @"res\packages\gui.pkg");

		    using (var zip = new ZipFile(filepath, Encoding.GetEncoding((int)CodePage.CyrillicDOS)))
		    {
			    zip.ExtractSelectedEntries("name = *.*", @"gui/maps/icons/map", destination, ExtractExistingFileAction.OverwriteSilently);
			    zip.ExtractSelectedEntries("name = *.*", @"gui/maps/icons/map/stats", destination, ExtractExistingFileAction.OverwriteSilently);
			    zip.ExtractSelectedEntries("name = *.*", @"gui/maps/icons/vehicle", destination, ExtractExistingFileAction.OverwriteSilently);
			    zip.ExtractSelectedEntries("name = *.*", @"gui/maps/icons/achievement", destination, ExtractExistingFileAction.OverwriteSilently);
			    zip.ExtractSelectedEntries("name = *.*", @"gui/maps/icons/artefact", destination, ExtractExistingFileAction.OverwriteSilently);
				zip.ExtractSelectedEntries("name = *.*", @"gui/maps/icons/shell", destination, ExtractExistingFileAction.OverwriteSilently);
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
		        //ImportAchievementsXml(client);

				Console.WriteLine("Copy maps");
				//ImportMapsXml(client);
				//ImportMapMinimapsXml(client, prevVer);

				Console.WriteLine("Copy tanks components");
				//ImportShellsXml(client);
				//ImportArtefactXml(client);

		        Console.WriteLine("Copy tanks definitions");
		        //ImportTanksXml(client);
		        //ImportActionTanksXml();
		        //CheckTankImages();

				//destination = Path.Combine(Environment.CurrentDirectory, @"Output\Patch\Images");

				//string filepath = Path.Combine(clientPath, @"res\packages\gui.pkg");
				//using (var zip = new ZipFile(filepath, Encoding.GetEncoding((int)CodePage.CyrillicDOS)))
				//{
				//    var achievement = @"gui/maps/icons/achievement";
				//    zip.ExtractSelectedEntries("name = *.*", achievement, destination, ExtractExistingFileAction.OverwriteSilently);

				//    var achievementsDestinationPath = Path.Combine(destination, "achievement");

				//    if (Directory.Exists(achievementsDestinationPath))
				//    {
				//        Directory.Delete(achievementsDestinationPath, true);
				//    }

				//    Directory.Move(Path.Combine(destination, achievement), achievementsDestinationPath);

				//    var vehicle = @"gui/maps/icons/vehicle";
				//    zip.ExtractSelectedEntries("name = *.*", vehicle, destination, ExtractExistingFileAction.OverwriteSilently);
				//    var vehiclesPath = Path.Combine(destination, @"vehicle");

				//    if (Directory.Exists(vehiclesPath))
				//    {
				//        Directory.Delete(vehiclesPath, true);
				//    }

				//    Directory.Move(Path.Combine(destination, vehicle), vehiclesPath);

				//    var files = Directory.GetFiles(vehiclesPath);

				//    foreach (var file in files)
				//    {
				//        FileInfo info = new FileInfo(file);
				//        var destFileName = file.Replace("-", "_");
				//        if (!File.Exists(destFileName))
				//        {
				//            info.MoveTo(destFileName);
				//        }
				//    }
				//}
				prevVer = client.PatchVer;
	        }


		}

	    public void GenerateStringResources(ClientInfo client)
	    {
		    var destination = Path.Combine(Environment.CurrentDirectory, $@"Output\Strings");
		    if (!Directory.Exists(destination))
			    Directory.CreateDirectory(destination);

		    foreach (var src in Directory.GetFiles(
			    Path.Combine(Environment.CurrentDirectory, $@"Output\{client.PatchVer}\Resources\Text"), "*.resx"))
		    {
			    string locale = "";
			    string fileName = Path.GetFileNameWithoutExtension(src);

			    if (fileName.Contains(".ru"))
			    {
				    locale = ".ru";
				    fileName = fileName.Replace(".ru", "");
			    }
			    var dict = new Dictionary<string, string>();

			    var resultName = $"{fileName}{locale}.resx";
			    var nmlist = resultName.ToCharArray();
			    var capit = true;
			    resultName = "";
			    foreach (var ch in nmlist)
			    {
				    if (ch == '_')
				    {
					    capit = true;
					    continue;
				    }
				    var c = ch.ToString();
				    if (capit)
					    c = c.ToUpper();
				    resultName += c;
				    capit = false;
			    }
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
			var destination = Path.Combine(Environment.CurrentDirectory, $@"Output\{client.PatchVer}");
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
				destination = Path.Combine(Environment.CurrentDirectory, $@"Output\{client.PatchVer}\text\{lc.locale}");
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

					var res = Path.Combine(Environment.CurrentDirectory, $@"Output\{client.PatchVer}\Resources\Text\{lc.locale}");
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
						proc.StartInfo.Arguments = Path.Combine(Environment.CurrentDirectory, $@"Output\{client.PatchVer}") + " " +
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

		//[Flags]
	 //   private enum DiffResult
	 //   {
		//    Equal = 0,
		//	ElementValueDiff = 1,
		//    SecondaryMore = 2,
		//	SecondaryMore = 2,
		//	SecondaryLess = 4
	 //   }

	 //   private static DiffResult XMLDiff(XElement primary, XElement secondary, bool compareCount = true, bool compareOnlyValues = false)
	 //   {
		//    var result = DiffResult.Equal;

		//	if (primary.HasElements)
		//    {
		//	    if (primary.Elements().Count() > secondary.Elements().Count())
		//	    {
		//		    result = result | DiffResult.SecondaryLess;
		//	    }
		//	    else if (primary.Elements().Count() < secondary.Elements().Count())
		//	    {
		//		    result = result | DiffResult.SecondaryLess;
		//	    }

		//		foreach (var elem in primary.Elements())
		//	    {
		//		    if (secondary.Element(elem.Name.LocalName) == null)
		//			    return false;
		//		    if (!XMLCompare(elem, secondary.Element(elem.Name.LocalName), compareCount, compareOnlyValues))
		//			    return false;
		//	    }
		//    }
		//    else if (!string.Equals(primary.Value.Trim(' ', '\t'), secondary.Value.Trim(' ', '\t'), StringComparison.InvariantCultureIgnoreCase))
		//	    return false;

		//    return true;
	 //   }

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