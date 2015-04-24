﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using WotDossier.Applications.Logic;

namespace WotDossier.Converters.Color
{
    /// <summary>
    /// Converts Effectivity Rating value to <see cref="SolidColorBrush"/>
    /// </summary>
    public class EffectivityToColorConverter : IValueConverter
    {
        private static readonly EffectivityToColorConverter _defaultInstance = new EffectivityToColorConverter();

        /// <summary>
        /// Gets the default instance.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        public static EffectivityToColorConverter Default { get { return _defaultInstance; } }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double? eff = value as double?;
            if (eff != null)
            {
                if (eff >= Constants.Rating.EFF_P5)
                    return EffRangeBrushes.Purple;
                if (eff >= Constants.Rating.EFF_P4)
                    return EffRangeBrushes.Blue;
                if (eff >= Constants.Rating.EFF_P3)
                    return EffRangeBrushes.Green;
                if (eff >= Constants.Rating.EFF_P2)
                    return EffRangeBrushes.Yellow;
                if (eff >= Constants.Rating.EFF_P1)
                    return EffRangeBrushes.Orange;
            }
            return EffRangeBrushes.Red;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
