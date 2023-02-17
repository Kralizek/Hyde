using Hyde.Utilities;
using Spectre.Console.Cli;

namespace Hyde.Commands.Post;

public class ViewPostCommand : AsyncCommand<ViewPostCommand.ViewPostSettings>
{
    public class ViewPostSettings : PostSettings
    {
        
    }

    public override Task<int> ExecuteAsync(CommandContext context, ViewPostSettings settings)
    {
        SettingsDumper.Dump(settings);
        
        return Task.FromResult(0);
    }
}