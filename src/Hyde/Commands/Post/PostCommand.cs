using System.CommandLine;

namespace Hyde.Commands.Post
{
    public class PostCommand : Command
    {
        public PostCommand() : base("post", null)
        {
            Add(new NewPostCommand());
            Add(new ListPostCommand());
            Add(new EditPostCommand());
            Add(new DeletePostCommand());
        }
    }
}
