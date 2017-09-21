ALTER TABLE [HistoricalBattlesStatistic] ADD [BattlesOnStunningVehicles] int NOT NULL default(0);

ALTER TABLE [RandomBattlesStatistic] ADD [BattlesOnStunningVehicles] int NOT NULL default(0);

ALTER TABLE [TeamBattlesStatistic] ADD [BattlesOnStunningVehicles] int NOT NULL default(0);

ALTER TABLE [HistoricalBattlesStatistic] ADD [StunNum] int NOT NULL default(0);

ALTER TABLE [RandomBattlesStatistic] ADD [StunNum] int NOT NULL default(0);

ALTER TABLE [TeamBattlesStatistic] ADD [StunNum] int NOT NULL default(0);

ALTER TABLE [HistoricalBattlesStatistic] ADD [DamageAssistedStun] int NOT NULL default(0);

ALTER TABLE [RandomBattlesStatistic] ADD [DamageAssistedStun] int NOT NULL default(0);

ALTER TABLE [TeamBattlesStatistic] ADD [DamageAssistedStun] int NOT NULL default(0);

ALTER TABLE [RandomBattlesAchievements] ADD [Aimer] int NOT NULL default(0);