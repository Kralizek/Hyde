using System.ComponentModel;
using Hyde.Utilities;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Hyde.Commands.Post;

public class ListPostCommand : AsyncCommand<ListPostCommand.ListPostSettings>
{
    private readonly IContentFileSerializer _serializer;

    public class ListPostSettings : PostSettings
    {
        [CommandOption("--include-drafts")]
        [Description("Includes posts from the draft folder")]
        public bool IncludeDrafts { get; init; }

        [CommandOption("--older-first")]
        [DefaultValue(false)]
        [Description("Shows older posts fist")]
        public bool ShowOlderFirst { get; init; }
    }

    public ListPostCommand(IContentFileSerializer serializer)
    {
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }

    public override async Task<int> ExecuteAsync(CommandContext context, ListPostSettings settings)
    {
        settings.Dump();

        var posts = await AnsiConsole.Status().StartAsync("Fetching posts", async _ =>
        {
            var items = settings.PostsDirectory.EnsureExists().EnumerateFiles().Select(file => (file, false));

            if (settings.IncludeDrafts)
            {
                items = items.Concat(settings.DraftsDirectory.EnsureExists().EnumerateFiles().Select(file => (file, true)));
            }

            var posts = new List<PostSummary>();

            foreach (var item in items)
            {
                using var reader = new StreamReader(item.file.FullName);

                var post = await _serializer.ReadPostFromStreamAsync(reader);

                posts.Add(new PostSummary(post.Title, item.file, item.Item2, post.Date, post.IsPublished));
            }

            return posts;
        });
        
        var table = new Table();

        table.AddColumn("Title", c => c.Width(80));
        table.AddColumn("Date");
        table.AddColumn("Is published");
        table.AddColumn("Is draft");
        table.AddColumn("File name");

        var postSequence = settings.ShowOlderFirst ? posts.OrderBy(file => file.Date) : posts.OrderByDescending(file => file.Date);

        foreach (var post in postSequence)
        {
            var title = new Text(post.Title);
            var date = new Text(post.Date.ToString("yyyy-MM-dd hh:mm:ss"));
            var isPublished = new Text(post is { IsPublished: true, IsDraft: false } ? "Yes" : "No");
            var fileName = new Text(post.File.Name);
            var isDraft = new Text(post.IsDraft ? "Yes" : string.Empty);

            table.AddRow(title, date, isPublished, fileName, isDraft);
        }
        
        AnsiConsole.Write(table);

        return 0;
    }

    private record PostSummary(string Title, FileInfo File, bool IsDraft, DateTime Date, bool IsPublished);
}