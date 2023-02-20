using System.ComponentModel;
using System.Globalization;

namespace Hyde.Utilities;

public class DirectoryInfoTypeConverter : TypeConverter
{
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is string stringValue)
        {
            var directoryInfo = new DirectoryInfo(stringValue);

            if (!directoryInfo.Exists)
            {
                throw new InvalidOperationException();
            }

            return new DirectoryInfo(directoryInfo.FullName);
        }

        throw new NotSupportedException();
    }
}