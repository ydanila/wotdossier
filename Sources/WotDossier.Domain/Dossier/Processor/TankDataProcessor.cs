using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WotDossier.Domain.Tank;
using WotDossier.Common;
using WotDossier.Domain.Interfaces;

namespace WotDossier.Domain.Dossier.Processor
{
    public static class TankDataProcessor
    {
        #region [Static data]

        private static Dictionary<int, Func<DossierTankData, TankJson>> processors =
            new Dictionary<int, Func<DossierTankData, TankJson>>
            {
                {10, Process10},
                {20, Process20},
                {22, Process22},
                {24, Process24},
                {26, Process26},
                {27, Process27},
                {29, Process29},
                {65, Process65},
                {69, Process69},
                {77, Process77},
                {81, Process81},
                {85, Process85},
                {87, Process87},
                {88, Process88},
                {92, Process92},
                {94, Process94},
                {95, Process95},
                {97, Process97},
                {99, Process99},

            };
        public static Func<DossierTankData, TankJson> GetProcessor(int version)
        {
            var vl = processors.Keys.OrderByDescending(x => x).FirstOrDefault(key => key <= version);
            if (vl > 0) return processors[vl];
            throw new NotImplementedException();
        }

        private static int GetValue(this IReadOnlyDictionary<string, int> dict, string key)
        {
            if (dict == null) return 0;
            return dict.TryGetValue(key, out int value) ? value : 0;
        }

        private static Dictionary<string, int> GetStaticSizeBlock(this DossierTankData dossier, string key)
        {
            return dossier.StaticSize.TryGetValue(key, out Dictionary<string, int> value) ? value : null;
        }

        private static IEnumerable<FragsJson> ProcessFrags(Dictionary<int, int> list, int tankUniqueId)
        {
            return list.Select(
                x =>
                {
                    var tank = Dictionaries.Instance.GetTankDescription(x.Key);

                    return new FragsJson
                    {
                        CountryId = tank.CountryId,
                        TankId = tank.TankId,
                        Icon = tank.Icon,
                        TankUniqueId = tank.UniqueId,
                        Count = x.Value,
                        Type = tank.Type,
                        Tier = tank.Tier,
                        KilledByTankUniqueId = tankUniqueId,
                        Tank = tank.Title
                    };
                }).ToList();
        }

        #endregion

        private static TankJson Process10(DossierTankData tankData)
        {
            var tank = Dictionaries.Instance.GetTankDescription(tankData.compDescr);
            var result = new TankJson()
            {
                Description = tank
            };

            #region Common
            result.Common = new CommonJson()
            {
                basedonversion = tankData.basedonversion,
                compactDescr = tankData.compDescr,
                countryid = tank.CountryId,
                creationTime = 1356998400,
                creationTimeR = WotDossier.Common.Utils.UnixDateToDateTime(1356998400),
                frags = tankData.GetStaticSizeBlock("tankdata").GetValue("frags"),
                //frags_compare = tankData.frags_compare,
                lastBattleTime = tankData.GetStaticSizeBlock("tankdata").GetValue("lastBattleTime"),
                lastBattleTimeR = WotDossier.Common.Utils.UnixDateToDateTime(tankData.GetStaticSizeBlock("tankdata").GetValue("lastBattleTime")),
                premium = tank.Premium,
                tankid = tank.TankId,
                tanktitle = tank.Title,
                tier = tank.Tier,
                type = tank.Type,
                updated = tankData.updated,
                updatedR = WotDossier.Common.Utils.UnixDateToDateTime(tankData.updated),
                battleLifeTime = tankData.GetStaticSizeBlock("tankdata").GetValue("battleLifeTime"),
                treesCut = tankData.GetStaticSizeBlock("tankdata").GetValue("treesCut"),
            };
            #endregion

            #region 15x15
            result.A15x15 = new StatisticJson
            {
                battlesCount = tankData.GetStaticSizeBlock("tankdata").GetValue("battlesCount"),
                battlesCountBefore8_8 = tankData.GetStaticSizeBlock("tankdata").GetValue("battlesCount"),
                capturePoints = tankData.GetStaticSizeBlock("tankdata").GetValue("capturePoints"),
                damageDealt = tankData.GetStaticSizeBlock("tankdata").GetValue("damageDealt"),
                damageReceived = tankData.GetStaticSizeBlock("tankdata").GetValue("damageReceived"),
                droppedCapturePoints = tankData.GetStaticSizeBlock("tankdata").GetValue("droppedCapturePoints"),
                frags = tankData.GetStaticSizeBlock("tankdata").GetValue("frags"),
                frags8p = tankData.GetStaticSizeBlock("tankdata").GetValue("frags8p"),
                hits = tankData.GetStaticSizeBlock("tankdata").GetValue("hits"),
                losses = tankData.GetStaticSizeBlock("tankdata").GetValue("losses"),
                shots = tankData.GetStaticSizeBlock("tankdata").GetValue("shots"),
                spotted = tankData.GetStaticSizeBlock("tankdata").GetValue("spotted"),
                survivedBattles = tankData.GetStaticSizeBlock("tankdata").GetValue("survivedBattles"),
                winAndSurvived = tankData.GetStaticSizeBlock("tankdata").GetValue("winAndSurvived"),
                wins = tankData.GetStaticSizeBlock("tankdata").GetValue("wins"),
                xp = tankData.GetStaticSizeBlock("tankdata").GetValue("xp"),
                xpBefore8_8 = tankData.GetStaticSizeBlock("tankdata").GetValue("xp"),
                maxFrags = tankData.GetStaticSizeBlock("tankdata").GetValue("maxFrags"),
                maxXP = tankData.GetStaticSizeBlock("tankdata").GetValue("maxXP")
            };
            #endregion

            #region Achievements

            result.Achievements = new AchievementsJson
            {
                FragsBeast = tankData.GetStaticSizeBlock("tankdata").GetValue("fragsBeast")
            };

            #endregion

            result.Frags = tankData.Dict.TryGetValue("vehTypeFrags", out Dictionary<object, object> frgs)
                ? ProcessFrags(frgs.ToDictionary(p => Convert.ToInt32(p.Key), p => Convert.ToInt32(p.Value)),
                    tank.UniqueId)
                : new List<FragsJson>();
            return result;
        }
        
        private static TankJson Process20(DossierTankData tankData)
        {
            var result = Process10(tankData);

            #region Series
            result.Achievements.SharpshooterProgress = tankData.GetStaticSizeBlock("series").GetValue("sniperSeries");
            result.Achievements.SharpshooterLongest = tankData.GetStaticSizeBlock("series").GetValue("maxSniperSeries");
            result.Achievements.InvincibleProgress = tankData.GetStaticSizeBlock("series").GetValue("invincibleSeries");
            result.Achievements.InvincibleLongest = tankData.GetStaticSizeBlock("series").GetValue("maxInvincibleSeries");
            result.Achievements.SurvivorProgress = tankData.GetStaticSizeBlock("series").GetValue("diehardSeries");
            result.Achievements.SurvivorLongest = tankData.GetStaticSizeBlock("series").GetValue("maxDiehardSeries");
            result.Achievements.ReaperProgress = tankData.GetStaticSizeBlock("series").GetValue("killingSeries");
            result.Achievements.ReaperLongest = tankData.GetStaticSizeBlock("series").GetValue("maxKillingSeries");
            result.Achievements.MasterGunnerProgress = tankData.GetStaticSizeBlock("series").GetValue("piercingSeries");
            result.Achievements.MasterGunnerLongest = tankData.GetStaticSizeBlock("series").GetValue("maxPiercingSeries");
            #endregion

            #region Battle
            result.Achievements.BattleHero = tankData.GetStaticSizeBlock("battle").GetValue("battleHeroes");
            result.Achievements.Warrior = tankData.GetStaticSizeBlock("battle").GetValue("warrior");
            result.Achievements.Invader = tankData.GetStaticSizeBlock("battle").GetValue("invader");
            result.Achievements.Sniper = tankData.GetStaticSizeBlock("battle").GetValue("sniper");
            result.Achievements.Defender = tankData.GetStaticSizeBlock("battle").GetValue("defender");
            result.Achievements.SteelWall = tankData.GetStaticSizeBlock("battle").GetValue("steelwall");
            result.Achievements.Confederate = tankData.GetStaticSizeBlock("battle").GetValue("confederate");
            result.Achievements.Scout = tankData.GetStaticSizeBlock("battle").GetValue("scout");
            result.Achievements.PatrolDuty = tankData.GetStaticSizeBlock("battle").GetValue("evileye");
            #endregion

            #region Major
            result.Achievements.Kay = tankData.GetStaticSizeBlock("major").GetValue("medalKay");
            result.Achievements.Carius = tankData.GetStaticSizeBlock("major").GetValue("medalCarius");
            result.Achievements.Knispel = tankData.GetStaticSizeBlock("major").GetValue("medalKnispel");
            result.Achievements.Poppel = tankData.GetStaticSizeBlock("major").GetValue("medalPoppel");
            result.Achievements.Abrams = tankData.GetStaticSizeBlock("major").GetValue("medalAbrams");
            result.Achievements.Leclerk = tankData.GetStaticSizeBlock("major").GetValue("medalLeClerc");
            result.Achievements.Lavrinenko = tankData.GetStaticSizeBlock("major").GetValue("medalLavrinenko");
            result.Achievements.Ekins = tankData.GetStaticSizeBlock("major").GetValue("medalEkins");
            #endregion

            #region Epic
            result.Achievements.Boelter = tankData.GetStaticSizeBlock("epic").GetValue("medalWittmann");
            result.Achievements.Orlik = tankData.GetStaticSizeBlock("epic").GetValue("medalOrlik");
            result.Achievements.Oskin = tankData.GetStaticSizeBlock("epic").GetValue("medalOskin");
            result.Achievements.Halonen = tankData.GetStaticSizeBlock("epic").GetValue("medalHalonen");
            result.Achievements.Burda = tankData.GetStaticSizeBlock("epic").GetValue("medalBurda");
            result.Achievements.Billotte = tankData.GetStaticSizeBlock("epic").GetValue("medalBillotte");
            result.Achievements.Kolobanov = tankData.GetStaticSizeBlock("epic").GetValue("medalKolobanov");
            result.Achievements.Fadin = tankData.GetStaticSizeBlock("epic").GetValue("medalFadin");
            result.Achievements.DeLanglade = tankData.GetStaticSizeBlock("epic").GetValue("medalDeLanglade");
            result.Achievements.TamadaYoshio = tankData.GetStaticSizeBlock("epic").GetValue("medalTamadaYoshio");
            //result.Achievements.XX = tankData.GetStaticSizeBlock("epic").GetValue("medalErohin");
            //result.Achievements.XX = tankData.GetStaticSizeBlock("epic").GetValue("medalHoroshilov");
            //result.Achievements.XX = tankData.GetStaticSizeBlock("epic").GetValue("medalLister");
            #endregion

            #region Special
            result.Achievements.HeroesOfRassenay = tankData.GetStaticSizeBlock("special").GetValue("heroesOfRassenay");
            result.Achievements.Hunter = tankData.GetStaticSizeBlock("special").GetValue("beasthunter");
            result.Achievements.MouseTrap = tankData.GetStaticSizeBlock("special").GetValue("mousebane");
            result.Achievements.TankExpertStrg = tankData.GetStaticSizeBlock("special").GetValue("tankExpert");
            result.Achievements.Sharpshooter = tankData.GetStaticSizeBlock("special").GetValue("sniperspecial");
            result.Achievements.Invincible = tankData.GetStaticSizeBlock("special").GetValue("invincible");
            result.Achievements.Survivor = tankData.GetStaticSizeBlock("special").GetValue("diehard");
            result.Achievements.Raider = tankData.GetStaticSizeBlock("special").GetValue("raider");
            result.Achievements.Reaper = tankData.GetStaticSizeBlock("special").GetValue("handOfDeath");
            result.Achievements.MasterGunner = tankData.GetStaticSizeBlock("special").GetValue("armorPiercer");
            result.Achievements.Kamikaze = tankData.GetStaticSizeBlock("special").GetValue("kamikaze");
            result.Achievements.Lumberjack = tankData.GetStaticSizeBlock("special").GetValue("lumberjack");
            result.Achievements.MarkOfMastery = tankData.GetStaticSizeBlock("special").GetValue("markOfMastery");
            #endregion

            #region Company
            result.Company = new StatisticJson
            {
                xp = tankData.GetStaticSizeBlock("company").GetValue("xp"),
                battlesCount = tankData.GetStaticSizeBlock("company").GetValue("battlesCount"),
                wins = tankData.GetStaticSizeBlock("company").GetValue("wins"),
                losses = tankData.GetStaticSizeBlock("company").GetValue("losses"),
                survivedBattles = tankData.GetStaticSizeBlock("company").GetValue("survivedBattles"),
                frags = tankData.GetStaticSizeBlock("company").GetValue("frags"),
                shots = tankData.GetStaticSizeBlock("company").GetValue("shots"),
                hits = tankData.GetStaticSizeBlock("company").GetValue("hits"),
                spotted = tankData.GetStaticSizeBlock("company").GetValue("spotted"),
                damageDealt = tankData.GetStaticSizeBlock("company").GetValue("damageDealt"),
                damageReceived = tankData.GetStaticSizeBlock("company").GetValue("damageReceived"),
                capturePoints = tankData.GetStaticSizeBlock("company").GetValue("capturePoints"),
                droppedCapturePoints = tankData.GetStaticSizeBlock("company").GetValue("droppedCapturePoints"),
            };
            #endregion

            #region Clan
            result.Clan = new StatisticJson
            {
                xp = tankData.GetStaticSizeBlock("clan").GetValue("xp"),
                battlesCount = tankData.GetStaticSizeBlock("clan").GetValue("battlesCount"),
                wins = tankData.GetStaticSizeBlock("clan").GetValue("wins"),
                losses = tankData.GetStaticSizeBlock("clan").GetValue("losses"),
                survivedBattles = tankData.GetStaticSizeBlock("clan").GetValue("survivedBattles"),
                frags = tankData.GetStaticSizeBlock("clan").GetValue("frags"),
                shots = tankData.GetStaticSizeBlock("clan").GetValue("shots"),
                hits = tankData.GetStaticSizeBlock("clan").GetValue("hits"),
                spotted = tankData.GetStaticSizeBlock("clan").GetValue("spotted"),
                damageDealt = tankData.GetStaticSizeBlock("clan").GetValue("damageDealt"),
                damageReceived = tankData.GetStaticSizeBlock("clan").GetValue("damageReceived"),
                capturePoints = tankData.GetStaticSizeBlock("clan").GetValue("capturePoints"),
                droppedCapturePoints = tankData.GetStaticSizeBlock("clan").GetValue("droppedCapturePoints"),
            };
            #endregion

            return result;
        }

