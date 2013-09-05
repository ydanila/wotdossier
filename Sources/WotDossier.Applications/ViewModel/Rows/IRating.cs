﻿namespace WotDossier.Applications.ViewModel.Rows
{
    public interface IRating
    {
        double EffRating { get; }
        double WN6Rating { get; }
        double KievArmorRating { get; }
        double XEFF { get; }
        double XWN { get; }
        double? PerformanceRating { get; }
    }
}