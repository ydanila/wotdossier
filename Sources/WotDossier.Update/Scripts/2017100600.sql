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
  Rank          int NOT NULL DEFAULT 0,
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
  BattlesOnStunningVehicles int NOT NULL default(0),
  DamageAssistedStun int NOT NULL default(0),
  StunNum int NOT NULL default(0),
  /* Foreign keys */
  CONSTRAINT Foreign_key01
    FOREIGN KEY (AchievementsId)
    REFERENCES RankedBattlesAchievements(Id), 
  CONSTRAINT Foreign_key02
    FOREIGN KEY (PlayerId)
    REFERENCES Player(Id)
);

CREATE TABLE TankGrandBattleBattlesStatistic (
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

CREATE TABLE GrandBattleBattlesStatistic (
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
  BattlesOnStunningVehicles int NOT NULL default(0),
  DamageAssistedStun int NOT NULL default(0),
  StunNum int NOT NULL default(0),
  /* Foreign keys */
  CONSTRAINT Foreign_key01
    FOREIGN KEY (AchievementsId)
    REFERENCES RandomBattlesAchievements(Id), 
  CONSTRAINT Foreign_key02
    FOREIGN KEY (PlayerId)
    REFERENCES Player(Id)
);