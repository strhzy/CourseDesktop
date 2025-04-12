using System;
using System.Globalization;
using System.Windows.Data;

namespace DnDPartyManager.S
{
    public class AbilityScoreToModifierConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int score)
            {
                int modifier = (score - 10) / 2;
                return modifier >= 0 ? $"+{modifier}" : modifier.ToString();
            }
            return "+0";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}