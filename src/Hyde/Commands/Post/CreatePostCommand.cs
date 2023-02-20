using Hyde.Utilities;
using Spectre.Console.Cli;

namespace Hyde.Commands.Post;

public class CreatePostCommand : AsyncCommand<CreatePostCommand.CreatePostSettings>
{
    public class CreatePostSettings : PostSettings
    {
        
    }

    public override Task<int> ExecuteAsync(CommandContext context, CreatePostSettings settings)
    {
        SettingsDumper.Dump(settings);
        
        return Task.FromResult(0);
    }
}
