using System;
using System.Globalization;
using System.Windows.Data;

namespace WotDossier.Converters
{
    /// <summary>
    /// Resource Id to localized string converter
    /// </summary>
    public class ResourceMultiValueFormatConverter : IMultiValueConverter
    {
        private static readonly ResourceMultiValueFormatConverter _defaultInstance = new ResourceMultiValueFormatConverter();

        /// <summary>
        /// Gets the default instance.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        public static ResourceMultiValueFormatConverter Default { get { return _defaultInstance; } }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The array of values produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null)
            {
                return string.Format(parameter.ToString(), value);
            }
            return string.Format("{0}", value);
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The types to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted array of values. If the method returns null, the valid null value is used.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
