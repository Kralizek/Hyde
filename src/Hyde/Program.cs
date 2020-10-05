using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Hyde.Commands.Post;
using Microsoft.Extensions.DependencyInjection;
using Hyde.Commands;

namespace Hyde
{
    class Program
    {
        static async Task Main(string[] args) => await BuildCommandLine()
            .UseHost(_ => Host.CreateDefaultBuilder(), host =>
            {
                host.ConfigureLogging(ConfigureLogging);

                host.ConfigureServices(ConfigureServices);
            })
            .UseDefaults()
            .Build()
            .InvokeAsync(args);

        private static void ConfigureLogging(HostBuilderContext context, ILoggingBuilder logging)
        {
            logging.SetMinimumLevel(LogLevel.Error);
        }

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.Scan(scan => scan.FromAssemblyOf<Program>()
                                      .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
                                                                    .AsSelf()
                                                                    .WithTransientLifetime()
            );

            services.AddTransient<ExecutionContext>(sp => 
            {
                var host = sp.GetRequiredService<IHost>();

                var console = host.Services.GetRequiredService<IConsole>();

                return new ExecutionContext{ Console = console };
            });
        }

        private static CommandLineBuilder BuildCommandLine()
        {
            var builder = new CommandLineBuilder(new RootCommand("Hyde")
            {
                new PostCommand()
            });

            return builder;
        }

    }
}
