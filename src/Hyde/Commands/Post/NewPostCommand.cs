using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

namespace Hyde.Commands.Post
{
    public class NewPostCommand : Command
    {
        public NewPostCommand() : base("new", "Creates a new blog post")
        {
            AddOption(new Option<string>(new[] { "--title", "-t" }, "The title of the post") { IsRequired = true });
            AddOption(new Option<DateTime>(new[] { "--date", "-d" }, getDefaultValue: () => DateTime.Now, "The date and time of the post"));
            AddOption(new Option<string[]>(new[] { "--categories", "-c" }, "The categories to be added to the post"));
            AddOption(new Option<string[]>(new[] { "--tags" }, "The tags to be added to the post"));
            AddOption(new Option<string>(new[] { "--filename", "-fn" }, "The title part of the post file name"));
            AddOption(new Option<string>(new[] { "--extension", "-ext" }, getDefaultValue: () => "md", "The extension to be used when creating the file"));
            AddOption(new Option<string>(new[] { "--layout", "-l" }, getDefaultValue: () => "post", "The layout to be used when creating the post"));
            AddOption(new Option<FileInfo>(new[] { "--content" }, "Loads the content of the post from the specified file"));
            AddOption(new Option<FileInfo>(new[] { "--custom-values", "-cv" }, "Loads the content of the file and uses it to add custom values to the Front Matter part of the post"));
            AddOption(new Option<bool>(new[]{"--draft"}, "Creates the new blog post as a draft"));
            AddOption(new Option<bool>(new[]{"--interactive", "-i"}, "Interactively queries the user to provide values that were not provided via other options"));
            AddOption(new Option<bool>(new[]{"--editor", "-e"}, "Opens the default text editor with the newly created file"));

            Handler = CommandHandler.Create<NewPostCommandArguments, IConsole>(CreateNewPost);
        }

        private static Task CreateNewPost(NewPostCommandArguments args, IConsole console)
        {
            console.Out.WriteLine($"Title: {args.Title}");
            console.Out.WriteLine($"Date: {args.Date:yyyy-MM-dd}");
            console.Out.WriteLine($"Categories: {args.Categories}");
            console.Out.WriteLine($"Tags: {args.Tags}");
            console.Out.WriteLine($"Extension: {args.Extension}");
            console.Out.WriteLine($"Layout: {args.Layout}");

            return Task.CompletedTask;
        }
    }

    public class NewPostCommandArguments
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public IReadOnlyList<string> Categories { get; set; }
        public IReadOnlyList<string> Tags { get; set; }
        public string Extension { get; set; }
        public string Layout { get; set; }
    }
}
