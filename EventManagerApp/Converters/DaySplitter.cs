using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace EventManagerApp.Converters
{
    [ValueConversion(typeof(DateTime), typeof(String))]
    public class DateSplitter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime) value;
            switch (parameter.ToString())
            {
                case "time":
                    return date.ToShortTimeString();
                case "day":
                    return date.Day.ToString();
                case "date":
                    return date.ToShortDateString();
                case "month":
                    string month = date.Month.ToString();
                    switch (month)
                    {
                        case "1":
                            return "JAN";
                        case "2":
                            return "FEB";
                        case "3":
                            return "MAR";
                        case "4":
                            return "APR";
                        case "5":
                            return "MAY";
                        case "6":
                            return "JUN";
                        case "7":
                            return "JUL";
                        case "8":
                            return "AUG";
                        case "9":
                            return "SEP";
                        case "10":
                            return "OCT";
                        case "11":
                            return "NOV";
                        case "12":
                            return "DEC";
                    }
                    return "NIL";
            }
            return "NIL";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}
