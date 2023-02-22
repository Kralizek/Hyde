using Hyde.Utilities;
using Spectre.Console.Cli;

namespace Hyde.Commands.Page;

public class ViewPageCommand : AsyncCommand<ViewPageCommand.ViewPageSettings>
{
    public class ViewPageSettings : PageSettings
    {
        
    }

    public override Task<int> ExecuteAsync(CommandContext context, ViewPageSettings settings)
    {
        settings.Dump();
        
        return Task.FromResult(0);
    }
}