        private static TankJson Process22(DossierTankData tankData)
        {
            var result = Process20(tankData);

            result.Achievements.FragsSinai = tankData.GetStaticSizeBlock("battle").GetValue("fragsSinai");
            result.Achievements.Confederate = tankData.GetStaticSizeBlock("battle").GetValue("supporter");

            result.Achievements.RadleyWalters = tankData.GetStaticSizeBlock("epic").GetValue("medalRadleyWalters");
            result.Achievements.BrunoPietro = tankData.GetStaticSizeBlock("epic").GetValue("medalBrunoPietro");
            result.Achievements.Tarczay = tankData.GetStaticSizeBlock("epic").GetValue("medalTarczay");
            result.Achievements.Pascucci = tankData.GetStaticSizeBlock("epic").GetValue("medalPascucci");
            result.Achievements.Dumitru = tankData.GetStaticSizeBlock("epic").GetValue("medalDumitru");
            result.Achievements.Lehvaslaiho = tankData.GetStaticSizeBlock("epic").GetValue("medalLehvaslaiho");
            result.Achievements.Nikolas = tankData.GetStaticSizeBlock("epic").GetValue("medalNikolas");
            result.Achievements.LafayettePool = tankData.GetStaticSizeBlock("epic").GetValue("medalLafayettePool");

            result.Achievements.Sinai = tankData.GetStaticSizeBlock("special").GetValue("sinai");
            result.Achievements.Sharpshooter = tankData.GetStaticSizeBlock("special").GetValue("titleSniper");
            result.Achievements.TankExpertStrg = tankData.GetStaticSizeBlock("special").GetValue("tankExpertStrg");
            return result;
        }

        private static TankJson Process24(DossierTankData tankData)
        {
            var result = Process22(tankData);

            result.Achievements.BrothersInArms = tankData.GetStaticSizeBlock("epic").GetValue("medalBrothersInArms");
            result.Achievements.CrucialContribution = tankData.GetStaticSizeBlock("epic").GetValue("medalCrucialContribution");
            return result;
        }

        private static TankJson Process26(DossierTankData tankData)
        {
            var result = Process24(tankData);

            result.Achievements.Bombardier = tankData.GetStaticSizeBlock("special").GetValue("bombardier");
            result.Achievements.Huntsman = tankData.GetStaticSizeBlock("special").GetValue("huntsman");
            result.Achievements.Alaric = tankData.GetStaticSizeBlock("special").GetValue("alaric");
            result.Achievements.Sturdy = tankData.GetStaticSizeBlock("special").GetValue("sturdy");
            result.Achievements.IronMan = tankData.GetStaticSizeBlock("special").GetValue("ironMan");
            result.Achievements.LuckyDevil = tankData.GetStaticSizeBlock("special").GetValue("luckyDevil");
            result.Achievements.PattonValley = tankData.GetStaticSizeBlock("special").GetValue("pattonValley");
            result.Achievements.FragsPatton = tankData.GetStaticSizeBlock("special").GetValue("fragsPatton");

            return result;
        }

