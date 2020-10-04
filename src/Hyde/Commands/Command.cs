using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Hyde.Commands
{
    public interface ICommandHandler<TArg>
        where TArg: class
    {
        Task HandleAsync(TArg arguments, ExecutionContext context);
    }

    public abstract class Command<TArg, THandler> : Command
        where TArg : class
        where THandler : ICommandHandler<TArg>
    {
        protected Command(string name, string description) : base(name, description)
        {
            Handler = CommandHandler.Create<IHost, TArg, ExecutionContext>((IHost host, TArg args, ExecutionContext context) => 
            {
                var handler = host.Services.GetRequiredService<THandler>();
                return handler.HandleAsync(args, context);
            });
        }
    }
}