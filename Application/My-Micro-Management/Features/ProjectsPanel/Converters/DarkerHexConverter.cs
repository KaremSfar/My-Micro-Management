using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Micro_Management.Features.ProjectsPanel.Converters
{
    public class DarkerHexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GetDarkerShade(value as string, 0.2);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public string GetDarkerShade(string hexColor, double percentage)
        {
            // Convert the hex color to RGB
            int r = int.Parse(hexColor.Substring(1, 2), NumberStyles.HexNumber);
            int g = int.Parse(hexColor.Substring(3, 2), NumberStyles.HexNumber);
            int b = int.Parse(hexColor.Substring(5, 2), NumberStyles.HexNumber);

            // Calculate the new RGB values for the darker shade
            r = (int)Math.Round(r * (1 - percentage));
            g = (int)Math.Round(g * (1 - percentage));
            b = (int)Math.Round(b * (1 - percentage));

            // Convert the RGB values back to hex
            string darkHex = "#" + r.ToString("X2") + g.ToString("X2") + b.ToString("X2");

            return darkHex;
        }
    }
}