        private static TankJson Process27(DossierTankData tankData)
        {
            var result = Process26(tankData);
            result.Common.creationTime = tankData.GetStaticSizeBlock("tankdata").GetValue("creationTime");
            result.Common.creationTimeR = WotDossier.Common.Utils.UnixDateToDateTime(tankData.GetStaticSizeBlock("tankdata").GetValue("creationTime"));
            return result;
        }
        private static TankJson Process29(DossierTankData tankData)
        {
            var result = Process27(tankData);
            result.Common.mileage = tankData.GetStaticSizeBlock("tankdata").GetValue("mileage");

            result.A15x15.originalXP = tankData.GetStaticSizeBlock("tankdata").GetValue("originalXP");
            result.A15x15.damageAssistedTrack = tankData.GetStaticSizeBlock("tankdata").GetValue("damageAssistedTrack");
            result.A15x15.damageAssistedRadio = tankData.GetStaticSizeBlock("tankdata").GetValue("damageAssistedRadio");
            result.A15x15.shotsReceived = tankData.GetStaticSizeBlock("tankdata").GetValue("shotsReceived");
            result.A15x15.noDamageShotsReceived = tankData.GetStaticSizeBlock("tankdata").GetValue("noDamageShotsReceived");
            result.A15x15.piercedReceived = tankData.GetStaticSizeBlock("tankdata").GetValue("piercedReceived");
            result.A15x15.heHitsReceived = tankData.GetStaticSizeBlock("tankdata").GetValue("heHitsReceived");
            result.A15x15.he_hits = tankData.GetStaticSizeBlock("tankdata").GetValue("he_hits");
            result.A15x15.pierced = tankData.GetStaticSizeBlock("tankdata").GetValue("pierced");
            result.A15x15.xpBefore8_8 = tankData.GetStaticSizeBlock("tankdata").GetValue("xpBefore8_8");
            result.A15x15.battlesCountBefore8_8 = tankData.GetStaticSizeBlock("tankdata").GetValue("battlesCountBefore8_8");
            return result;
        }
        private static TankJson Process65(DossierTankData tankData)
        {
            var tank = Dictionaries.Instance.GetTankDescription(tankData.compDescr);
            var result = new TankJson()
            {
                Description = tank
            };
            #region Common
            result.Common = new CommonJson()
            {
                basedonversion = tankData.basedonversion,
                compactDescr = tankData.compDescr,
                countryid = tank.CountryId,
                creationTime = tankData.GetStaticSizeBlock("total").GetValue("creationTime"),
                creationTimeR = WotDossier.Common.Utils.UnixDateToDateTime(tankData.GetStaticSizeBlock("total").GetValue("creationTime")),
                frags = tankData.GetStaticSizeBlock("tankdata").GetValue("frags"),
                lastBattleTime = tankData.GetStaticSizeBlock("total").GetValue("lastBattleTime"),
                lastBattleTimeR = WotDossier.Common.Utils.UnixDateToDateTime(tankData.GetStaticSizeBlock("total").GetValue("lastBattleTime")),
                premium = tank.Premium,
                tankid = tank.TankId,
                tanktitle = tank.Title,
                tier = tank.Tier,
                type = tank.Type,
                updated = tankData.updated,
                updatedR = WotDossier.Common.Utils.UnixDateToDateTime(tankData.updated),
                battleLifeTime = tankData.GetStaticSizeBlock("total").GetValue("battleLifeTime"),
                treesCut = tankData.GetStaticSizeBlock("total").GetValue("treesCut"),
                mileage = tankData.GetStaticSizeBlock("total").GetValue("mileage"),
            };
            #endregion

            #region 15x15
            result.A15x15 = new StatisticJson
            {
                xp = tankData.GetStaticSizeBlock("a15x15").GetValue("xp"),
                battlesCount = tankData.GetStaticSizeBlock("a15x15").GetValue("battlesCount"),
                wins = tankData.GetStaticSizeBlock("a15x15").GetValue("wins"),
                losses = tankData.GetStaticSizeBlock("a15x15").GetValue("losses"),
                survivedBattles = tankData.GetStaticSizeBlock("a15x15").GetValue("survivedBattles"),
                frags = tankData.GetStaticSizeBlock("a15x15").GetValue("frags"),
                shots = tankData.GetStaticSizeBlock("a15x15").GetValue("shots"),
                hits = tankData.GetStaticSizeBlock("a15x15").GetValue("hits"),
                spotted = tankData.GetStaticSizeBlock("a15x15").GetValue("spotted"),
                damageDealt = tankData.GetStaticSizeBlock("a15x15").GetValue("damageDealt"),
                damageReceived = tankData.GetStaticSizeBlock("a15x15").GetValue("damageReceived"),
                capturePoints = tankData.GetStaticSizeBlock("a15x15").GetValue("capturePoints"),
                droppedCapturePoints = tankData.GetStaticSizeBlock("a15x15").GetValue("droppedCapturePoints"),
                xpBefore8_8 = tankData.GetStaticSizeBlock("a15x15").GetValue("xpBefore8_8"),
                battlesCountBefore8_8 = tankData.GetStaticSizeBlock("a15x15").GetValue("battlesCountBefore8_8"),
                winAndSurvived = tankData.GetStaticSizeBlock("a15x15").GetValue("winAndSurvived"),
                frags8p = tankData.GetStaticSizeBlock("a15x15").GetValue("frags8p"),


                originalXP = tankData.GetStaticSizeBlock("a15x15_2").GetValue("originalXP"),
                damageAssistedTrack = tankData.GetStaticSizeBlock("a15x15_2").GetValue("damageAssistedTrack"),
                damageAssistedRadio = tankData.GetStaticSizeBlock("a15x15_2").GetValue("damageAssistedRadio"),
                shotsReceived = tankData.GetStaticSizeBlock("a15x15_2").GetValue("shotsReceived"),
                noDamageShotsReceived = tankData.GetStaticSizeBlock("a15x15_2").GetValue("noDamageShotsReceived"),
                piercedReceived = tankData.GetStaticSizeBlock("a15x15_2").GetValue("piercedReceived"),
                heHitsReceived = tankData.GetStaticSizeBlock("a15x15_2").GetValue("heHitsReceived"),
                he_hits = tankData.GetStaticSizeBlock("a15x15_2").GetValue("he_hits"),
                pierced = tankData.GetStaticSizeBlock("a15x15_2").GetValue("pierced"),

                maxXP = tankData.GetStaticSizeBlock("max15x15").GetValue("maxXP"),
                maxFrags = tankData.GetStaticSizeBlock("max15x15").GetValue("maxFrags"),
                maxDamage = tankData.GetStaticSizeBlock("max15x15").GetValue("maxDamage"),

            };
            #endregion

            #region Clan
            result.Clan = new StatisticJson
            {
                xp = tankData.GetStaticSizeBlock("clan").GetValue("xp"),
                battlesCount = tankData.GetStaticSizeBlock("clan").GetValue("battlesCount"),
                wins = tankData.GetStaticSizeBlock("clan").GetValue("wins"),
                losses = tankData.GetStaticSizeBlock("clan").GetValue("losses"),
                survivedBattles = tankData.GetStaticSizeBlock("clan").GetValue("survivedBattles"),
                frags = tankData.GetStaticSizeBlock("clan").GetValue("frags"),
                shots = tankData.GetStaticSizeBlock("clan").GetValue("shots"),
                hits = tankData.GetStaticSizeBlock("clan").GetValue("hits"),
                spotted = tankData.GetStaticSizeBlock("clan").GetValue("spotted"),
                damageDealt = tankData.GetStaticSizeBlock("clan").GetValue("damageDealt"),
                damageReceived = tankData.GetStaticSizeBlock("clan").GetValue("damageReceived"),
                capturePoints = tankData.GetStaticSizeBlock("clan").GetValue("capturePoints"),
                droppedCapturePoints = tankData.GetStaticSizeBlock("clan").GetValue("droppedCapturePoints"),
                //Не понятно.... в clan и company 8_9!!!!!
                xpBefore8_8 = tankData.GetStaticSizeBlock("clan").GetValue("xpBefore8_9"),
                battlesCountBefore8_8 = tankData.GetStaticSizeBlock("clan").GetValue("battlesCountBefore8_9"),

                originalXP = tankData.GetStaticSizeBlock("clan2").GetValue("originalXP"),
                damageAssistedTrack = tankData.GetStaticSizeBlock("clan2").GetValue("damageAssistedTrack"),
                damageAssistedRadio = tankData.GetStaticSizeBlock("clan2").GetValue("damageAssistedRadio"),
                shotsReceived = tankData.GetStaticSizeBlock("clan2").GetValue("shotsReceived"),
                noDamageShotsReceived = tankData.GetStaticSizeBlock("clan2").GetValue("noDamageShotsReceived"),
                piercedReceived = tankData.GetStaticSizeBlock("clan2").GetValue("piercedReceived"),
                heHitsReceived = tankData.GetStaticSizeBlock("clan2").GetValue("heHitsReceived"),
                he_hits = tankData.GetStaticSizeBlock("clan2").GetValue("he_hits"),
                pierced = tankData.GetStaticSizeBlock("clan2").GetValue("pierced"),
            };
            #endregion

            #region Company
            result.Company = new StatisticJson
            {
                xp = tankData.GetStaticSizeBlock("company").GetValue("xp"),
                battlesCount = tankData.GetStaticSizeBlock("company").GetValue("battlesCount"),
                wins = tankData.GetStaticSizeBlock("company").GetValue("wins"),
                losses = tankData.GetStaticSizeBlock("company").GetValue("losses"),
                survivedBattles = tankData.GetStaticSizeBlock("company").GetValue("survivedBattles"),
                frags = tankData.GetStaticSizeBlock("company").GetValue("frags"),
                shots = tankData.GetStaticSizeBlock("company").GetValue("shots"),
                hits = tankData.GetStaticSizeBlock("company").GetValue("hits"),
                spotted = tankData.GetStaticSizeBlock("company").GetValue("spotted"),
                damageDealt = tankData.GetStaticSizeBlock("company").GetValue("damageDealt"),
                damageReceived = tankData.GetStaticSizeBlock("company").GetValue("damageReceived"),
                capturePoints = tankData.GetStaticSizeBlock("company").GetValue("capturePoints"),
                droppedCapturePoints = tankData.GetStaticSizeBlock("company").GetValue("droppedCapturePoints"),
                //Не понятно.... в clan и company 8_9!!!!!
                xpBefore8_8 = tankData.GetStaticSizeBlock("company").GetValue("xpBefore8_9"),
                battlesCountBefore8_8 = tankData.GetStaticSizeBlock("company").GetValue("battlesCountBefore8_9"),

                originalXP = tankData.GetStaticSizeBlock("company2").GetValue("originalXP"),
                damageAssistedTrack = tankData.GetStaticSizeBlock("company2").GetValue("damageAssistedTrack"),
                damageAssistedRadio = tankData.GetStaticSizeBlock("company2").GetValue("damageAssistedRadio"),
                shotsReceived = tankData.GetStaticSizeBlock("company2").GetValue("shotsReceived"),
                noDamageShotsReceived = tankData.GetStaticSizeBlock("company2").GetValue("noDamageShotsReceived"),
                piercedReceived = tankData.GetStaticSizeBlock("company2").GetValue("piercedReceived"),
                heHitsReceived = tankData.GetStaticSizeBlock("company2").GetValue("heHitsReceived"),
                he_hits = tankData.GetStaticSizeBlock("company2").GetValue("he_hits"),
                pierced = tankData.GetStaticSizeBlock("company2").GetValue("pierced"),
            };
            #endregion

            #region 7x7
            result.A7x7 = new StatisticJson
            {
                xp = tankData.GetStaticSizeBlock("a7x7").GetValue("xp"),
                battlesCount = tankData.GetStaticSizeBlock("a7x7").GetValue("battlesCount"),
                wins = tankData.GetStaticSizeBlock("a7x7").GetValue("wins"),
                losses = tankData.GetStaticSizeBlock("a7x7").GetValue("losses"),
                survivedBattles = tankData.GetStaticSizeBlock("a7x7").GetValue("survivedBattles"),
                frags = tankData.GetStaticSizeBlock("a7x7").GetValue("frags"),
                shots = tankData.GetStaticSizeBlock("a7x7").GetValue("shots"),
                hits = tankData.GetStaticSizeBlock("a7x7").GetValue("hits"),
                spotted = tankData.GetStaticSizeBlock("a7x7").GetValue("spotted"),
                damageDealt = tankData.GetStaticSizeBlock("a7x7").GetValue("damageDealt"),
                damageReceived = tankData.GetStaticSizeBlock("a7x7").GetValue("damageReceived"),
                capturePoints = tankData.GetStaticSizeBlock("a7x7").GetValue("capturePoints"),
                droppedCapturePoints = tankData.GetStaticSizeBlock("a7x7").GetValue("droppedCapturePoints"),

                originalXP = tankData.GetStaticSizeBlock("a7x7").GetValue("originalXP"),
                damageAssistedTrack = tankData.GetStaticSizeBlock("a7x7").GetValue("damageAssistedTrack"),
                damageAssistedRadio = tankData.GetStaticSizeBlock("a7x7").GetValue("damageAssistedRadio"),
                shotsReceived = tankData.GetStaticSizeBlock("a7x7").GetValue("shotsReceived"),
                noDamageShotsReceived = tankData.GetStaticSizeBlock("a7x7").GetValue("noDamageShotsReceived"),
                piercedReceived = tankData.GetStaticSizeBlock("a7x7").GetValue("piercedReceived"),
                heHitsReceived = tankData.GetStaticSizeBlock("a7x7").GetValue("heHitsReceived"),
                he_hits = tankData.GetStaticSizeBlock("a7x7").GetValue("he_hits"),
                pierced = tankData.GetStaticSizeBlock("a7x7").GetValue("pierced"),

                maxXP = tankData.GetStaticSizeBlock("max7x7").GetValue("maxXP"),
                maxFrags = tankData.GetStaticSizeBlock("max7x7").GetValue("maxFrags"),
                maxDamage = tankData.GetStaticSizeBlock("max7x7").GetValue("maxDamage"),
            };
            #endregion
            
            #region Achievements
            result.Achievements = new AchievementsJson
            {
                FragsBeast = tankData.GetStaticSizeBlock("achievements").GetValue("fragsBeast"),
                SharpshooterProgress = tankData.GetStaticSizeBlock("achievements").GetValue("sniperSeries"),
                SharpshooterLongest = tankData.GetStaticSizeBlock("achievements").GetValue("maxSniperSeries"),
                InvincibleProgress = tankData.GetStaticSizeBlock("achievements").GetValue("invincibleSeries"),
                InvincibleLongest = tankData.GetStaticSizeBlock("achievements").GetValue("maxInvincibleSeries"),
                SurvivorProgress = tankData.GetStaticSizeBlock("achievements").GetValue("diehardSeries"),
                SurvivorLongest = tankData.GetStaticSizeBlock("achievements").GetValue("maxDiehardSeries"),
                ReaperProgress = tankData.GetStaticSizeBlock("achievements").GetValue("killingSeries"),
                FragsSinai = tankData.GetStaticSizeBlock("achievements").GetValue("fragsSinai"),
                ReaperLongest = tankData.GetStaticSizeBlock("achievements").GetValue("maxKillingSeries"),
                MasterGunnerProgress = tankData.GetStaticSizeBlock("achievements").GetValue("piercingSeries"),
                MasterGunnerLongest = tankData.GetStaticSizeBlock("achievements").GetValue("maxPiercingSeries"),
                BattleHero = tankData.GetStaticSizeBlock("achievements").GetValue("battleHeroes"),
                Warrior = tankData.GetStaticSizeBlock("achievements").GetValue("warrior"),
                Invader = tankData.GetStaticSizeBlock("achievements").GetValue("invader"),
                Sniper = tankData.GetStaticSizeBlock("achievements").GetValue("sniper"),
                Defender = tankData.GetStaticSizeBlock("achievements").GetValue("defender"),
                SteelWall = tankData.GetStaticSizeBlock("achievements").GetValue("steelwall"),
                Confederate = tankData.GetStaticSizeBlock("achievements").GetValue("supporter"),
                Scout = tankData.GetStaticSizeBlock("achievements").GetValue("scout"),
                PatrolDuty = tankData.GetStaticSizeBlock("achievements").GetValue("evileye"),
                Kay = tankData.GetStaticSizeBlock("achievements").GetValue("medalKay"),
                Carius = tankData.GetStaticSizeBlock("achievements").GetValue("medalCarius"),
                Knispel = tankData.GetStaticSizeBlock("achievements").GetValue("medalKnispel"),
                Poppel = tankData.GetStaticSizeBlock("achievements").GetValue("medalPoppel"),
                Abrams = tankData.GetStaticSizeBlock("achievements").GetValue("medalAbrams"),
                Leclerk = tankData.GetStaticSizeBlock("achievements").GetValue("medalLeClerc"),
                Lavrinenko = tankData.GetStaticSizeBlock("achievements").GetValue("medalLavrinenko"),
                Ekins = tankData.GetStaticSizeBlock("achievements").GetValue("medalEkins"),
                Boelter = tankData.GetStaticSizeBlock("achievements").GetValue("medalWittmann"),
                Orlik = tankData.GetStaticSizeBlock("achievements").GetValue("medalOrlik"),
                Oskin = tankData.GetStaticSizeBlock("achievements").GetValue("medalOskin"),
                Halonen = tankData.GetStaticSizeBlock("achievements").GetValue("medalHalonen"),
                Burda = tankData.GetStaticSizeBlock("achievements").GetValue("medalBurda"),
                Billotte = tankData.GetStaticSizeBlock("achievements").GetValue("medalBillotte"),
                Kolobanov = tankData.GetStaticSizeBlock("achievements").GetValue("medalKolobanov"),
                Fadin = tankData.GetStaticSizeBlock("achievements").GetValue("medalFadin"),
                RadleyWalters = tankData.GetStaticSizeBlock("achievements").GetValue("medalRadleyWalters"),
                BrunoPietro = tankData.GetStaticSizeBlock("achievements").GetValue("medalBrunoPietro"),
                Tarczay = tankData.GetStaticSizeBlock("achievements").GetValue("medalTarczay"),
                Pascucci = tankData.GetStaticSizeBlock("achievements").GetValue("medalPascucci"),
                Dumitru = tankData.GetStaticSizeBlock("achievements").GetValue("medalDumitru"),
                Lehvaslaiho = tankData.GetStaticSizeBlock("achievements").GetValue("medalLehvaslaiho"),
                Nikolas = tankData.GetStaticSizeBlock("achievements").GetValue("medalNikolas"),
                LafayettePool = tankData.GetStaticSizeBlock("achievements").GetValue("medalLafayettePool"),

                Sinai = tankData.GetStaticSizeBlock("achievements").GetValue("sinai"),
                HeroesOfRassenay = tankData.GetStaticSizeBlock("achievements").GetValue("heroesOfRassenay"),

                Hunter = tankData.GetStaticSizeBlock("achievements").GetValue("beasthunter"),
                MouseTrap = tankData.GetStaticSizeBlock("achievements").GetValue("mousebane"),
                TankExpertStrg = tankData.GetStaticSizeBlock("achievements").GetValue("tankExpertStrg"),
                Sharpshooter = tankData.GetStaticSizeBlock("achievements").GetValue("titleSniper"),
                Invincible = tankData.GetStaticSizeBlock("achievements").GetValue("invincible"),
                Survivor = tankData.GetStaticSizeBlock("achievements").GetValue("diehard"),
                Raider = tankData.GetStaticSizeBlock("achievements").GetValue("raider"),
                Reaper = tankData.GetStaticSizeBlock("achievements").GetValue("handOfDeath"),
                MasterGunner = tankData.GetStaticSizeBlock("achievements").GetValue("armorPiercer"),
                Kamikaze = tankData.GetStaticSizeBlock("achievements").GetValue("kamikaze"),
                Lumberjack = tankData.GetStaticSizeBlock("achievements").GetValue("lumberjack"),

                BrothersInArms = tankData.GetStaticSizeBlock("achievements").GetValue("medalBrothersInArms"),
                CrucialContribution = tankData.GetStaticSizeBlock("achievements").GetValue("medalCrucialContribution"),
                DeLanglade = tankData.GetStaticSizeBlock("achievements").GetValue("medalDeLanglade"),
                TamadaYoshio = tankData.GetStaticSizeBlock("achievements").GetValue("medalTamadaYoshio"),
                Bombardier = tankData.GetStaticSizeBlock("achievements").GetValue("bombardier"),
                Huntsman = tankData.GetStaticSizeBlock("achievements").GetValue("huntsman"),
                Alaric = tankData.GetStaticSizeBlock("achievements").GetValue("alaric"),
                Sturdy = tankData.GetStaticSizeBlock("achievements").GetValue("sturdy"),
                IronMan = tankData.GetStaticSizeBlock("achievements").GetValue("ironMan"),
                LuckyDevil = tankData.GetStaticSizeBlock("achievements").GetValue("luckyDevil"),
                PattonValley = tankData.GetStaticSizeBlock("achievements").GetValue("pattonValley"),
                FragsPatton = tankData.GetStaticSizeBlock("achievements").GetValue("fragsPatton"),
                MarkOfMastery = tankData.GetStaticSizeBlock("achievements").GetValue("markOfMastery"),
            };
            #endregion

            result.Frags = tankData.Dict.TryGetValue("vehTypeFrags", out Dictionary<object, object> frgs)
                ? ProcessFrags(frgs.ToDictionary(p => Convert.ToInt32(p.Key), p => Convert.ToInt32(p.Value)),
                    tank.UniqueId)
                : new List<FragsJson>();
            return result;
        }
        private static TankJson Process69(DossierTankData tankData)
        {
            var result = Process65(tankData);
            result.Achievements7x7 = new Achievements7x7
            {
                WolfAmongSheep = tankData.GetStaticSizeBlock("achievements7x7").GetValue("wolfAmongSheep"),
                WolfAmongSheepMedal = tankData.GetStaticSizeBlock("achievements7x7").GetValue("wolfAmongSheepMedal"),
                GeniusForWar = tankData.GetStaticSizeBlock("achievements7x7").GetValue("geniusForWar"),
                GeniusForWarMedal = tankData.GetStaticSizeBlock("achievements7x7").GetValue("geniusForWarMedal"),
                KingOfTheHill = tankData.GetStaticSizeBlock("achievements7x7").GetValue("kingOfTheHill"),
                TacticalBreakthroughSeries =
                    tankData.GetStaticSizeBlock("achievements7x7").GetValue("tacticalBreakthroughSeries"),
                MaxTacticalBreakthroughSeries =
                    tankData.GetStaticSizeBlock("achievements7x7").GetValue("maxTacticalBreakthroughSeries"),
                ArmoredFist = tankData.GetStaticSizeBlock("achievements7x7").GetValue("armoredFist"),
                TacticalBreakthrough = tankData.GetStaticSizeBlock("achievements7x7").GetValue("tacticalBreakthrough"),
            };
            return result;
        }
        private static TankJson Process77(DossierTankData tankData)
        {
            var result = Process69(tankData);


            result.A15x15.hits = tankData.GetStaticSizeBlock("a15x15").GetValue("directHits");
            result.A15x15.battlesCountBefore9_0 = tankData.GetStaticSizeBlock("a15x15").GetValue("battlesCountBefore9_0");
            result.A15x15.shotsReceived = tankData.GetStaticSizeBlock("a15x15_2").GetValue("directHitsReceived");
            result.A15x15.noDamageShotsReceived = tankData.GetStaticSizeBlock("a15x15_2").GetValue("noDamageDirectHitsReceived");
            result.A15x15.piercedReceived = tankData.GetStaticSizeBlock("a15x15_2").GetValue("piercingsReceived");
            result.A15x15.heHitsReceived = tankData.GetStaticSizeBlock("a15x15_2").GetValue("explosionHitsReceived");
            result.A15x15.he_hits = tankData.GetStaticSizeBlock("a15x15_2").GetValue("explosionHits");
            result.A15x15.pierced = tankData.GetStaticSizeBlock("a15x15_2").GetValue("piercings");
            result.A15x15.potentialDamageReceived = tankData.GetStaticSizeBlock("a15x15_2").GetValue("potentialDamageReceived");
            result.A15x15.damageBlockedByArmor = tankData.GetStaticSizeBlock("a15x15_2").GetValue("damageBlockedByArmor");

            result.Clan.hits = tankData.GetStaticSizeBlock("clan").GetValue("directHits");
            result.Clan.battlesCountBefore9_0 = tankData.GetStaticSizeBlock("clan").GetValue("battlesCountBefore9_0");
            result.Clan.shotsReceived = tankData.GetStaticSizeBlock("clan2").GetValue("directHitsReceived");
            result.Clan.noDamageShotsReceived = tankData.GetStaticSizeBlock("clan2").GetValue("noDamageDirectHitsReceived");
            result.Clan.piercedReceived = tankData.GetStaticSizeBlock("clan2").GetValue("piercingsReceived");
            result.Clan.heHitsReceived = tankData.GetStaticSizeBlock("clan2").GetValue("explosionHitsReceived");
            result.Clan.he_hits = tankData.GetStaticSizeBlock("clan2").GetValue("explosionHits");
            result.Clan.pierced = tankData.GetStaticSizeBlock("clan2").GetValue("piercings");
            result.Clan.potentialDamageReceived = tankData.GetStaticSizeBlock("clan2").GetValue("potentialDamageReceived");
            result.Clan.damageBlockedByArmor = tankData.GetStaticSizeBlock("clan2").GetValue("damageBlockedByArmor");

            result.Company.hits = tankData.GetStaticSizeBlock("company").GetValue("directHits");
            result.Company.battlesCountBefore9_0 = tankData.GetStaticSizeBlock("company").GetValue("battlesCountBefore9_0");
            result.Company.shotsReceived = tankData.GetStaticSizeBlock("company2").GetValue("directHitsReceived");
            result.Company.noDamageShotsReceived = tankData.GetStaticSizeBlock("company2").GetValue("noDamageDirectHitsReceived");
            result.Company.piercedReceived = tankData.GetStaticSizeBlock("company2").GetValue("piercingsReceived");
            result.Company.heHitsReceived = tankData.GetStaticSizeBlock("company2").GetValue("explosionHitsReceived");
            result.Company.he_hits = tankData.GetStaticSizeBlock("company2").GetValue("explosionHits");
            result.Company.pierced = tankData.GetStaticSizeBlock("company2").GetValue("piercings");
            result.Company.potentialDamageReceived = tankData.GetStaticSizeBlock("company2").GetValue("potentialDamageReceived");
            result.Company.damageBlockedByArmor = tankData.GetStaticSizeBlock("company2").GetValue("damageBlockedByArmor");

            result.A7x7.hits = tankData.GetStaticSizeBlock("a7x7").GetValue("directHits");
            result.A7x7.shotsReceived = tankData.GetStaticSizeBlock("a7x7").GetValue("directHitsReceived");
            result.A7x7.piercedReceived = tankData.GetStaticSizeBlock("a7x7").GetValue("piercingsReceived");
            result.A7x7.noDamageShotsReceived = tankData.GetStaticSizeBlock("a7x7").GetValue("noDamageDirectHitsReceived");
            result.A7x7.heHitsReceived = tankData.GetStaticSizeBlock("a7x7").GetValue("explosionHitsReceived");
            result.A7x7.he_hits = tankData.GetStaticSizeBlock("a7x7").GetValue("explosionHits");
            result.A7x7.pierced = tankData.GetStaticSizeBlock("a7x7").GetValue("piercings");
            result.A7x7.winAndSurvived = tankData.GetStaticSizeBlock("a7x7").GetValue("winAndSurvived");
            result.A7x7.frags8p = tankData.GetStaticSizeBlock("a7x7").GetValue("frags8p");
            result.A7x7.potentialDamageReceived = tankData.GetStaticSizeBlock("a7x7").GetValue("potentialDamageReceived");
            result.A7x7.damageBlockedByArmor = tankData.GetStaticSizeBlock("a7x7").GetValue("damageBlockedByArmor");
            result.A7x7.battlesCountBefore9_0 = tankData.GetStaticSizeBlock("a7x7").GetValue("battlesCountBefore9_0");

            result.Achievements.Sniper2 = tankData.GetStaticSizeBlock("achievements").GetValue("sniper2");
            result.Achievements.MainGun = tankData.GetStaticSizeBlock("achievements").GetValue("mainGun");
            result.Achievements.MarksOnGun = tankData.GetStaticSizeBlock("achievements").GetValue("marksOnGun");
            result.Achievements.MovingAvgDamage = tankData.GetStaticSizeBlock("achievements").GetValue("movingAvgDamage");
            result.Achievements.MedalMonolith = tankData.GetStaticSizeBlock("achievements").GetValue("medalMonolith");
            result.Achievements.MedalAntiSpgFire = tankData.GetStaticSizeBlock("achievements").GetValue("medalAntiSpgFire");
            result.Achievements.MedalGore = tankData.GetStaticSizeBlock("achievements").GetValue("medalGore");
            result.Achievements.MedalCoolBlood = tankData.GetStaticSizeBlock("achievements").GetValue("medalCoolBlood");
            result.Achievements.MedalStark = tankData.GetStaticSizeBlock("achievements").GetValue("medalStark");

            result.Achievements7x7.GodOfWar = tankData.GetStaticSizeBlock("achievements7x7").GetValue("godOfWar");
            result.Achievements7x7.FightingReconnaissance = tankData.GetStaticSizeBlock("achievements7x7").GetValue("fightingReconnaissance");
            result.Achievements7x7.FightingReconnaissanceMedal = tankData.GetStaticSizeBlock("achievements7x7").GetValue("fightingReconnaissanceMedal");
            result.Achievements7x7.WillToWinSpirit = tankData.GetStaticSizeBlock("achievements7x7").GetValue("willToWinSpirit");
            result.Achievements7x7.CrucialShot = tankData.GetStaticSizeBlock("achievements7x7").GetValue("crucialShot");
            result.Achievements7x7.CrucialShotMedal = tankData.GetStaticSizeBlock("achievements7x7").GetValue("crucialShotMedal");
            result.Achievements7x7.ForTacticalOperations = tankData.GetStaticSizeBlock("achievements7x7").GetValue("forTacticalOperations");

            #region Historical
            result.Historical = new StatisticJson
            {
                xp = tankData.GetStaticSizeBlock("historical").GetValue("xp"),
                battlesCount = tankData.GetStaticSizeBlock("historical").GetValue("battlesCount"),
                wins = tankData.GetStaticSizeBlock("historical").GetValue("wins"),
                losses = tankData.GetStaticSizeBlock("historical").GetValue("losses"),
                survivedBattles = tankData.GetStaticSizeBlock("historical").GetValue("survivedBattles"),
                frags = tankData.GetStaticSizeBlock("historical").GetValue("frags"),
                shots = tankData.GetStaticSizeBlock("historical").GetValue("shots"),
                hits = tankData.GetStaticSizeBlock("historical").GetValue("directHits"),
                spotted = tankData.GetStaticSizeBlock("historical").GetValue("spotted"),
                damageDealt = tankData.GetStaticSizeBlock("historical").GetValue("damageDealt"),
                damageReceived = tankData.GetStaticSizeBlock("historical").GetValue("damageReceived"),
                capturePoints = tankData.GetStaticSizeBlock("historical").GetValue("capturePoints"),
                droppedCapturePoints = tankData.GetStaticSizeBlock("historical").GetValue("droppedCapturePoints"),

                originalXP = tankData.GetStaticSizeBlock("historical").GetValue("originalXP"),
                damageAssistedTrack = tankData.GetStaticSizeBlock("historical").GetValue("damageAssistedTrack"),
                damageAssistedRadio = tankData.GetStaticSizeBlock("historical").GetValue("damageAssistedRadio"),
                shotsReceived = tankData.GetStaticSizeBlock("historical").GetValue("directHitsReceived"),
                noDamageShotsReceived = tankData.GetStaticSizeBlock("historical").GetValue("noDamageDirectHitsReceived"),
                piercedReceived = tankData.GetStaticSizeBlock("historical").GetValue("piercingsReceived"),
                heHitsReceived = tankData.GetStaticSizeBlock("historical").GetValue("explosionHitsReceived"),
                he_hits = tankData.GetStaticSizeBlock("historical").GetValue("explosionHits"),
                pierced = tankData.GetStaticSizeBlock("historical").GetValue("piercings"),
                winAndSurvived = tankData.GetStaticSizeBlock("historical").GetValue("winAndSurvived"),
                frags8p = tankData.GetStaticSizeBlock("historical").GetValue("frags8p"),
                potentialDamageReceived = tankData.GetStaticSizeBlock("historical").GetValue("potentialDamageReceived"),
                damageBlockedByArmor = tankData.GetStaticSizeBlock("historical").GetValue("damageBlockedByArmor"),

                maxXP = tankData.GetStaticSizeBlock("maxHistorical").GetValue("maxXP"),
                maxFrags = tankData.GetStaticSizeBlock("maxHistorical").GetValue("maxFrags"),
                maxDamage = tankData.GetStaticSizeBlock("maxHistorical").GetValue("maxDamage"),
            };
            #endregion

            return result;
        }
        
