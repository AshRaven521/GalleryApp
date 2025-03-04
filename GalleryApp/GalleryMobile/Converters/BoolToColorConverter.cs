using System.Globalization;

namespace GalleryMobile.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        private readonly string redColor = "#FF0000";
        private readonly string whiteColor = "#FFFFFF";
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            /* NOTE: will use to convert is liked property to color (if is liked color red, else white) */
            if ((bool)value)
            {
                return new SolidColorBrush(Color.FromArgb(redColor));
                //return Color.FromArgb(redColor);
            }
            return new SolidColorBrush(Color.FromArgb(whiteColor));
            //return Color.FromArgb(whiteColor);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var color = (SolidColorBrush)value;
            string colorHex = color.Color.ToArgbHex();
            if (colorHex == redColor)
            {
                return true;
            }
            return false;
            //throw new NotImplementedException();
        }
    }
}
