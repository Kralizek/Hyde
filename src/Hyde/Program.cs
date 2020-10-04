using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Hyde.Commands.Post;
using Microsoft.Extensions.DependencyInjection;

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

                        host.ConfigureServices(ConfigureServices);
                    })
                    .UseDefaults()
                    .Build()
                    .InvokeAsync(args);
        }

        private static void ConfigureLogging(HostBuilderContext context, ILoggingBuilder logging)
        {
            logging.SetMinimumLevel(LogLevel.Error);
        }

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddTransient<NewPostCommandHandler>();

            services.AddTransient<ExecutionContext>(sp => 
            {
                var host = sp.GetRequiredService<IHost>();

                var console = host.Services.GetRequiredService<IConsole>();

                return new ExecutionContext{ Console = console };
            });
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

    public class ExecutionContext 
    {
        public IConsole Console { get; set; }
    }
}
