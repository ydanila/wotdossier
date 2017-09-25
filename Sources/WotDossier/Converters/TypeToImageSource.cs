using System;
using System.Globalization;
using System.Windows.Data;
using WotDossier.Domain;

namespace WotDossier.Converters
{
    /// <summary>
    /// Converts tank type to image source
    /// </summary>
    public class TypeToImageSourceConverter : IValueConverter
    {
        private static TypeToImageSourceConverter _default = new TypeToImageSourceConverter();

        /// <summary>
        /// Gets the default instance.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        public static TypeToImageSourceConverter Default
        {
            get { return _default; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((TankType)value)
            {
                case TankType.LT:
                    return "pack://application:,,,/WotDossier.Resources;component/Images/Types/tank_type_light.png";

                case TankType.MT:
                    return "pack://application:,,,/WotDossier.Resources;component/Images/Types/tank_type_medium.png";

                case TankType.HT:
                    return "pack://application:,,,/WotDossier.Resources;component/Images/Types/tank_type_heavy.png";

                case TankType.SPG:
                    return "pack://application:,,,/WotDossier.Resources;component/Images/Types/tank_type_spg.png";

                case TankType.TD:
                    return "pack://application:,,,/WotDossier.Resources;component/Images/Types/tank_type_td.png";
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
