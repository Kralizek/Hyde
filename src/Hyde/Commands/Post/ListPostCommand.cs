using System.ComponentModel;
using Hyde.Utilities;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Hyde.Commands.Post;

public class ListPostCommand : AsyncCommand<ListPostCommand.ListPostSettings>
{
    public class ListPostSettings : PostSettings
    {
        [CommandOption("--include-drafts")]
        [Description("Includes posts from the draft folder")]
        public bool IncludeDrafts { get; init; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, ListPostSettings settings)
    {
        settings.Dump();

        var files = await AnsiConsole.Status().StartAsync("Fetching posts", _ =>
        {
            var files = settings.PostsDirectory.EnsureExists().EnumerateFiles().Select(file => (file, false));

            if (settings.IncludeDrafts)
            {
                files = files.Concat(settings.DraftsDirectory.EnsureExists().EnumerateFiles().Select(file => (file, true)));
            }

            return Task.FromResult(files);
        });
        
        var table = new Table();

        table.AddColumn("File name");
        table.AddColumn("Is draft");

        foreach (var file in files.OrderByDescending(file => file.file.Name))
        {
            table.AddRow(file.file.Name, file.Item2 ? "Yes" : string.Empty);
        }
        
        AnsiConsole.Write(table);

        return 0;
    }
}