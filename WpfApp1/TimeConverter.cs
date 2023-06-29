using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WpfApp1
{
    public class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //throw new NotImplementedException();
            TimeSpan ts = (TimeSpan)value;
            //MessageBox.Show(ts.ToString());
            int temp = (int)ts.TotalMilliseconds;
            //MessageBox.Show(temp.ToString());
            return temp;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan temp = TimeSpan.FromMilliseconds((double)value);
            //MessageBox.Show(temp.ToString());
            return temp;
        }
    }

    public class SubConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double inputValue = (double)value;
            double subValue = double.Parse(parameter.ToString());

            return inputValue - subValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //public class IsPlayConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if ((bool)value == false) {
    //            return "播放";
    //        }
    //        else
    //        {
    //            return "暂停";
    //        }
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
