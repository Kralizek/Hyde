using System.ComponentModel;
using Hyde.Types;
using Hyde.Utilities;
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Rendering;

namespace Hyde.Commands.Post;

public class ViewPostCommand : AsyncCommand<ViewPostCommand.ViewPostSettings>
{
    private readonly IContentFileSerializer _serializer;

    public class ViewPostSettings : PostSettings
    {
        [CommandOption("--draft")]
        [Description("Deletes the post from the draft folder")]
        [DefaultValue(false)]
        public bool IsDraft { get; init; }

        [CommandOption("--show-content")]
        [Description("Shows the content of the post")]
        [DefaultValue(false)]
        public bool ShowContent { get; init; }
    }

    public ViewPostCommand(IContentFileSerializer serializer)
    {
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }

    public override async Task<int> ExecuteAsync(CommandContext context, ViewPostSettings settings)
    {
        settings.Dump();
        
        var directory = settings.IsDraft ? settings.DraftsDirectory : settings.PostsDirectory;

        if (!directory.EnumerateFiles().Any())
        {
            AnsiConsole.WriteLine($"No files found in {directory.Name}");

            return 0;
        }
            
        var fileToView = AnsiConsole.Prompt(
            new SelectionPrompt<FileInfo>()
                .Title("Select a post to be viewed")
                .PageSize(10)
                .AddChoices(directory.EnsureExists().EnumerateFiles().OrderByDescending(f => f.Name))
                .MoreChoicesText("[grey](Move up and down to reveal more posts)[/]")
                .UseConverter(file => file.Name)
        );

        using var reader = new StreamReader(fileToView.FullName);

        var post = await _serializer.ReadPostFromStreamAsync(reader);
        
        PrintFrontMatter(post);

        if (settings.ShowContent)
        {
            PrintContent(post);
        }

        return 0;
    }

    private static void PrintFrontMatter(PostFile post)
    {
        var table = new Table { Width = 100 };
        table.AddColumn("Property");
        table.AddColumn("Value", tc => tc.Width(85));

        table.AddRow("Title", post.Title);
        table.AddRow("Date", post.Date.ToString("yyyy-MM-dd hh:mm:ss"));
        table.AddRow("Is published", post.IsPublished.ToString());
        table.AddRow("Layout", post.Layout ?? "[grey]none[/]");
        table.AddRow("Excerpt", post.Excerpt ?? "[grey]none[/]");

        if (post.Tags.Any())
        {
            table.AddRow(new Text("Tags"), post.Tags.Aggregate(new Table().AddColumn("Tag"), (t, item) => t.AddRow(item)));
        }

        if (post.Categories.Any())
        {
            table.AddRow(new Text("Categories"), post.Categories.Aggregate(new Table().AddColumn("Category"), (t, item) => t.AddRow(item)));
        }
        
        foreach (var item in post.CustomVariables)
        {
            IRenderable valueCell = item.Value switch
            {
                string str => new Text(str),
                IEnumerable<string> list => list.Aggregate(new Table().AddColumn("Value"), (t, it) => t.AddRow(new Text(it))),
                IEnumerable<object> list => list.Aggregate(new Table().AddColumn("Value"), (t, it) => t.AddRow(new Text($"{it}"))),
                { } obj => new Text($"{obj}")
            };

            table.AddRow(new Text(item.Key), valueCell);
        }

        AnsiConsole.Write(table);
    }

    private static void PrintContent(PostFile post)
    {
        AnsiConsole.Write(new Text(post.Content));
    }
}