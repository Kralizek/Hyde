using System.ComponentModel;
using Hyde.Utilities;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Hyde.Commands.Post;

public class DeletePostCommand : AsyncCommand<DeletePostCommand.DeletePostSettings>
{
    public class DeletePostSettings : PostSettings
    {
        [CommandArgument(0, "[FILENAME]")]
        [Description("The name of the post file to delete.")]
        public string? FileName { get; init; }
        
        [CommandOption("--draft")]
        [Description("Deletes the post from the draft folder")]
        public bool IsDraft { get; init; }
    }

    public override Task<int> ExecuteAsync(CommandContext context, DeletePostSettings settings)
    {
        settings.Dump();

        FileInfo? fileToDelete = null;

        if (!string.IsNullOrEmpty(settings.FileName))
        {
            fileToDelete = settings.IsDraft switch
            {
                false => settings.PostsDirectory.GetFile(settings.FileName),
                true => settings.DraftsDirectory.GetFile(settings.FileName)
            };
        }
        
        if (fileToDelete is null || !fileToDelete.Exists)
        {
            var directory = settings.IsDraft ? settings.DraftsDirectory : settings.PostsDirectory;
            
            fileToDelete = AnsiConsole.Prompt(
                new SelectionPrompt<FileInfo>()
                    .Title("Select a post to be deleted")
                    .PageSize(10)
                    .AddChoices(directory.EnsureExists().EnumerateFiles().OrderByDescending(f => f.Name))
                    .MoreChoicesText("[grey](Move up and down to reveal more posts)[/]")
                    .UseConverter(file => file.Name)
            );
        }

        AnsiConsole.WriteLine($"Deleting {fileToDelete.Name}");
        
        fileToDelete.Delete();
        
        AnsiConsole.WriteLine($"{fileToDelete.Name} deleted");

        return Task.FromResult(0);
    }
}