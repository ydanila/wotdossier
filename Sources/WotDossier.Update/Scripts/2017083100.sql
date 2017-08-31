CREATE TABLE TankRankedBattlesStatistic (
  Id            integer NOT NULL PRIMARY KEY,
  UId           nvarchar(36),
  TankId        int NOT NULL,
  TankUId       nvarchar(36),
  BattlesCount  int NOT NULL DEFAULT 0,
  Updated       datetime NOT NULL,
  Version       int NOT NULL,
  Rev           int NOT NULL DEFAULT 2017083100,
  Raw           image NOT NULL,
  /* Foreign keys */
  CONSTRAINT Foreign_key01
    FOREIGN KEY (TankId)
    REFERENCES Tank(Id)
);

CREATE TABLE RankedBattlesAchievements (
  Id                   integer NOT NULL PRIMARY KEY,
  UId                  nvarchar(36),
  Season1Rank          int NOT NULL DEFAULT 0,
  Rev                  int NOT NULL DEFAULT 2017083100
);

CREATE TABLE RankedBattlesStatistic (
  Id                    integer NOT NULL PRIMARY KEY,
  UId                   nvarchar(36),
  PlayerId              int NOT NULL,
  PlayerUId             nvarchar(36),
  Updated               datetime NOT NULL,
  Wins                  int NOT NULL,
  Losses                int NOT NULL,
  SurvivedBattles       int NOT NULL,
  Xp                    int NOT NULL,
  BattleAvgXp           float,
  MaxXp                 int NOT NULL,
  Frags                 int NOT NULL,
  Spotted               int NOT NULL,
  HitsPercents          float,
  DamageDealt           int NOT NULL,
  CapturePoints         int NOT NULL,
  DroppedCapturePoints  int NOT NULL,
  BattlesCount          int NOT NULL,
  AvgLevel              float NOT NULL,
  AchievementsId        int,
  AchievementsUId       nvarchar(36),
  RBR                   float NOT NULL DEFAULT 0,
  WN8Rating             float NOT NULL DEFAULT 0,
  PerformanceRating     float NOT NULL DEFAULT 0,
  DamageTaken           int NOT NULL DEFAULT 0,
  MaxFrags              int NOT NULL DEFAULT 0,
  MaxDamage             int NOT NULL DEFAULT 0,
  MarkOfMastery         int NOT NULL DEFAULT 0,
  Rev                   int NOT NULL DEFAULT 2017083100,
  WN8KTTCRating         float NOT NULL DEFAULT 0,
  WN8XVMRating          float NOT NULL DEFAULT 0,
  /* Foreign keys */
  CONSTRAINT Foreign_key01
    FOREIGN KEY (AchievementsId)
    REFERENCES RankedBattlesAchievements(Id), 
  CONSTRAINT Foreign_key02
    FOREIGN KEY (PlayerId)
    REFERENCES Player(Id)
);

CREATE TABLE TankGeneralBattlesStatistic (
  Id            integer NOT NULL PRIMARY KEY,
  UId           nvarchar(36),
  TankId        int NOT NULL,
  TankUId       nvarchar(36),
  BattlesCount  int NOT NULL DEFAULT 0,
  Updated       datetime NOT NULL,
  Version       int NOT NULL,
  Rev           int NOT NULL DEFAULT 2017083100,
  Raw           image NOT NULL,
  /* Foreign keys */
  CONSTRAINT Foreign_key01
    FOREIGN KEY (TankId)
    REFERENCES Tank(Id)
);

