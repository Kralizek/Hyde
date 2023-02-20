using System.ComponentModel;
using Hyde.Utilities;
using Spectre.Console.Cli;

namespace Hyde.Commands;

public class Settings : CommandSettings
{
    [CommandOption("-d|--directory")]
    [TypeConverter(typeof(DirectoryInfoTypeConverter))]
    public DirectoryInfo CurrentDirectory { get; set; } = new DirectoryInfo(Directory.GetCurrentDirectory());
}