        /// <summary>
        /// 0.9.1
        /// </summary>
        private static TankJson Process81(DossierTankData tankData)
        {
            var result = Process77(tankData);

            result.Achievements.DamageRating = tankData.GetStaticSizeBlock("achievements").GetValue("damageRating");

            result.Achievements7x7.PromisingFighter = tankData.GetStaticSizeBlock("achievements7x7").GetValue("promisingFighter");
            result.Achievements7x7.PromisingFighterMedal = tankData.GetStaticSizeBlock("achievements7x7").GetValue("promisingFighterMedal");
            result.Achievements7x7.HeavyFire = tankData.GetStaticSizeBlock("achievements7x7").GetValue("heavyFire");
            result.Achievements7x7.HeavyFireMedal = tankData.GetStaticSizeBlock("achievements7x7").GetValue("heavyFireMedal");
            result.Achievements7x7.Ranger = tankData.GetStaticSizeBlock("achievements7x7").GetValue("ranger");
            result.Achievements7x7.RangerMedal = tankData.GetStaticSizeBlock("achievements7x7").GetValue("rangerMedal");
            result.Achievements7x7.FireAndSteel = tankData.GetStaticSizeBlock("achievements7x7").GetValue("fireAndSteel");
            result.Achievements7x7.FireAndSteelMedal = tankData.GetStaticSizeBlock("achievements7x7").GetValue("fireAndSteelMedal");
            result.Achievements7x7.Pyromaniac = tankData.GetStaticSizeBlock("achievements7x7").GetValue("pyromaniac");
            result.Achievements7x7.PyromaniacMedal = tankData.GetStaticSizeBlock("achievements7x7").GetValue("pyromaniacMedal");
            result.Achievements7x7.NoMansLand = tankData.GetStaticSizeBlock("achievements7x7").GetValue("noMansLand");

            result.AchievementsHistorical = new AchievementsHistorical
            {
                GuardsMan = tankData.GetStaticSizeBlock("uniqueAchievements").GetValue("guardsman"),
                MakerOfHistory = tankData.GetStaticSizeBlock("uniqueAchievements").GetValue("makerOfHistory"),
                BothSidesWins = tankData.GetStaticSizeBlock("uniqueAchievements").GetValue("bothSidesWins"),
                WeakVehiclesWins = tankData.GetStaticSizeBlock("uniqueAchievements").GetValue("weakVehiclesWins"),
            };

            #region FortBattles
            result.FortBattles = new StatisticJsonFort
            {
                xp = tankData.GetStaticSizeBlock("fortBattles").GetValue("xp"),
                battlesCount = tankData.GetStaticSizeBlock("fortBattles").GetValue("battlesCount"),
                wins = tankData.GetStaticSizeBlock("fortBattles").GetValue("wins"),
                losses = tankData.GetStaticSizeBlock("fortBattles").GetValue("losses"),
                survivedBattles = tankData.GetStaticSizeBlock("fortBattles").GetValue("survivedBattles"),
                frags = tankData.GetStaticSizeBlock("fortBattles").GetValue("frags"),
                shots = tankData.GetStaticSizeBlock("fortBattles").GetValue("shots"),
                hits = tankData.GetStaticSizeBlock("fortBattles").GetValue("directHits"),
                spotted = tankData.GetStaticSizeBlock("fortBattles").GetValue("spotted"),
                damageDealt = tankData.GetStaticSizeBlock("fortBattles").GetValue("damageDealt"),
                damageReceived = tankData.GetStaticSizeBlock("fortBattles").GetValue("damageReceived"),
                capturePoints = tankData.GetStaticSizeBlock("fortBattles").GetValue("capturePoints"),
                droppedCapturePoints = tankData.GetStaticSizeBlock("fortBattles").GetValue("droppedCapturePoints"),

                originalXP = tankData.GetStaticSizeBlock("fortBattles").GetValue("originalXP"),
                damageAssistedTrack = tankData.GetStaticSizeBlock("fortBattles").GetValue("damageAssistedTrack"),
                damageAssistedRadio = tankData.GetStaticSizeBlock("fortBattles").GetValue("damageAssistedRadio"),
                shotsReceived = tankData.GetStaticSizeBlock("fortBattles").GetValue("directHitsReceived"),
                noDamageShotsReceived = tankData.GetStaticSizeBlock("fortBattles").GetValue("noDamageDirectHitsReceived"),
                piercedReceived = tankData.GetStaticSizeBlock("fortBattles").GetValue("piercingsReceived"),
                heHitsReceived = tankData.GetStaticSizeBlock("fortBattles").GetValue("explosionHitsReceived"),
                he_hits = tankData.GetStaticSizeBlock("fortBattles").GetValue("explosionHits"),
                pierced = tankData.GetStaticSizeBlock("fortBattles").GetValue("piercings"),
                winAndSurvived = tankData.GetStaticSizeBlock("fortBattles").GetValue("winAndSurvived"),
                frags8p = tankData.GetStaticSizeBlock("fortBattles").GetValue("frags8p"),
                potentialDamageReceived = tankData.GetStaticSizeBlock("fortBattles").GetValue("potentialDamageReceived"),
                damageBlockedByArmor = tankData.GetStaticSizeBlock("fortBattles").GetValue("damageBlockedByArmor"),

                maxXP = tankData.GetStaticSizeBlock("maxFortBattles").GetValue("maxXP"),
                maxFrags = tankData.GetStaticSizeBlock("maxFortBattles").GetValue("maxFrags"),
                maxDamage = tankData.GetStaticSizeBlock("maxFortBattles").GetValue("maxDamage"),
            };
            #endregion

            #region FortSorties
            result.FortSorties = new StatisticJsonSortie()
            {
                xp = tankData.GetStaticSizeBlock("fortSorties").GetValue("xp"),
                battlesCount = tankData.GetStaticSizeBlock("fortSorties").GetValue("battlesCount"),
                wins = tankData.GetStaticSizeBlock("fortSorties").GetValue("wins"),
                losses = tankData.GetStaticSizeBlock("fortSorties").GetValue("losses"),
                survivedBattles = tankData.GetStaticSizeBlock("fortSorties").GetValue("survivedBattles"),
                frags = tankData.GetStaticSizeBlock("fortSorties").GetValue("frags"),
                shots = tankData.GetStaticSizeBlock("fortSorties").GetValue("shots"),
                hits = tankData.GetStaticSizeBlock("fortSorties").GetValue("directHits"),
                spotted = tankData.GetStaticSizeBlock("fortSorties").GetValue("spotted"),
                damageDealt = tankData.GetStaticSizeBlock("fortSorties").GetValue("damageDealt"),
                damageReceived = tankData.GetStaticSizeBlock("fortSorties").GetValue("damageReceived"),
                capturePoints = tankData.GetStaticSizeBlock("fortSorties").GetValue("capturePoints"),
                droppedCapturePoints = tankData.GetStaticSizeBlock("fortSorties").GetValue("droppedCapturePoints"),

                originalXP = tankData.GetStaticSizeBlock("fortSorties").GetValue("originalXP"),
                damageAssistedTrack = tankData.GetStaticSizeBlock("fortSorties").GetValue("damageAssistedTrack"),
                damageAssistedRadio = tankData.GetStaticSizeBlock("fortSorties").GetValue("damageAssistedRadio"),
                shotsReceived = tankData.GetStaticSizeBlock("fortSorties").GetValue("directHitsReceived"),
                noDamageShotsReceived = tankData.GetStaticSizeBlock("fortSorties").GetValue("noDamageDirectHitsReceived"),
                piercedReceived = tankData.GetStaticSizeBlock("fortSorties").GetValue("piercingsReceived"),
                heHitsReceived = tankData.GetStaticSizeBlock("fortSorties").GetValue("explosionHitsReceived"),
                he_hits = tankData.GetStaticSizeBlock("fortSorties").GetValue("explosionHits"),
                pierced = tankData.GetStaticSizeBlock("fortSorties").GetValue("piercings"),
                winAndSurvived = tankData.GetStaticSizeBlock("fortSorties").GetValue("winAndSurvived"),
                frags8p = tankData.GetStaticSizeBlock("fortSorties").GetValue("frags8p"),
                potentialDamageReceived = tankData.GetStaticSizeBlock("fortSorties").GetValue("potentialDamageReceived"),
                damageBlockedByArmor = tankData.GetStaticSizeBlock("fortSorties").GetValue("damageBlockedByArmor"),

                maxXP = tankData.GetStaticSizeBlock("maxFortSorties").GetValue("maxXP"),
                maxFrags = tankData.GetStaticSizeBlock("maxFortSorties").GetValue("maxFrags"),
                maxDamage = tankData.GetStaticSizeBlock("maxFortSorties").GetValue("maxDamage"),
            };
            #endregion

            #region FortSorties
            result.FortAchievements = new AchievementsFort()
            {
                Conqueror = tankData.GetStaticSizeBlock("fortAchievements").GetValue("conqueror"),
                FireAndSword = tankData.GetStaticSizeBlock("fortAchievements").GetValue("fireAndSword"),
                Crusher = tankData.GetStaticSizeBlock("fortAchievements").GetValue("crusher"),
                CounterBlow = tankData.GetStaticSizeBlock("fortAchievements").GetValue("counterblow"),
                SoldierOfFortune = tankData.GetStaticSizeBlock("fortAchievements").GetValue("soldierOfFortune"),
                Kampfer = tankData.GetStaticSizeBlock("fortAchievements").GetValue("kampfer")
            };
            #endregion

            return result;
        }

