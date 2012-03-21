using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows;

namespace EventManagerApp.Converters
{
    [ValueConversion(typeof(DateTime), typeof(DateTime))]
    public class FormDateTimeConverter : DependencyObject, IValueConverter
    {
        public static DependencyProperty SourceValueProperty =
            DependencyProperty.Register("SourceValue", typeof(DateTime), typeof(FormDateTimeConverter));

        public DateTime SourceValue
        {
            get { return (DateTime)this.GetValue(SourceValueProperty); }
            set { this.SetValue(SourceValueProperty, value); }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;
            DateTime result = DateTime.Now;

            switch (parameter.ToString())
            {
                case "Hour":
                    result = new DateTime().AddHours(date.Hour);
                    break;

                case "Date":
                    break;
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;
            DateTime result = this.SourceValue;

            switch (parameter.ToString())
            {
                case "Hour":
                    result = this.SourceValue.Add(date.Subtract(new DateTime()));
                    break;

                case "Date":
                    break;
            }
            return result;
        }
    }
}
