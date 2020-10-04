using System.CommandLine;

namespace Hyde.Commands.Post
{
    public class ListPostCommand : Command
    {
        public ListPostCommand() : base("list", "Lists the blog posts of the current site")
        {
            AddOption(new Option<bool>(new[]{"--include-future", "-f"}, "Includes future posts"));
            AddOption(new Option<bool>(new[]{"--include-drafts", "-d"}, "Includes posts drafts"));
        }
    }

    public class ListPostCommandArguments 
    {
        public bool IncludeFuture { get; set; }
        public bool IncludeDrafts { get; set; }
    }
}
