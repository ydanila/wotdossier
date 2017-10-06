using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using WotDossier.Resources;

namespace WotDossier.Converters
{
    public class ImageSourceConverter : IValueConverter
    {
        private static readonly ImageSourceConverter _default = new ImageSourceConverter();

        /// <summary>
        /// Gets the default.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        public static ImageSourceConverter Default
        {
            get { return _default; }
        }

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value produced by the binding source.</param><param name="targetType">The type of the binding target property.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var baseUri = (string)value;

	        if (baseUri.Contains(" "))
	        {
		        var parts = baseUri.Split(' ');
		        Point location;
		        Size size = new Size();
		        if (parts.Length > 3)
		        {
			        location = new Point(System.Convert.ToInt32(parts[1]), System.Convert.ToInt32(parts[2]));
					size = new Size(System.Convert.ToInt32(parts[3]), System.Convert.ToInt32(parts[4]));
			        return ImageCache.GetBitmapImage(parts[0], location, size);
		        }
		        if (parts.Length  > 1)
		        {
					location = new Point(System.Convert.ToInt32(parts[1]), System.Convert.ToInt32(parts[2]));
			        if (parameter != null)
			        {
				        size = new Size(System.Convert.ToInt32(parameter), System.Convert.ToInt32(parameter));
				        return ImageCache.GetBitmapImage(parts[0], location, size);
					}
			        return ImageCache.GetBitmapImage(parts[0]);
				}
			}
			return ImageCache.GetBitmapImage(baseUri);
        }

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value that is produced by the binding target.</param><param name="targetType">The type to convert to.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
