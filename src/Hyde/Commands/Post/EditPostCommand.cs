using System.CommandLine;
using System.CommandLine.IO;
using System.Threading.Tasks;

namespace Hyde.Commands.Post
{
    public class EditPostCommand : Command<EditPostCommandArguments, EditPostCommandHandler>
    {
        public EditPostCommand() : base("edit", "Opens a blog post in the preferred editor")
        {
            AddOption(new Option<string>(new[] { "--title", "-t" }, "The title of the post to delete") 
            { 
                IsRequired = true,
                Name = nameof(EditPostCommandArguments.Title)
            });
        }
    }

    public class EditPostCommandArguments
    {
        public string Title { get; set; }
    }

    public class EditPostCommandHandler : ICommandHandler<EditPostCommandArguments>
    {
        public Task HandleAsync(EditPostCommandArguments args, ExecutionContext context)
        {
            context.Console.Out.WriteLine($"Title: {args.Title}");

            return Task.CompletedTask;
        }
    }
}
