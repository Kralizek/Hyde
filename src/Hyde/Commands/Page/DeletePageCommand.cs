﻿using Hyde.Utilities;
using Spectre.Console.Cli;

namespace Hyde.Commands.Page;

public class DeletePageCommand : AsyncCommand<DeletePageCommand.DeletePageSettings>
{
    public class DeletePageSettings : PageSettings
    {
        
    }

    public override Task<int> ExecuteAsync(CommandContext context, DeletePageSettings settings)
    {
        settings.Dump();
        
        return Task.FromResult(0);
    }
}