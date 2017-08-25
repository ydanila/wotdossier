ALTER TABLE [HistoricalBattlesStatistic] ADD [WN8KTTCRating] float NOT NULL default(0);
update [HistoricalBattlesStatistic] set [WN8KTTCRating] = [WN8Rating];

ALTER TABLE [HistoricalBattlesStatistic] ADD [WN8XVMRating] float NOT NULL default(0);
update [HistoricalBattlesStatistic] set [WN8XVMRating] = [WN8Rating];

ALTER TABLE [RandomBattlesStatistic] ADD [WN8KTTCRating] float NOT NULL default(0);
update [RandomBattlesStatistic] set [WN8KTTCRating] = [WN8Rating];

ALTER TABLE [RandomBattlesStatistic] ADD [WN8XVMRating] float NOT NULL default(0);
update [RandomBattlesStatistic] set [WN8XVMRating] = [WN8Rating];

ALTER TABLE [TeamBattlesStatistic] ADD [WN8KTTCRating] float NOT NULL default(0);
update [TeamBattlesStatistic] set [WN8KTTCRating] = [WN8Rating];

ALTER TABLE [TeamBattlesStatistic] ADD [WN8XVMRating] float NOT NULL default(0);
update [TeamBattlesStatistic] set [WN8XVMRating] = [WN8Rating];