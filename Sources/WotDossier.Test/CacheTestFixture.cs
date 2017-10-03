using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHibernate.Util;
using NUnit.Framework;
using WotDossier.Applications;
using WotDossier.Applications.BattleModeStrategies;
using WotDossier.Applications.Logic;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Dossier;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Server;
using WotDossier.Domain.Tank;

namespace WotDossier.Test
{
    /// <summary>
    /// Dossier cache load tests
    /// </summary>
    public class CacheTestFixture : TestFixtureBase
    {
        [Test]
        public void CacheFilesTest()
        {
            //trick
            DataProvider.RollbackTransaction();
            DataProvider.CloseSession();

            //reset DB
            DatabaseManager.DeleteDatabase();
            DatabaseManager.InitDatabase();

            Player serverStatistic = new Player();
            serverStatistic.dataField = new PlayerData { account_id = 10800699, nickname = "_rembel__ru", created_at = 1349068892 };

            foreach (Version version in Dictionaries.Instance.Versions)
            {
                string cacheFolder = string.Format(@"\CacheFiles\{0}\", version.ToString(3));

                FileInfo cacheFile = GetCacheFile("_rembel__ru", cacheFolder);

                if (cacheFile != null)
                {
                    List<TankJson> tanks = CacheFileHelper.InternalBinaryCacheToJson(cacheFile);
                    foreach (TankJson tankJson in tanks)
                    {
                        string iconPath = string.Format(@"..\..\..\WotDossier.Resources\Images\Tanks\{0}.png",
                            tankJson.Description.Icon.IconId);
                        Assert.True(File.Exists(iconPath),
                            string.Format("Version: {1}. Can't find icon {0}", tankJson.Description.Icon.IconId, version));
                    }

                    foreach (BattleMode battleMode in Enum.GetValues(typeof (BattleMode)))
                    {
                        StatisticViewStrategyBase strategy = StatisticViewStrategyManager.Get(battleMode,
                            DossierRepository);

                        PlayerEntity player = strategy.UpdatePlayerStatistic(
                            serverStatistic.dataField.account_id, tanks, serverStatistic);

                        var playerStatisticViewModel = strategy.GetPlayerStatistic(player, tanks, serverStatistic);
                        Assert.IsNotNull(playerStatisticViewModel);
                    }
                }
                else
                {
                    Console.WriteLine("Cache file not found: {0}", version);
                }
            }
            
        }

        [Test]
        public void NoobMeterPerformanceRatingAlgorithmTest()
        {
            Version version = Dictionaries.VersionRelease;

            string cacheFolder = string.Format(@"\CacheFiles\{0}\", version.ToString(3));

            FileInfo cacheFile = GetCacheFile("_rembel__ru", cacheFolder);

            if (cacheFile != null)
            {
                List<TankJson> tanks = CacheFileHelper.InternalBinaryCacheToJson(cacheFile);
                var performanceRating = RatingHelper.PerformanceRating(tanks, json => json.A15x15);
                Console.WriteLine(performanceRating);

            }
            
        }

        [Test]
        public void InternalCacheLoaderTest()
        {
            Version version = new Version("0.9.20");

            string cacheFolder = Path.Combine(TestContext.CurrentContext.TestDirectory, $@"CacheFiles\{version.ToString(3)}\");

            FileInfo cacheFile = Directory.GetFiles(cacheFolder, "*.dat").Select(f => new FileInfo(f)).FirstOrDefault();//GetCacheFile("_rembel__ru", cacheFolder);

            CacheFileHelper.InternalBinaryCacheToJson(cacheFile);
        }


        //private static void GenerateStructuresXml()
        //{
        //    var cacheFolder = Path.Combine(TestContext.CurrentContext.TestDirectory, $@"External\structures\");

        //    #region order

        //    var order = new List<(string section, int version, int type)>
        //    {
        //        ("tankdata", 10, 1),
        //        ("structure", 10, 1),
        //        ("series", 20, 1),
        //        ("battle", 20, 1),
        //        ("major", 20, 1),
        //        ("epic", 20, 1),
        //        ("special", 20, 1),
        //        ("company", 20, 1),
        //        ("clan", 20, 1),

        //        ("a15x15", 65, 2),
        //        ("a15x15_2", 65, 2),
        //        ("clan", 65, 2),
        //        ("clan2", 65, 2),
        //        ("company", 65, 2),
        //        ("company2", 65, 2),
        //        ("a7x7", 65, 2),
        //        ("achievements", 65, 2),
        //        ("vehTypeFrags", 65, 2),
        //        ("total", 65, 2),
        //        ("max15x15", 65, 2),
        //        ("max7x7", 65, 2),
        //        ("playerInscriptions", 69, 2),
        //        ("playerEmblems", 69, 2),
        //        ("camouflages", 69, 2),
        //        ("compensation", 69, 2),
        //        ("achievements7x7", 69, 2),
        //        ("historical", 77, 2),
        //        ("maxHistorical", 77, 2),
        //        ("historicalAchievements", 81, 2),
        //        ("fortBattles", 81, 2),
        //        ("maxFortBattles", 81, 2),
        //        ("fortSorties", 81, 2),
        //        ("maxFortSorties", 81, 2),
        //        ("fortAchievements", 81, 2),
        //        ("singleAchievements", 87, 2),
        //        ("clanAchievements", 87, 2),
        //        ("rated7x7", 89, 2),
        //        ("maxRated7x7", 89, 2),
        //        ("globalMapCommon", 92, 2),
        //        ("maxGlobalMapCommon", 92, 2),
        //        ("fallout", 96, 2),
        //        ("maxFallout", 96, 2),
        //        ("falloutAchievements", 96, 2),
        //        ("ranked", 97, 2),
        //        ("maxRanked", 97, 2),
        //        ("rankedSeasons",  97, 2),
        //        ("a30x30", 99, 2),
        //        ("max30x30", 99, 2)
        //    };

        //    #endregion

        //    var doc = new XDocument();
        //    var lst = new XElement("layouts");
        //    foreach (var filename in Directory.GetFiles(cacheFolder, "*.json"))
        //    {
        //        using (var ms = new MemoryStream(File.ReadAllBytes(filename)))
        //        {
        //            using (var re = new StreamReader(ms))
        //            {
        //                var parsedData = new JsonSerializer().Deserialize<JArray>(new JsonTextReader(re));
        //                var version = Convert.ToInt32(Path.GetFileNameWithoutExtension(filename).Replace("structures_", ""));

        //                var dsv = (version < 65) ? 1 : 2;

        //                var structure = parsedData.ToObject<List<DossierStructure>>();
        //                XElement root = new XElement("vehicleDossierLayout", new XAttribute("dossierVersion", dsv), new XAttribute("version", version));
        //                foreach (var item in order.Where(o=>o.type==dsv && o.version <= version))
        //                {
        //                    var section = item.section;
        //                    if (version >= 88 && section == "historicalAchievements")
        //                        section = "uniqueVehAchievement";

        //                    var fi = structure.FirstOrDefault(s => s.category == section);
        //                    if (fi == null)
        //                    {
        //                        continue;
        //                    }

        //                    XElement ctg = null;
        //                    var layoutType = "StaticSize";
        //                    foreach (var str in structure.Where(s => s.category == section).OrderBy(s => s.offset))
        //                    {
        //                        if (ctg == null)
        //                        {
        //                            if (str.category == "structure")
        //                            {
        //                                layoutType = "Dict";
        //                                ctg = new XElement("vehTypeFrags",
        //                                    new XAttribute("order", order.IndexOf(item) * 100),
        //                                    new XAttribute("blockBuilder", layoutType));
        //                                ctg.Add(new XAttribute("keyFormat", "I"));
        //                                ctg.Add(new XAttribute("valueFormat", "H"));
        //                                ctg.Add(new XElement("count", new XAttribute("length", str.length),
        //                                    new XAttribute("offset", str.offset)));
        //                                continue;
        //                            }
        //                            if (str.category == "playerInscriptions" || str.category == "playerEmblems" ||
        //                                str.category == "camouflages")
        //                                layoutType = "List";
        //                            else if (str.category == "vehTypeFrags" || str.category == "rankedSeasons")
        //                                layoutType = "Dict";
        //                            else if (str.category == "uniqueVehAchievement")
        //                                layoutType = "Binary";

        //                            ctg = new XElement(str.category,
        //                                new XAttribute("order", order.IndexOf(item) * 100),
        //                                new XAttribute("blockBuilder", layoutType));

        //                            if (layoutType == "Binary")
        //                            {
        //                                ctg.Add(new XAttribute("itemName", "uniqueAchievements"));
        //                                continue;
        //                            }

        //                            if (layoutType == "List")
        //                            {
        //                                var nm = "";
        //                                if (str.category == "playerInscriptions")
        //                                    nm = "inscriptions";
        //                                else if (str.category == "playerEmblems")
        //                                    nm = "emblems";
        //                                   else if(str.category == "camouflages")
        //                                        nm = "camouflages";
        //                                ctg.Add(new XAttribute("itemName", nm));
        //                                ctg.Add(new XAttribute("itemFormat", "H"));
        //                                continue;
        //                            }
        //                            if (layoutType == "Dict")
        //                            {
        //                                if (str.category == "vehTypeFrags")
        //                                {
        //                                    ctg.Add(new XAttribute("keyFormat", "I"));
        //                                    ctg.Add(new XAttribute("valueFormat", "H"));
        //                                }
        //                                else if (str.category == "rankedSeasons")
        //                                {
        //                                    ctg.Add(new XAttribute("keyFormat", "II"));
        //                                    ctg.Add(new XAttribute("valueFormat", "BB"));
        //                                }
        //                                continue;
        //                            }
        //                        }
        //                        if (layoutType != "StaticSize")
        //                            continue;
                                
        //                        var elem = new XElement(str.name, new XAttribute("length", str.length), new XAttribute("offset", str.offset));
                                
        //                        ctg.Add(elem);

        //                    }
        //                    root.Add(ctg);
        //                }
        //                lst.Add(root);

                        
        //                //structuresCache.Add(version, structure);
        //            }
        //        }
        //    }
        //    doc.Add(lst);
        //    doc.Save(Path.Combine(cacheFolder, "vehicleDossierLayout.xml"));
            
        //}


        [Test]
        public void CacheFileTest()
        {
            Version version = new Version("0.9.13.0");
            const BattleMode battleMode = BattleMode.RandomCompany;

            //trick
            DataProvider.RollbackTransaction();
            DataProvider.CloseSession();

            Player serverStatistic = new Player();
            serverStatistic.dataField = new PlayerData
            {
                account_id = 10800699,
                nickname = "_rembel__ru",
                created_at = 1349068892
            };
            //reset DB
            DatabaseManager.DeleteDatabase();
            DatabaseManager.InitDatabase();

            string cacheFolder = string.Format(@"\CacheFiles\{0}\", version.ToString(3));

            FileInfo cacheFile = GetCacheFile("_rembel__ru", cacheFolder);

            List<TankJson> tanks = CacheFileHelper.InternalBinaryCacheToJson(cacheFile);
            foreach (TankJson tankJson in tanks)
            {
                string iconPath = string.Format(@"..\..\..\WotDossier.Resources\Images\Tanks\{0}.png", tankJson.Description.Icon.IconId);
                Assert.True(File.Exists(iconPath), string.Format("Version: {1}. Can't find icon {0}", tankJson.Description.Icon.IconId, version));
            }
            StatisticViewStrategyBase strategy = StatisticViewStrategyManager.Get(battleMode, DossierRepository);

            PlayerEntity player = strategy.UpdatePlayerStatistic(serverStatistic.dataField.account_id, tanks,
                serverStatistic);

            var playerStatisticViewModel = strategy.GetPlayerStatistic(player, tanks, serverStatistic);
            Assert.IsNotNull(playerStatisticViewModel);
        }

        /// <summary>
        /// Gets the cache file.
        /// </summary>
        /// <param name="playerId">The player id.</param>
        /// <param name="folder">The folder.</param>
        /// <returns>
        /// null if there is no any dossier cache file for specified player
        /// </returns>
        public static FileInfo GetCacheFile(string playerId, string folder)
        {
            FileInfo cacheFile = null;

            string path = TestContext.CurrentContext.TestDirectory + folder;

            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path, "*.dat");

                if (!files.Any())
                {
                    return null;
                }

                foreach (string file in files)
                {
                    FileInfo info = new FileInfo(file);

                    if (CacheFileHelper.GetPlayerName(info)
                        .Equals(playerId, StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (cacheFile == null)
                        {
                            cacheFile = info;
                        }
                        else if (cacheFile.LastWriteTime < info.LastWriteTime)
                        {
                            cacheFile = info;
                        }
                    }
                }
            }
            return cacheFile;
        }
    }
}
