using System.CommandLine;
using System.CommandLine.IO;
using System.Threading.Tasks;

namespace Hyde.Commands.Post
{
    public class ListPostCommand : Command<ListPostCommandArguments, ListPostCommandHandler>
    {
        public ListPostCommand() : base("list", "Lists the blog posts of the current site")
        {
            AddOption(new Option<bool>(new[]{"--include-future", "-f"}, "Includes future posts")
            {
                Name = nameof(ListPostCommandArguments.IncludeFuture)
            });

            AddOption(new Option<bool>(new[]{"--include-drafts", "-d"}, "Includes posts drafts")
            {
                Name = nameof(ListPostCommandArguments.IncludeDrafts)
            });
        }
    }

    public class ListPostCommandArguments 
    {
        public bool IncludeFuture { get; set; }
        public bool IncludeDrafts { get; set; }
    }

    public class ListPostCommandHandler : ICommandHandler<ListPostCommandArguments>
    {
        public Task HandleAsync(ListPostCommandArguments arguments, ExecutionContext context)
        {
            context.Console.Out.WriteLine($"IncludeFuture: {arguments.IncludeFuture}");
            context.Console.Out.WriteLine($"IncludeDraft: {arguments.IncludeDrafts}");
            
            return Task.CompletedTask;
        }
    }
}
