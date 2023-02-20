using System.ComponentModel;
using Hyde.Utilities;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Hyde.Commands.Post;

public class CreatePostCommand : AsyncCommand<CreatePostCommand.CreatePostSettings>
{
    public class CreatePostSettings : PostSettings
    {
        [CommandArgument(0, "<TITLE>")]
        [Description("The title of the post")]
        public required string Title { get; init; }
        
        [CommandOption("-f|--filename")]
        [Description("The alphanumeric portion of the file name. Default: generated from the title.")]
        public string? FileName { get; init; }
        
        [CommandOption("--date")]
        [Description("The date the post is published")]
        public DateOnly PostDate { get; init; } = DateOnly.FromDateTime(DateTime.Today);
        
        [CommandOption("--time")]
        [Description("The time the post is published")]
        public TimeOnly PostTime { get; init; } = TimeOnly.FromDateTime(DateTime.Now);
        
        [CommandOption("--excerpt")]
        [Description("The excerpt of the post")]
        public string? Excerpt { get; init; }

        [CommandOption("--layout")]
        [Description("Sets the layout to be used when creating the post")]
        public string Layout { get; init; } = "post";
        
        [CommandOption("--draft")]
        [Description("Creates the post as draft")]
        public bool IsDraft { get; init; }
    }

    public override Task<int> ExecuteAsync(CommandContext context, CreatePostSettings settings)
    {
        SettingsDumper.Dump(settings);

        var filename = Path.Combine(settings.SiteDirectory.FullName, settings.IsDraft ? "_drafts" : "_posts", GetFileName(settings.PostDate, settings.Title));
        
        AnsiConsole.WriteLine($"Writing file {filename}");
        
        return Task.FromResult(0);
    }

    private static string GetFileName(DateOnly date, string title) => $"{date:yyyy-MM-dd}-{ToKebabCase(title)}.md";

    private static string ToKebabCase(string fileName) => fileName
        .ToLowerInvariant()
        .Replace(":", "")
        .Replace(",", "")
        .Replace(" ", "-");
}
