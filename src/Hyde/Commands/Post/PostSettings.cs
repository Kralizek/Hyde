using System.ComponentModel;
using Spectre.Console.Cli;

namespace Hyde.Commands.Post;

public class PostSettings : Settings
{
    [CommandOption("-d|--directory-name")]
    [DefaultValue("_posts")]
    public string PostDirectoryName { get; init; } = "_posts";
}