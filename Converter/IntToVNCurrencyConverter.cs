using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _21120127_Week04.Converter
{
    class IntToVNCurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                int number = (int)value;
                CultureInfo vietnameseCulture = new CultureInfo("vi-VN");
                string formattedAmount = number.ToString("C0", vietnameseCulture);
                return formattedAmount;
            }
            catch (Exception)
            {
                return "Na";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            string amount = value as string;
            int number;
            if (int.TryParse(amount, NumberStyles.Currency, new CultureInfo("vi-VN"), out number))
            {
                return number;
            }
            return 0;
        }
    }
}
