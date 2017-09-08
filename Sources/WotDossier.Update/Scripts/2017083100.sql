﻿CREATE TABLE TankRankedBattlesStatistic (
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
    REFERENCES RandomBattlesAchievements(Id), 
  CONSTRAINT Foreign_key02
    FOREIGN KEY (PlayerId)
    REFERENCES Player(Id)
);

ALTER TABLE [HistoricalBattlesStatistic] ADD [BattlesOnStunningVehicles] int NOT NULL default(0);

ALTER TABLE [RandomBattlesStatistic] ADD [BattlesOnStunningVehicles] int NOT NULL default(0);

ALTER TABLE [TeamBattlesStatistic] ADD [BattlesOnStunningVehicles] int NOT NULL default(0);

ALTER TABLE [RankedBattlesStatistic] ADD [BattlesOnStunningVehicles] int NOT NULL default(0);

ALTER TABLE [GeneralBattlesStatistic] ADD [BattlesOnStunningVehicles] int NOT NULL default(0);

ALTER TABLE [HistoricalBattlesStatistic] ADD [StunNum] int NOT NULL default(0);

ALTER TABLE [RandomBattlesStatistic] ADD [StunNum] int NOT NULL default(0);

ALTER TABLE [TeamBattlesStatistic] ADD [StunNum] int NOT NULL default(0);

ALTER TABLE [RankedBattlesStatistic] ADD [StunNum] int NOT NULL default(0);

ALTER TABLE [GeneralBattlesStatistic] ADD [StunNum] int NOT NULL default(0);

ALTER TABLE [HistoricalBattlesStatistic] ADD [DamageAssistedStun] int NOT NULL default(0);

ALTER TABLE [RandomBattlesStatistic] ADD [DamageAssistedStun] int NOT NULL default(0);

ALTER TABLE [TeamBattlesStatistic] ADD [DamageAssistedStun] int NOT NULL default(0);

ALTER TABLE [RankedBattlesStatistic] ADD [DamageAssistedStun] int NOT NULL default(0);

ALTER TABLE [GeneralBattlesStatistic] ADD [DamageAssistedStun] int NOT NULL default(0);

ALTER TABLE [RandomBattlesAchievements] ADD [Aimer] int NOT NULL default(0);