using System.ComponentModel;
using Hyde.Utilities;
using Spectre.Console;
using Spectre.Console.Cli;
using YamlDotNet.Serialization;

namespace Hyde.Commands.Post;

public class CreatePostCommand : AsyncCommand<CreatePostCommand.CreatePostSettings>
{
    private readonly ISerializer _serializer;
    private readonly IFileNameGenerator _fileNameGenerator;

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

    public CreatePostCommand(ISerializer serializer, IFileNameGenerator fileNameGenerator)
    {
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        _fileNameGenerator = fileNameGenerator ?? throw new ArgumentNullException(nameof(fileNameGenerator));
    }

    public override ValidationResult Validate(CommandContext context, CreatePostSettings settings)
    {
        if (!settings.IsInJekyll)
        {
            return ValidationResult.Error("This command can only be executed while in a Jekyll site");
        }
        
        return ValidationResult.Success();
    }

    public override async Task<int> ExecuteAsync(CommandContext context, CreatePostSettings settings)
    {
        settings.Dump();

        var fileName = _fileNameGenerator.GeneratePostFileName(settings.PostDate, settings.FileName ?? settings.Title);

        var filePath = new FileInfo(Path.Combine(settings.SiteDirectory.FullName, settings.IsDraft ? JekyllFolders.Drafts : JekyllFolders.Posts, fileName));

        var frontMatter = CreateFrontMatterHeader(settings, context.Remaining.Parsed);

        await AnsiConsole.Status().StartAsync($"Creating post '{settings.Title}'", async _ =>
        {
            if (!(filePath.Directory?.Exists ?? false))
            {
                AnsiConsole.WriteLine($"Directory '{filePath.Directory!.Name}' is missing. Creating it.");
                
                filePath.Directory.Create();
            }
            
            await using var sw = File.CreateText(filePath.FullName);
            
            await sw.WriteLineAsync("---");
        
            var frontMatterText = _serializer.Serialize(frontMatter);

            await sw.WriteAsync(frontMatterText);

            await sw.WriteLineAsync("---");

            await sw.WriteLineAsync();

            await sw.WriteLineAsync("<!-- Post created by Hyde -->");
        });
        
        AnsiConsole.WriteLine($"{(!settings.IsDraft ? "Post" : "Draft for post")} '{settings.Title}' successfully created as '{filePath.Name}'.");

        return 0;
    }

    private static IReadOnlyDictionary<string, object> CreateFrontMatterHeader(CreatePostSettings settings, ILookup<string, string?> remaining)
    {
        var frontMatter = new Dictionary<string, object>
        {
            ["title"] = settings.Title,
            ["date"] = settings.PostDate.ToDateTime(settings.PostTime).ToString("yyyy-MM-dd hh:mm:ss"),
            ["layout"] = settings.Layout
        };

        if (!string.IsNullOrWhiteSpace(settings.Excerpt))
        {
            frontMatter.Add("excerpt", settings.Excerpt);
        }

        foreach (var item in remaining)
        {
            switch (item.Count())
            {
                case > 1:
                    frontMatter.Add(item.Key, item);

                    break;

                case 1 when item.Single() is { } value:
                    frontMatter.Add(item.Key, value);

                    break;
            }
        }

        return frontMatter;
    }
}
