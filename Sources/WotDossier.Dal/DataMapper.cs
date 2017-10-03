using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using WotDossier.Common;
using WotDossier.Domain;
using WotDossier.Domain.Server;
using WotDossier.Domain.Tank;

namespace WotDossier.Dal
{
    public class DataMapper
    {
        /// <summary>
        /// Converts the specified tank json from WOT API.
        /// </summary>
        /// <param name="vehicle">The tank json.</param>
        /// <returns></returns>
        public static TankJson Map(Vehicle vehicle)
        {
            TankJson v2 = new TankJson();

            //statistic 15x15
            v2.A15x15 = new StatisticJson
            {
                battlesCount = vehicle.all.battles,
                battlesCountBefore8_8 = vehicle.all.battles,
                capturePoints = vehicle.all.capture_points,
                damageDealt = vehicle.all.damage_dealt,
                damageReceived = vehicle.all.damage_received,
                droppedCapturePoints = vehicle.all.dropped_capture_points,
                frags = vehicle.all.frags,
                frags8p = vehicle.all.frags,
                hits = vehicle.all.hits,
                losses = vehicle.all.losses,
                shots = vehicle.all.shots,
                spotted = vehicle.all.spotted,
                survivedBattles = vehicle.all.survived_battles,
                winAndSurvived = vehicle.all.survived_battles,
                wins = vehicle.all.wins,
                xp = vehicle.all.xp,
                xpBefore8_8 = vehicle.all.xp,
                originalXP = 0,
                damageAssistedRadio = 0,
                damageAssistedTrack = 0,
                shotsReceived = 0,
                noDamageShotsReceived = 0,
                piercedReceived = 0,
                heHitsReceived = 0,
                he_hits = 0,
                pierced = 0,
                maxFrags = vehicle.max_frags,
                maxXP = vehicle.max_xp
            };

            //v2.A15x15.maxDamage = tankJson;

            v2.FragsList = new List<IList<string>>();

            //achievements 15x15
            v2.Achievements = new AchievementsJson();
            //v2.Achievements.armorPiercer = tankJson.series.master_gunner;
            //v2.Achievements.battleHeroes = tankJson.awards.battle_hero;
            //v2.Achievements.beasthunter = tankJson.awards.hunter;
            //v2.Achievements.bombardier = tankJson.awards.bombardier;
            //v2.Achievements.defender = tankJson.awards.defender;
            //v2.Achievements.diehard = tankJson.series.survivor;
            //v2.Achievements.diehardSeries = tankJson.series.survivor_progress;
            //v2.Achievements.evileye = tankJson.awards.patrol_duty;
            //v2.Achievements.fragsBeast = tankJson.amounts.beast_frags;
            //v2.Achievements.fragsPatton = tankJson.amounts.patton_frags;
            //v2.Achievements.fragsSinai = tankJson.amounts.sinai_frags;
            //v2.Achievements.handOfDeath = tankJson.awards.reaper;
            //v2.Achievements.heroesOfRassenay = tankJson.epics.heroes_of_raseiniai;
            //v2.Achievements.huntsman = tankJson.awards.ranger;
            //v2.Achievements.invader = tankJson.awards.invader;
            //v2.Achievements.invincible = tankJson.series.invincible;
            //v2.Achievements.invincibleSeries = tankJson.series.invincible_progress;
            //v2.Achievements.ironMan = tankJson.awards.cool_headed;
            //v2.Achievements.kamikaze = tankJson.awards.kamikaze;
            //v2.Achievements.killingSeries = tankJson.series.reaper_progress;
            //v2.Achievements.luckyDevil = tankJson.awards.lucky_devil;
            //v2.Achievements.lumberjack = 0;
            //v2.Achievements.maxDiehardSeries = tankJson.series.survivor;
            //v2.Achievements.maxInvincibleSeries = tankJson.series.invincible_progress;
            //v2.Achievements.maxKillingSeries = tankJson.series.reaper;
            //v2.Achievements.maxPiercingSeries = tankJson.series.master_gunner;
            //v2.Achievements.maxSniperSeries = tankJson.series.sharpshooter;
            //v2.Achievements.medalAbrams = tankJson.medals.abrams;
            //v2.Achievements.medalBillotte = tankJson.epics.billotte;
            //v2.Achievements.medalBrothersInArms = tankJson.awards.brothers_in_arms;
            //v2.Achievements.medalBrunoPietro = tankJson.epics.bruno_pietro;
            //v2.Achievements.medalBurda = tankJson.epics.burda;
            //v2.Achievements.medalCarius = tankJson.medals.carius;
            //v2.Achievements.medalCrucialContribution = tankJson.awards.crucial_contribution;
            //v2.Achievements.medalDeLanglade = tankJson.epics.de_langlade;
            //v2.Achievements.medalDumitru = tankJson.epics.dumitru;
            //v2.Achievements.medalEkins = tankJson.medals.ekins;
            //v2.Achievements.medalFadin = tankJson.epics.fadin;
            //v2.Achievements.medalHalonen = tankJson.epics.halonen;
            //v2.Achievements.medalKay = tankJson.medals.kay;
            //v2.Achievements.medalKnispel = tankJson.medals.knispel;
            //v2.Achievements.medalKolobanov = tankJson.epics.kolobanov;
            //v2.Achievements.medalLafayettePool = tankJson.epics.lafayette_pool;
            //v2.Achievements.medalLavrinenko = tankJson.medals.lavrinenko;
            //v2.Achievements.medalLeClerc = tankJson.medals.leclerk;
            //v2.Achievements.medalLehvaslaiho = tankJson.epics.lehvaslaiho;
            //v2.Achievements.medalNikolas = tankJson.epics.nikolas;
            //v2.Achievements.medalOrlik = tankJson.epics.orlik;
            //v2.Achievements.medalOskin = tankJson.epics.oskin;
            //v2.Achievements.medalPascucci = tankJson.epics.pascucci;
            //v2.Achievements.medalPoppel = tankJson.medals.poppel;
            //v2.Achievements.medalRadleyWalters = tankJson.epics.radley_walters;
            //v2.Achievements.medalTamadaYoshio = tankJson.epics.tamada_yoshio;
            //v2.Achievements.medalTarczay = tankJson.epics.tarczay;
            //v2.Achievements.medalWittmann = tankJson.epics.boelter;
            //v2.Achievements.mousebane = tankJson.awards.mouse_trap;
            //v2.Achievements.pattonValley = tankJson.awards.patton_valley;
            //v2.Achievements.piercingSeries = tankJson.series.master_gunner_progress;
            //v2.Achievements.raider = tankJson.awards.raider;
            //v2.Achievements.scout = tankJson.awards.scout;
            //v2.Achievements.sinai = tankJson.awards.sinai;
            //v2.Achievements.sniper = tankJson.awards.sniper;
            //v2.Achievements.sniperSeries = tankJson.series.sharpshooter_progress;
            //v2.Achievements.steelwall = tankJson.awards.steel_wall;
            //v2.Achievements.sturdy = tankJson.awards.spartan;
            //v2.Achievements.supporter = tankJson.awards.confederate;
            //v2.Achievements.tankExpertStrg = 0;
            //v2.Achievements.titleSniper = tankJson.series.sharpshooter;
            //v2.Achievements.warrior = tankJson.awards.top_gun;
            v2.Achievements.MarksOnGun = vehicle.achievements.marksOnGun;
            v2.Achievements.MarkOfMastery = vehicle.mark_of_mastery;

            v2.Common = new CommonJson
            {
                basedonversion = 65,
                compactDescr = 0,
                countryid = vehicle.description.CountryId,
                creationTime = 0,
                creationTimeR = DateTime.MinValue,
                frags = vehicle.all.frags,
                frags_compare = 0,
                has_15x15 = 1,
                has_7x7 = 0,
                has_clan = 0,
                has_company = 0,
                tankid = vehicle.description.TankId,
                premium = Dictionaries.Instance.Tanks[v2.UniqueId()].Premium,
                tanktitle = Dictionaries.Instance.Tanks[v2.UniqueId()].Title,
                tier = Dictionaries.Instance.Tanks[v2.UniqueId()].Tier,
                type = Dictionaries.Instance.Tanks[v2.UniqueId()].Type,
                mileage = 0
            };
            //v2.Common.lastBattleTime = tankJson.last_time_played;
            //v2.Common.lastBattleTimeR = Utils.UnixDateToDateTime(tankJson.last_time_played);
            //v2.Common.updated = tankJson.updated;
            //v2.Common.updatedR = Utils.UnixDateToDateTime(tankJson.updated);
            //v2.Common.battleLifeTime = tankJson.play_time;
            //v2.Common.treesCut = tankJson.amounts.trees_knocked_down;
            v2.Description = vehicle.description;
            return v2;
        }

    }
}