        /// <summary>
        /// 0.9.2
        /// </summary>
        private static TankJson Process85(DossierTankData tankData)
        {
            var result = Process81(tankData);

            result.Achievements.Sharpshooter = tankData.GetStaticSizeBlock("singleAchievements").GetValue("titleSniper");
            result.Achievements.Invincible = tankData.GetStaticSizeBlock("singleAchievements").GetValue("invincible");
            result.Achievements.Survivor = tankData.GetStaticSizeBlock("singleAchievements").GetValue("diehard");
            result.Achievements.Reaper = tankData.GetStaticSizeBlock("singleAchievements").GetValue("handOfDeath");
            result.Achievements.MasterGunner = tankData.GetStaticSizeBlock("singleAchievements").GetValue("armorPiercer");
            result.Achievements7x7.TacticalBreakthrough = tankData.GetStaticSizeBlock("singleAchievements").GetValue("tacticalBreakthrough");

            result.AchievementsClan = new AchievementsClan
            {
                MedalRotmistrov = tankData.GetStaticSizeBlock("clanAchievements").GetValue("medalRotmistrov")
            };

            return result;
        }

        /// <summary>
        /// 0.9.3
        /// </summary>
        private static TankJson Process87(DossierTankData tankData)
        {
            var result = Process85(tankData);

            result.Achievements.Impenetrable = tankData.GetStaticSizeBlock("achievements").GetValue("impenetrable");
            result.Achievements.MaxAimerSeries = tankData.GetStaticSizeBlock("achievements").GetValue("maxAimerSeries");
            result.Achievements.ShootToKill = tankData.GetStaticSizeBlock("achievements").GetValue("shootToKill");
            result.Achievements.Fighter = tankData.GetStaticSizeBlock("achievements").GetValue("fighter");
            result.Achievements.Duelist = tankData.GetStaticSizeBlock("achievements").GetValue("duelist");
            result.Achievements.Demolition = tankData.GetStaticSizeBlock("achievements").GetValue("demolition");
            result.Achievements.Arsonist = tankData.GetStaticSizeBlock("achievements").GetValue("arsonist");
            result.Achievements.Bonecrusher = tankData.GetStaticSizeBlock("achievements").GetValue("bonecrusher");
            result.Achievements.Charmed = tankData.GetStaticSizeBlock("achievements").GetValue("charmed");
            result.Achievements.Even = tankData.GetStaticSizeBlock("achievements").GetValue("even");

            result.Achievements.Aimer = tankData.GetStaticSizeBlock("singleAchievements").GetValue("aimer");

            result.FortAchievements.FortWins = tankData.GetStaticSizeBlock("fortAchievements").GetValue("wins");
            result.FortAchievements.CapturedBasesInAttack = tankData.GetStaticSizeBlock("fortAchievements").GetValue("capturedBasesInAttack");
            result.FortAchievements.CapturedBasesInAttack = tankData.GetStaticSizeBlock("fortAchievements").GetValue("capturedBasesInDefence");

            return result;
        }
        private static TankJson Process88(DossierTankData tankData)
        {
            var result = Process87(tankData);

            #region Rated7x7
            result.Rated7x7 = new StatisticJson()
            {
                xp = tankData.GetStaticSizeBlock("rated7x7").GetValue("xp"),
                battlesCount = tankData.GetStaticSizeBlock("rated7x7").GetValue("battlesCount"),
                wins = tankData.GetStaticSizeBlock("rated7x7").GetValue("wins"),
                losses = tankData.GetStaticSizeBlock("rated7x7").GetValue("losses"),
                survivedBattles = tankData.GetStaticSizeBlock("rated7x7").GetValue("survivedBattles"),
                frags = tankData.GetStaticSizeBlock("rated7x7").GetValue("frags"),
                shots = tankData.GetStaticSizeBlock("rated7x7").GetValue("shots"),
                hits = tankData.GetStaticSizeBlock("rated7x7").GetValue("directHits"),
                spotted = tankData.GetStaticSizeBlock("rated7x7").GetValue("spotted"),
                damageDealt = tankData.GetStaticSizeBlock("rated7x7").GetValue("damageDealt"),
                damageReceived = tankData.GetStaticSizeBlock("rated7x7").GetValue("damageReceived"),
                capturePoints = tankData.GetStaticSizeBlock("rated7x7").GetValue("capturePoints"),
                droppedCapturePoints = tankData.GetStaticSizeBlock("rated7x7").GetValue("droppedCapturePoints"),

                originalXP = tankData.GetStaticSizeBlock("rated7x7").GetValue("originalXP"),
                damageAssistedTrack = tankData.GetStaticSizeBlock("rated7x7").GetValue("damageAssistedTrack"),
                damageAssistedRadio = tankData.GetStaticSizeBlock("rated7x7").GetValue("damageAssistedRadio"),
                shotsReceived = tankData.GetStaticSizeBlock("rated7x7").GetValue("directHitsReceived"),
                noDamageShotsReceived = tankData.GetStaticSizeBlock("rated7x7").GetValue("noDamageDirectHitsReceived"),
                piercedReceived = tankData.GetStaticSizeBlock("rated7x7").GetValue("piercingsReceived"),
                heHitsReceived = tankData.GetStaticSizeBlock("rated7x7").GetValue("explosionHitsReceived"),
                he_hits = tankData.GetStaticSizeBlock("rated7x7").GetValue("explosionHits"),
                pierced = tankData.GetStaticSizeBlock("rated7x7").GetValue("piercings"),
                winAndSurvived = tankData.GetStaticSizeBlock("rated7x7").GetValue("winAndSurvived"),
                frags8p = tankData.GetStaticSizeBlock("rated7x7").GetValue("frags8p"),
                potentialDamageReceived = tankData.GetStaticSizeBlock("rated7x7").GetValue("potentialDamageReceived"),
                damageBlockedByArmor = tankData.GetStaticSizeBlock("rated7x7").GetValue("damageBlockedByArmor"),

                maxXP = tankData.GetStaticSizeBlock("maxRated7x7").GetValue("maxXP"),
                maxFrags = tankData.GetStaticSizeBlock("maxRated7x7").GetValue("maxFrags"),
                maxDamage = tankData.GetStaticSizeBlock("maxRated7x7").GetValue("maxDamage"),
            };
            #endregion

            return result;
        }
        private static TankJson Process92(DossierTankData tankData)
        {
            var result = Process88(tankData);

            #region GlobalMapCommon
            result.GlobalMapCommon = new StatisticJson()
            {
                xp = tankData.GetStaticSizeBlock("globalMapCommon").GetValue("xp"),
                battlesCount = tankData.GetStaticSizeBlock("globalMapCommon").GetValue("battlesCount"),
                wins = tankData.GetStaticSizeBlock("globalMapCommon").GetValue("wins"),
                losses = tankData.GetStaticSizeBlock("globalMapCommon").GetValue("losses"),
                survivedBattles = tankData.GetStaticSizeBlock("globalMapCommon").GetValue("survivedBattles"),
                frags = tankData.GetStaticSizeBlock("globalMapCommon").GetValue("frags"),
                shots = tankData.GetStaticSizeBlock("globalMapCommon").GetValue("shots"),
                hits = tankData.GetStaticSizeBlock("globalMapCommon").GetValue("directHits"),
                spotted = tankData.GetStaticSizeBlock("globalMapCommon").GetValue("spotted"),
                damageDealt = tankData.GetStaticSizeBlock("globalMapCommon").GetValue("damageDealt"),
                damageReceived = tankData.GetStaticSizeBlock("globalMapCommon").GetValue("damageReceived"),
                capturePoints = tankData.GetStaticSizeBlock("globalMapCommon").GetValue("capturePoints"),
                droppedCapturePoints = tankData.GetStaticSizeBlock("globalMapCommon").GetValue("droppedCapturePoints"),

                originalXP = tankData.GetStaticSizeBlock("globalMapCommon").GetValue("originalXP"),
                damageAssistedTrack = tankData.GetStaticSizeBlock("globalMapCommon").GetValue("damageAssistedTrack"),
                damageAssistedRadio = tankData.GetStaticSizeBlock("globalMapCommon").GetValue("damageAssistedRadio"),
                shotsReceived = tankData.GetStaticSizeBlock("globalMapCommon").GetValue("directHitsReceived"),
                noDamageShotsReceived = tankData.GetStaticSizeBlock("globalMapCommon").GetValue("noDamageDirectHitsReceived"),
                piercedReceived = tankData.GetStaticSizeBlock("globalMapCommon").GetValue("piercingsReceived"),
                heHitsReceived = tankData.GetStaticSizeBlock("globalMapCommon").GetValue("explosionHitsReceived"),
                he_hits = tankData.GetStaticSizeBlock("globalMapCommon").GetValue("explosionHits"),
                pierced = tankData.GetStaticSizeBlock("globalMapCommon").GetValue("piercings"),
                winAndSurvived = tankData.GetStaticSizeBlock("globalMapCommon").GetValue("winAndSurvived"),
                frags8p = tankData.GetStaticSizeBlock("globalMapCommon").GetValue("frags8p"),
                potentialDamageReceived = tankData.GetStaticSizeBlock("globalMapCommon").GetValue("potentialDamageReceived"),
                damageBlockedByArmor = tankData.GetStaticSizeBlock("globalMapCommon").GetValue("damageBlockedByArmor"),

                maxXP = tankData.GetStaticSizeBlock("maxGlobalMapCommon").GetValue("maxXP"),
                maxFrags = tankData.GetStaticSizeBlock("maxGlobalMapCommon").GetValue("maxFrags"),
                maxDamage = tankData.GetStaticSizeBlock("maxGlobalMapCommon").GetValue("maxDamage"),
            };
            #endregion

            return result;
        }
        /// <summary>
        /// 0.9.12, 0.9.13, 0.9.14, 0.9.15, 0.9.16, 0.9.17
        /// </summary>
        private static TankJson Process94(DossierTankData tankData)
        {
            var result = Process92(tankData);

            #region Fallout
            result.Fallout = new StatisticJsonFallout()
            {
                xp = tankData.GetStaticSizeBlock("fallout").GetValue("xp"),
                battlesCount = tankData.GetStaticSizeBlock("fallout").GetValue("battlesCount"),
                wins = tankData.GetStaticSizeBlock("fallout").GetValue("wins"),
                losses = tankData.GetStaticSizeBlock("fallout").GetValue("losses"),
                survivedBattles = tankData.GetStaticSizeBlock("fallout").GetValue("survivedBattles"),
                frags = tankData.GetStaticSizeBlock("fallout").GetValue("frags"),
                shots = tankData.GetStaticSizeBlock("fallout").GetValue("shots"),
                hits = tankData.GetStaticSizeBlock("fallout").GetValue("directHits"),
                spotted = tankData.GetStaticSizeBlock("fallout").GetValue("spotted"),
                damageDealt = tankData.GetStaticSizeBlock("fallout").GetValue("damageDealt"),
                damageReceived = tankData.GetStaticSizeBlock("fallout").GetValue("damageReceived"),
                capturePoints = tankData.GetStaticSizeBlock("fallout").GetValue("capturePoints"),
                droppedCapturePoints = tankData.GetStaticSizeBlock("fallout").GetValue("droppedCapturePoints"),

                originalXP = tankData.GetStaticSizeBlock("fallout").GetValue("originalXP"),
                damageAssistedTrack = tankData.GetStaticSizeBlock("fallout").GetValue("damageAssistedTrack"),
                damageAssistedRadio = tankData.GetStaticSizeBlock("fallout").GetValue("damageAssistedRadio"),
                shotsReceived = tankData.GetStaticSizeBlock("fallout").GetValue("directHitsReceived"),
                noDamageShotsReceived = tankData.GetStaticSizeBlock("fallout").GetValue("noDamageDirectHitsReceived"),
                piercedReceived = tankData.GetStaticSizeBlock("fallout").GetValue("piercingsReceived"),
                heHitsReceived = tankData.GetStaticSizeBlock("fallout").GetValue("explosionHitsReceived"),
                he_hits = tankData.GetStaticSizeBlock("fallout").GetValue("explosionHits"),
                pierced = tankData.GetStaticSizeBlock("fallout").GetValue("piercings"),
                winAndSurvived = tankData.GetStaticSizeBlock("fallout").GetValue("winAndSurvived"),
                frags8p = tankData.GetStaticSizeBlock("fallout").GetValue("frags8p"),
                potentialDamageReceived = tankData.GetStaticSizeBlock("fallout").GetValue("potentialDamageReceived"),
                damageBlockedByArmor = tankData.GetStaticSizeBlock("fallout").GetValue("damageBlockedByArmor"),

                winPoints = tankData.GetStaticSizeBlock("fallout").GetValue("winPoints"),
                flagCapture = tankData.GetStaticSizeBlock("fallout").GetValue("flagCapture"),
                soloFlagCapture = tankData.GetStaticSizeBlock("fallout").GetValue("soloFlagCapture"),
                coins = tankData.GetStaticSizeBlock("fallout").GetValue("coins"),
                resourceAbsorbed = tankData.GetStaticSizeBlock("fallout").GetValue("resourceAbsorbed"),
                deathCount = tankData.GetStaticSizeBlock("fallout").GetValue("deathCount"),

                maxXP = tankData.GetStaticSizeBlock("maxFallout").GetValue("maxXP"),
                maxFrags = tankData.GetStaticSizeBlock("maxFallout").GetValue("maxFrags"),
                maxDamage = tankData.GetStaticSizeBlock("maxFallout").GetValue("maxDamage"),
                maxWinPoints = tankData.GetStaticSizeBlock("maxFallout").GetValue("maxWinPoints"),
                maxCoins = tankData.GetStaticSizeBlock("maxFallout").GetValue("maxCoins"),
            };
            #endregion

            #region AchievementsFallout

            result.AchievementsFallout = new AchievementsFallout()
            {
                ShoulderToShoulder = tankData.GetStaticSizeBlock("falloutAchievements").GetValue("shoulderToShoulder"),
                AloneInTheField = tankData.GetStaticSizeBlock("falloutAchievements").GetValue("aloneInTheField"),
                FallenFlags = tankData.GetStaticSizeBlock("falloutAchievements").GetValue("fallenFlags"),
                EffectiveSupport = tankData.GetStaticSizeBlock("falloutAchievements").GetValue("effectiveSupport"),
                StormLord = tankData.GetStaticSizeBlock("falloutAchievements").GetValue("stormLord"),
                WinnerLaurels = tankData.GetStaticSizeBlock("falloutAchievements").GetValue("winnerLaurels"),
                Predator = tankData.GetStaticSizeBlock("falloutAchievements").GetValue("predator"),
                Unreachable = tankData.GetStaticSizeBlock("falloutAchievements").GetValue("unreachable"),
                Champion = tankData.GetStaticSizeBlock("falloutAchievements").GetValue("champion"),
                Bannerman = tankData.GetStaticSizeBlock("falloutAchievements").GetValue("bannerman"),
                FalloutDieHard = tankData.GetStaticSizeBlock("falloutAchievements").GetValue("falloutDieHard"),
                SauronEye = tankData.GetStaticSizeBlock("falloutAchievements").GetValue("sauronEye"),
            };

            #endregion
            return result;
        }

