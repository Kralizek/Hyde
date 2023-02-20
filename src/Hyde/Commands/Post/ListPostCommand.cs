using Hyde.Utilities;
using Spectre.Console.Cli;

namespace Hyde.Commands.Post;

public class ListPostCommand : AsyncCommand<ListPostCommand.ListPostSettings>
{
    public class ListPostSettings : PostSettings
    {
        
    }

    public override Task<int> ExecuteAsync(CommandContext context, ListPostSettings settings)
    {
        SettingsDumper.Dump(settings);

        return Task.FromResult(0);
    }
}