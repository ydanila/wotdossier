﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WotDossier.Domain.Settings;

namespace WotDossier.Domain
{
    /// <summary>
    /// Application settings
    /// </summary>
    public class AppSettings : AppSettingsBase
    {
        private string _replaysUploadServerPath = "http://wotreplays.ru/site/upload";
        private bool _checkForUpdates = true;

        private TankFilterSettings _tankFilterSettings = new TankFilterSettings();
        private PeriodSettings _periodSettings = new PeriodSettings();

        /// <summary>
        /// Gets or sets the name of the player.
        /// </summary>
        /// <value>
        /// The name of the player.
        /// </value>
        public string PlayerName { get; set; }

        /// <summary>
        /// Gets or sets the player id.
        /// </summary>
        /// <value>
        /// The player id.
        /// </value>
        public int PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the replays upload server path.
        /// </summary>
        /// <value>
        /// The replays upload server path.
        /// </value>
        public string ReplaysUploadServerPath
        {
            get { return _replaysUploadServerPath; }
            set { _replaysUploadServerPath = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [check for updates].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [check for updates]; otherwise, <c>false</c>.
        /// </value>
        public bool CheckForUpdates
        {
            get { return _checkForUpdates; }
            set { _checkForUpdates = value; }
        }

        /// <summary>
        /// Gets or sets the new version check last date.
        /// </summary>
        /// <value>
        /// The new version check last date.
        /// </value>
        public DateTime NewVersionCheckLastDate { get; set; }

        /// <summary>
        /// Gets or sets the tank filter settings.
        /// </summary>
        /// <value>
        /// The tank filter settings.
        /// </value>
        public TankFilterSettings TankFilterSettings
        {
            get { return _tankFilterSettings; }
            set { _tankFilterSettings = value; }
        }

        /// <summary>
        /// Gets or sets the period settings.
        /// </summary>
        /// <value>
        /// The period settings.
        /// </value>
        public PeriodSettings PeriodSettings
        {
            get { return _periodSettings; }
            set { _periodSettings = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [automatic load statistic].
        /// </summary>
        /// <value>
        /// <c>true</c> if [automatic load statistic]; otherwise, <c>false</c>.
        /// </value>
        public bool AutoLoadStatistic { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show extended replays data].
        /// </summary>
        /// <value>
        /// <c>true</c> if [show extended replays data]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowExtendedReplaysData { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to try to find tank analog in tanks ratings.
        /// </summary>
        /// <value>
        /// <c>true</c> if try to find tank analog; otherwise, <c>false</c>.
        /// </value>
        public bool TryFindTankAnalog { get; set; } = true;

        private bool _useIncompleteReplaysResultsForCharts = false;
        
        public bool UseIncompleteReplaysResultsForCharts
        {
            get { return _useIncompleteReplaysResultsForCharts; }
            set { _useIncompleteReplaysResultsForCharts = value; }
        }

        private List<ReplayPlayer> _replayPlayers;
        public List<ReplayPlayer> ReplayPlayers
        {
            get
            {
                if (_replayPlayers == null)
                {
                    _replayPlayers = new List<ReplayPlayer>();
                }
                return _replayPlayers ;
            }
            set { _replayPlayers = value; }
        }

        public string ExternalDataVersion { get; set; }

        public String ColumnInfo { get; set; }
        public string DossierCachePath { get; set; }

        public string WotFolderPath { get; set; }

        public int WindowState { get; set; }
    }
}
