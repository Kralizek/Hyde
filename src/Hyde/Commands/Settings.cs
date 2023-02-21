using System.ComponentModel;
using Hyde.Utilities;
using Spectre.Console.Cli;

namespace Hyde.Commands;

public class Settings : CommandSettings
{
    private readonly DirectoryInfo _siteDirectory = null!;

    public Settings()
    {
        SiteDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
    }

    [CommandOption("-s|--site")]
    [TypeConverter(typeof(DirectoryInfoTypeConverter))]
    [Description("The path to the root of the site to work on. Default: working directory")]
    public DirectoryInfo SiteDirectory
    {
        get => _siteDirectory; 
        init
        {
            var siteRoot = FindSiteRoot(value);

            if (siteRoot is null)
            {
                _siteDirectory = value;
                IsInJekyll = false;
            }
            else
            {
                _siteDirectory = siteRoot;
                IsInJekyll = true;
            }
        }
    }
    
    public bool IsInJekyll { get; private init; }

    private static DirectoryInfo? FindSiteRoot(DirectoryInfo directory)
    {
        var iterator = directory;

        while (iterator != null)
        {
            var configFile = new FileInfo(Path.Combine(iterator.FullName, "_config.yml"));

            if (configFile.Exists)
            {
                return iterator;
            }

            iterator = iterator.Parent;
        }

        return null;
    }
}