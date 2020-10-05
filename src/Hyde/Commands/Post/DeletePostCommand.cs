using System.CommandLine;
using System.CommandLine.IO;
using System.Threading.Tasks;

namespace Hyde.Commands.Post
{
    public class DeletePostCommand : Command<DeletePostCommandArguments, DeletePostCommandHandler>
    {
        public DeletePostCommand() : base("delete", "Deletes a blog post")
        {
            AddOption(new Option<string>(new[] { "--title", "-t" }, "The title of the post to delete") 
            { 
                IsRequired = true,
                Name = nameof(DeletePostCommandArguments.Title)
            });
        }
    }

    public class DeletePostCommandArguments
    {
        public string Title { get; set; }
    }

    public class DeletePostCommandHandler : ICommandHandler<DeletePostCommandArguments>
    {
        public Task HandleAsync(DeletePostCommandArguments args, ExecutionContext context)
        {
            context.Console.Out.WriteLine($"Title: {args.Title}");

            return Task.CompletedTask;
        }
    }
}
