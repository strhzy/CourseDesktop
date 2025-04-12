using System.Globalization;
using System.Windows.Data;

namespace DnDPartyManager.S
{
    public class NullToEmptyUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value as Uri ?? new Uri("pack://application:,,,/V/UserControls/Character.xaml");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}