        /// <summary>
        /// 0.9.18
        /// </summary>
        /// <param name="tankData"></param>
        /// <returns></returns>
        private static TankJson Process95(DossierTankData tankData)
        {
            var result = Process94(tankData);

            result.A15x15.battlesOnStunningVehicles = tankData.GetStaticSizeBlock("a15x15_2").GetValue("battlesOnStunningVehicles");
            result.A15x15.stunNum = tankData.GetStaticSizeBlock("a15x15_2").GetValue("stunNum");
            result.A15x15.damageAssistedStun = tankData.GetStaticSizeBlock("a15x15_2").GetValue("damageAssistedStun");

            result.Clan.battlesOnStunningVehicles = tankData.GetStaticSizeBlock("clan2").GetValue("battlesOnStunningVehicles");
            result.Clan.stunNum = tankData.GetStaticSizeBlock("clan2").GetValue("stunNum");
            result.Clan.damageAssistedStun = tankData.GetStaticSizeBlock("clan2").GetValue("damageAssistedStun");

            result.Company.battlesOnStunningVehicles = tankData.GetStaticSizeBlock("company2").GetValue("battlesOnStunningVehicles");
            result.Company.stunNum = tankData.GetStaticSizeBlock("company2").GetValue("stunNum");
            result.Company.damageAssistedStun = tankData.GetStaticSizeBlock("company2").GetValue("damageAssistedStun");

            result.A7x7.battlesOnStunningVehicles = tankData.GetStaticSizeBlock("a7x7").GetValue("battlesOnStunningVehicles");
            result.A7x7.stunNum = tankData.GetStaticSizeBlock("a7x7").GetValue("stunNum");
            result.A7x7.damageAssistedStun = tankData.GetStaticSizeBlock("a7x7").GetValue("damageAssistedStun");

            result.Historical.battlesOnStunningVehicles = tankData.GetStaticSizeBlock("historical").GetValue("battlesOnStunningVehicles");
            result.Historical.stunNum = tankData.GetStaticSizeBlock("historical").GetValue("stunNum");
            result.Historical.damageAssistedStun = tankData.GetStaticSizeBlock("historical").GetValue("damageAssistedStun");

            result.FortBattles.battlesOnStunningVehicles = tankData.GetStaticSizeBlock("fortBattles").GetValue("battlesOnStunningVehicles");
            result.FortBattles.stunNum = tankData.GetStaticSizeBlock("fortBattles").GetValue("stunNum");
            result.FortBattles.damageAssistedStun = tankData.GetStaticSizeBlock("fortBattles").GetValue("damageAssistedStun");

            result.FortSorties.battlesOnStunningVehicles = tankData.GetStaticSizeBlock("fortSorties").GetValue("battlesOnStunningVehicles");
            result.FortSorties.stunNum = tankData.GetStaticSizeBlock("fortSorties").GetValue("stunNum");
            result.FortSorties.damageAssistedStun = tankData.GetStaticSizeBlock("fortSorties").GetValue("damageAssistedStun");

            result.Rated7x7.battlesOnStunningVehicles = tankData.GetStaticSizeBlock("rated7x7").GetValue("battlesOnStunningVehicles");
            result.Rated7x7.stunNum = tankData.GetStaticSizeBlock("rated7x7").GetValue("stunNum");
            result.Rated7x7.damageAssistedStun = tankData.GetStaticSizeBlock("rated7x7").GetValue("damageAssistedStun");

            result.GlobalMapCommon.battlesOnStunningVehicles = tankData.GetStaticSizeBlock("globalMapCommon").GetValue("battlesOnStunningVehicles");
            result.GlobalMapCommon.stunNum = tankData.GetStaticSizeBlock("globalMapCommon").GetValue("stunNum");
            result.GlobalMapCommon.damageAssistedStun = tankData.GetStaticSizeBlock("globalMapCommon").GetValue("damageAssistedStun");

            result.Fallout.battlesOnStunningVehicles = tankData.GetStaticSizeBlock("fallout").GetValue("battlesOnStunningVehicles");
            result.Fallout.stunNum = tankData.GetStaticSizeBlock("fallout").GetValue("stunNum");
            result.Fallout.damageAssistedStun = tankData.GetStaticSizeBlock("fallout").GetValue("damageAssistedStun");

            return result;
        }

