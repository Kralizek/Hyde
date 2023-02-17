using Hyde.Utilities;
using Spectre.Console.Cli;

namespace Hyde.Commands.Page;

public class ListPageCommand : AsyncCommand<ListPageCommand.ListPageSettings>
{
    public class ListPageSettings : PageSettings
    {
        
    }

    public override Task<int> ExecuteAsync(CommandContext context, ListPageSettings settings)
    {
        SettingsDumper.Dump(settings);
        
        return Task.FromResult(0);
    }
}