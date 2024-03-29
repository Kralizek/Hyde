﻿using Hyde.Utilities;
using Spectre.Console.Cli;

namespace Hyde.Commands.Page;

public class CreatePageCommand : AsyncCommand<CreatePageCommand.CreatePageSettings>
{
    public class CreatePageSettings : PageSettings
    {
        
    }
    
    public override Task<int> ExecuteAsync(CommandContext context, CreatePageSettings settings)
    {
        settings.Dump();
        
        return Task.FromResult(0);
    }
}