        /// <summary>
        /// 0.9.19 (96), 0.9.19.0.2 (97), 0.9.19.1 (98)
        /// </summary>
        /// <param name="tankData"></param>
        /// <returns></returns>
        private static TankJson Process97(DossierTankData tankData)
        {
            var result = Process95(tankData);

            #region Ranked
            result.Ranked = new StatisticJson()
            {
                xp = tankData.GetStaticSizeBlock("ranked").GetValue("xp"),
                battlesCount = tankData.GetStaticSizeBlock("ranked").GetValue("battlesCount"),
                wins = tankData.GetStaticSizeBlock("ranked").GetValue("wins"),
                losses = tankData.GetStaticSizeBlock("ranked").GetValue("losses"),
                survivedBattles = tankData.GetStaticSizeBlock("ranked").GetValue("survivedBattles"),
                frags = tankData.GetStaticSizeBlock("ranked").GetValue("frags"),
                shots = tankData.GetStaticSizeBlock("ranked").GetValue("shots"),
                hits = tankData.GetStaticSizeBlock("ranked").GetValue("directHits"),
                spotted = tankData.GetStaticSizeBlock("ranked").GetValue("spotted"),
                damageDealt = tankData.GetStaticSizeBlock("ranked").GetValue("damageDealt"),
                damageReceived = tankData.GetStaticSizeBlock("ranked").GetValue("damageReceived"),
                capturePoints = tankData.GetStaticSizeBlock("ranked").GetValue("capturePoints"),
                droppedCapturePoints = tankData.GetStaticSizeBlock("ranked").GetValue("droppedCapturePoints"),

                originalXP = tankData.GetStaticSizeBlock("ranked").GetValue("originalXP"),
                damageAssistedTrack = tankData.GetStaticSizeBlock("ranked").GetValue("damageAssistedTrack"),
                damageAssistedRadio = tankData.GetStaticSizeBlock("ranked").GetValue("damageAssistedRadio"),
                shotsReceived = tankData.GetStaticSizeBlock("ranked").GetValue("directHitsReceived"),
                noDamageShotsReceived = tankData.GetStaticSizeBlock("ranked").GetValue("noDamageDirectHitsReceived"),
                piercedReceived = tankData.GetStaticSizeBlock("ranked").GetValue("piercingsReceived"),
                heHitsReceived = tankData.GetStaticSizeBlock("ranked").GetValue("explosionHitsReceived"),
                he_hits = tankData.GetStaticSizeBlock("ranked").GetValue("explosionHits"),
                pierced = tankData.GetStaticSizeBlock("ranked").GetValue("piercings"),
                winAndSurvived = tankData.GetStaticSizeBlock("ranked").GetValue("winAndSurvived"),
                frags8p = tankData.GetStaticSizeBlock("ranked").GetValue("frags8p"),
                potentialDamageReceived = tankData.GetStaticSizeBlock("ranked").GetValue("potentialDamageReceived"),
                damageBlockedByArmor = tankData.GetStaticSizeBlock("ranked").GetValue("damageBlockedByArmor"),
                battlesOnStunningVehicles = tankData.GetStaticSizeBlock("ranked").GetValue("battlesOnStunningVehicles"),
                stunNum = tankData.GetStaticSizeBlock("ranked").GetValue("stunNum"),
                damageAssistedStun = tankData.GetStaticSizeBlock("ranked").GetValue("damageAssistedStun"),

                maxXP = tankData.GetStaticSizeBlock("maxFallout").GetValue("maxXP"),
                maxFrags = tankData.GetStaticSizeBlock("maxFallout").GetValue("maxFrags"),
                maxDamage = tankData.GetStaticSizeBlock("maxFallout").GetValue("maxDamage"),
            };
			#endregion

			//Ranked Seasons!!!!!
			result.AchievementsRanked = new AchievementsRanked
			{
				Rank = 0
			};

			return result;
        }

