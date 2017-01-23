using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace UML.GUI
{
    class AttributeToStringConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string output = "+";
            if ((bool)values[1])
            {
                output = "-";
            }

            output = output + " " + values[0];
            string typeString = (string)values[2];
            if( typeString != null &&!typeString.Equals( "" ) )
            {
                output = output + " : " + values[2];
            }

            return output;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
