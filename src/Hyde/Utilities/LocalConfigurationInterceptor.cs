using Hyde.Utilities;
using Microsoft.Extensions.Configuration;
using Spectre.Console.Cli;

public class LocalConfigurationInterceptor : ICommandInterceptor
{
    private readonly ConfigurationManager _configuration;

    public LocalConfigurationInterceptor(ConfigurationManager configurationManager)
    {
        _configuration = configurationManager ?? throw new ArgumentNullException(nameof(configurationManager));
    }
    
    public void Intercept(CommandContext context, CommandSettings commandSettings)
    {
        if (commandSettings is not Hyde.Commands.Settings settings) return;

        var localSettings = settings.SiteDirectory.GetFile(Constants.ConfigurationFileName);

        _configuration.AddJsonFile(localSettings);
    }
}