using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WotDossier.Converters.Color
{
    /// <summary>
    /// Statistic value change delta to color converter
    /// </summary>
    public class NegativeToColorConverter : IValueConverter
    {
	    /// <summary>
        /// Gets the default instance.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        public static NegativeToColorConverter Default { get; } = new NegativeToColorConverter();

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
            if (value == null)
            {
                return null;
            }

            double delta;
            if(value is int)
            {
                delta = (int)value;
            }
            else
            {
                delta = (double)value;
            }
            var negativBetter = parameter != null && bool.Parse(parameter.ToString());
            if ((delta > 0.0 && !negativBetter)
                || (delta < 0.0 && negativBetter) || (Math.Abs(delta - 0.0) < 0.001))
				return new SolidColorBrush(System.Windows.Media.Color.FromRgb(0xBA, 0xBF, 0xBA));
			return Brushes.Red;
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
