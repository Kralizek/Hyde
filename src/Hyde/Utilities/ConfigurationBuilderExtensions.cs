using Microsoft.Extensions.Configuration;

namespace Hyde.Utilities;

internal static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AddJsonFile(this IConfigurationBuilder builder, FileInfo file)
    {
        if (file.Exists)
        {
            using var fs = new FileStream(file.FullName, FileMode.Open);

            builder.AddJsonStream(fs);
        }

        return builder;
    }
}