CREATE TABLE GeneralBattlesAchievements (
  Id                   integer NOT NULL PRIMARY KEY,
  UId                  nvarchar(36),
  Warrior              int NOT NULL,
  Sniper               int NOT NULL,
  Invader              int NOT NULL,
  Defender             int NOT NULL,
  SteelWall            int NOT NULL,
  Confederate          int NOT NULL,
  Scout                int NOT NULL,
  PatrolDuty           int NOT NULL,
  HeroesOfRassenay     int NOT NULL,
  LafayettePool        int NOT NULL,
  RadleyWalters        int NOT NULL,
  CrucialContribution  int NOT NULL,
  BrothersInArms       int NOT NULL,
  Kolobanov            int NOT NULL,
  Nikolas              int NOT NULL,
  Orlik                int NOT NULL,
  Oskin                int NOT NULL,
  Halonen              int NOT NULL,
  Lehvaslaiho          int NOT NULL,
  DeLanglade           int NOT NULL,
  Burda                int NOT NULL,
  Dumitru              int NOT NULL,
  Pascucci             int NOT NULL,
  TamadaYoshio         int NOT NULL,
  Boelter              int NOT NULL,
  Fadin                int NOT NULL,
  Tarczay              int NOT NULL,
  BrunoPietro          int NOT NULL,
  Billotte             int NOT NULL,
  Survivor             int NOT NULL,
  Kamikaze             int NOT NULL,
  Invincible           int NOT NULL,
  Raider               int NOT NULL,
  Bombardier           int NOT NULL,
  Reaper               int NOT NULL,
  MouseTrap            int NOT NULL,
  PattonValley         int NOT NULL,
  Hunter               int NOT NULL,
  Sinai                int NOT NULL,
  MasterGunnerLongest  int NOT NULL,
  SharpshooterLongest  int NOT NULL,
  Ranger               int NOT NULL,
  CoolHeaded           int NOT NULL,
  Spartan              int NOT NULL,
  LuckyDevil           int NOT NULL,
  Kay                  int NOT NULL,
  Carius               int NOT NULL,
  Knispel              int NOT NULL,
  Poppel               int NOT NULL,
  Abrams               int NOT NULL,
  Leclerk              int NOT NULL,
  Lavrinenko           int NOT NULL,
  Ekins                int NOT NULL,
  Sniper2              int NOT NULL DEFAULT 0,
  MainGun              int NOT NULL DEFAULT 0,
  MarksOnGun           int NOT NULL DEFAULT 0,
  MovingAvgDamage      int NOT NULL DEFAULT 0,
  MedalMonolith        int NOT NULL DEFAULT 0,
  MedalAntiSpgFire     int NOT NULL DEFAULT 0,
  MedalGore            int NOT NULL DEFAULT 0,
  MedalCoolBlood       int NOT NULL DEFAULT 0,
  MedalStark           int NOT NULL DEFAULT 0,
  Impenetrable         int NOT NULL DEFAULT 0,
  MaxAimerSeries       int NOT NULL DEFAULT 0,
  ShootToKill          int NOT NULL DEFAULT 0,
  Fighter              int NOT NULL DEFAULT 0,
  Duelist              int NOT NULL DEFAULT 0,
  Demolition           int NOT NULL DEFAULT 0,
  Arsonist             int NOT NULL DEFAULT 0,
  Bonecrusher          int NOT NULL DEFAULT 0,
  Charmed              int NOT NULL DEFAULT 0,
  Even                 int NOT NULL DEFAULT 0,
  Rev                  int NOT NULL DEFAULT 2017083100
);

CREATE TABLE GeneralBattlesStatistic (
  Id                    integer NOT NULL PRIMARY KEY,
  UId                   nvarchar(36),
  PlayerId              int NOT NULL,
  PlayerUId             nvarchar(36),
  Updated               datetime NOT NULL,
  Wins                  int NOT NULL,
  Losses                int NOT NULL,
  SurvivedBattles       int NOT NULL,
  Xp                    int NOT NULL,
  BattleAvgXp           float,
  MaxXp                 int NOT NULL,
  Frags                 int NOT NULL,
  Spotted               int NOT NULL,
  HitsPercents          float,
  DamageDealt           int NOT NULL,
  CapturePoints         int NOT NULL,
  DroppedCapturePoints  int NOT NULL,
  BattlesCount          int NOT NULL,
  AvgLevel              float NOT NULL,
  AchievementsId        int,
  AchievementsUId       nvarchar(36),
  RBR                   float NOT NULL DEFAULT 0,
  WN8Rating             float NOT NULL DEFAULT 0,
  PerformanceRating     float NOT NULL DEFAULT 0,
  DamageTaken           int NOT NULL DEFAULT 0,
  MaxFrags              int NOT NULL DEFAULT 0,
  MaxDamage             int NOT NULL DEFAULT 0,
  MarkOfMastery         int NOT NULL DEFAULT 0,
  Rev                   int NOT NULL DEFAULT 2017083100,
  WN8KTTCRating         float NOT NULL DEFAULT 0,
  WN8XVMRating          float NOT NULL DEFAULT 0,
  /* Foreign keys */
  CONSTRAINT Foreign_key01
    FOREIGN KEY (AchievementsId)
    REFERENCES GeneralBattlesAchievements(Id), 
  CONSTRAINT Foreign_key02
    FOREIGN KEY (PlayerId)
    REFERENCES Player(Id)
);