        /// <summary>
        /// 0.9.20, 0.9.20.1
        /// </summary>
        private static TankJson Process99(DossierTankData tankData)
        {
            var result = Process97(tankData);

			#region A30x30
			result.A30x30 = new StatisticJson()
            {
                xp = tankData.GetStaticSizeBlock("a30x30").GetValue("xp"),
                battlesCount = tankData.GetStaticSizeBlock("a30x30").GetValue("battlesCount"),
                wins = tankData.GetStaticSizeBlock("a30x30").GetValue("wins"),
                losses = tankData.GetStaticSizeBlock("a30x30").GetValue("losses"),
                survivedBattles = tankData.GetStaticSizeBlock("a30x30").GetValue("survivedBattles"),
                frags = tankData.GetStaticSizeBlock("a30x30").GetValue("frags"),
                shots = tankData.GetStaticSizeBlock("a30x30").GetValue("shots"),
                hits = tankData.GetStaticSizeBlock("a30x30").GetValue("directHits"),
                spotted = tankData.GetStaticSizeBlock("a30x30").GetValue("spotted"),
                damageDealt = tankData.GetStaticSizeBlock("a30x30").GetValue("damageDealt"),
                damageReceived = tankData.GetStaticSizeBlock("a30x30").GetValue("damageReceived"),
                capturePoints = tankData.GetStaticSizeBlock("a30x30").GetValue("capturePoints"),
                droppedCapturePoints = tankData.GetStaticSizeBlock("a30x30").GetValue("droppedCapturePoints"),

                originalXP = tankData.GetStaticSizeBlock("a30x30").GetValue("originalXP"),
                damageAssistedTrack = tankData.GetStaticSizeBlock("a30x30").GetValue("damageAssistedTrack"),
                damageAssistedRadio = tankData.GetStaticSizeBlock("a30x30").GetValue("damageAssistedRadio"),
                shotsReceived = tankData.GetStaticSizeBlock("a30x30").GetValue("directHitsReceived"),
                noDamageShotsReceived = tankData.GetStaticSizeBlock("a30x30").GetValue("noDamageDirectHitsReceived"),
                piercedReceived = tankData.GetStaticSizeBlock("a30x30").GetValue("piercingsReceived"),
                heHitsReceived = tankData.GetStaticSizeBlock("a30x30").GetValue("explosionHitsReceived"),
                he_hits = tankData.GetStaticSizeBlock("a30x30").GetValue("explosionHits"),
                pierced = tankData.GetStaticSizeBlock("a30x30").GetValue("piercings"),
                winAndSurvived = tankData.GetStaticSizeBlock("a30x30").GetValue("winAndSurvived"),
                frags8p = tankData.GetStaticSizeBlock("a30x30").GetValue("frags8p"),
                potentialDamageReceived = tankData.GetStaticSizeBlock("a30x30").GetValue("potentialDamageReceived"),
                damageBlockedByArmor = tankData.GetStaticSizeBlock("a30x30").GetValue("damageBlockedByArmor"),
                battlesOnStunningVehicles = tankData.GetStaticSizeBlock("a30x30").GetValue("battlesOnStunningVehicles"),
                stunNum = tankData.GetStaticSizeBlock("a30x30").GetValue("stunNum"),
                damageAssistedStun = tankData.GetStaticSizeBlock("a30x30").GetValue("damageAssistedStun"),

                maxXP = tankData.GetStaticSizeBlock("max30x30").GetValue("maxXP"),
                maxFrags = tankData.GetStaticSizeBlock("max30x30").GetValue("maxFrags"),
                maxDamage = tankData.GetStaticSizeBlock("max30x30").GetValue("maxDamage"),
            };
            #endregion
            return result;
        }
    }
}
