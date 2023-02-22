using System.ComponentModel;
using Hyde.Types;
using Hyde.Utilities;
using Spectre.Console;
using Spectre.Console.Cli;
using YamlDotNet.Serialization;

namespace Hyde.Commands.Post;

public class CreatePostCommand : AsyncCommand<CreatePostCommand.CreatePostSettings>
{
    private readonly IContentFileSerializer _serializer;
    private readonly IFileNameGenerator _fileNameGenerator;

    public class CreatePostSettings : PostSettings
    {
        [CommandArgument(0, "[TITLE]")]
        [Description("The title of the post")]
        public string? Title { get; init; }
        
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

    public CreatePostCommand(IContentFileSerializer serializer, IFileNameGenerator fileNameGenerator)
    {
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        _fileNameGenerator = fileNameGenerator ?? throw new ArgumentNullException(nameof(fileNameGenerator));
    }

    public override async Task<int> ExecuteAsync(CommandContext context, CreatePostSettings settings)
    {
        settings.Dump();

        var fileName = _fileNameGenerator.GeneratePostFileName(settings.PostDate, settings.FileName ?? settings.Title);
        var title = settings.Title ?? AnsiConsole.Ask<string>("Specify a title for the new post:");

        var filePath = settings.IsDraft switch
        {
            false => settings.PostsDirectory.GetFile(fileName),
            true => settings.DraftsDirectory.GetFile(fileName)
        };

        await AnsiConsole.Status().StartAsync($"Creating post '{title}'", async _ =>
        {
            filePath.Directory?.EnsureExists();
            
            var post = new PostFile
            {
                Title = title,
                Date = settings.PostDate.ToDateTime(settings.PostTime),
                Layout = settings.Layout,
                Excerpt = settings.Excerpt,
                Content = "<!-- Post created by Hyde -->",
                IsPublished = !settings.IsDraft
            };
            
            await using var sw = File.CreateText(filePath.FullName);

            await _serializer.WriteToStreamAsync(sw, post);
            
        });
        
        AnsiConsole.WriteLine($"{(!settings.IsDraft ? "Post" : "Draft for post")} '{title}' successfully created as '{filePath.Name}'.");

        return 0;
    }
}
