using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Hyde.Commands.Post;

namespace Hyde
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await BuildCommandLine()
                    .UseHost(_ => Host.CreateDefaultBuilder(), host =>
                    {
                        host.ConfigureLogging(ConfigureLogging);
                    })
                    .UseDefaults()
                    .Build()
                    .InvokeAsync(args);
        }

        private static void ConfigureLogging(HostBuilderContext context, ILoggingBuilder logging)
        {
            logging.SetMinimumLevel(LogLevel.Error);
        }

        private static CommandLineBuilder BuildCommandLine()
        {
            Command rootCommand = new RootCommand("Hyde")
            {
                new Command("post")
                {
                    new NewPostCommand(),
                    new ListPostCommand()
                }
            };

            return new CommandLineBuilder(rootCommand);
        }

    }
}
