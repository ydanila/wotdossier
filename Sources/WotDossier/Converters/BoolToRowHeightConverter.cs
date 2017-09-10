using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WotDossier.Converters
{
    /// <summary>
    /// Bool to RowDefinition height
    /// </summary>
    public class BoolToRowHeightConverter : IValueConverter
    {
        private static readonly BoolToRowHeightConverter _defaultInstance = new BoolToRowHeightConverter();

        /// <summary>
        /// Gets the default instance.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        public static BoolToRowHeightConverter Default { get { return _defaultInstance; } }

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
            return (bool)value ? new GridLength(0, GridUnitType.Auto) : new GridLength(0, GridUnitType.Pixel);
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