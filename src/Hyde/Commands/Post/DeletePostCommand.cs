using Hyde.Utilities;
using Spectre.Console.Cli;

namespace Hyde.Commands.Post;

public class DeletePostCommand : AsyncCommand<DeletePostCommand.DeletePostSettings>
{
    public class DeletePostSettings : PostSettings
    {
        
    }

    public override Task<int> ExecuteAsync(CommandContext context, DeletePostSettings settings)
    {
        SettingsDumper.Dump(settings);

        return Task.FromResult(0);
    }
}