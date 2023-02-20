using System.ComponentModel;
using Hyde.Utilities;
using Spectre.Console.Cli;

namespace Hyde.Commands;

public class Settings : CommandSettings
{
    [CommandOption("-s|--site")]
    [TypeConverter(typeof(DirectoryInfoTypeConverter))]
    [Description("The path to the root of the site to work on. Default: working directory")]
    public DirectoryInfo SiteDirectory { get; set; } = new DirectoryInfo(Directory.GetCurrentDirectory());
}