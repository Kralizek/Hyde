using System;
using System.CommandLine;
using System.CommandLine.IO;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Hyde.Commands.Post
{
    public class NewPostCommand : Command<NewPostCommandArguments, NewPostCommandHandler>
    {
        public NewPostCommand() : base("new", "Creates a new blog post")
        {
            AddOption(new Option<string>(new[] { "--title", "-t" }, "The title of the post") 
            { 
                IsRequired = true,
                Name = nameof(NewPostCommandArguments.Title)
            });

            AddOption(new Option<DateTime>(new[] { "--date", "-d" }, getDefaultValue: () => DateTime.Now, "The date and time of the post")
            {
                Name = nameof(NewPostCommandArguments.Date)
            });
            
            AddOption(new Option<string[]>(new[] { "--categories", "-c" }, "The categories to be added to the post")
            {
                Name = nameof(NewPostCommandArguments.Categories)
            });
            
            AddOption(new Option<string[]>(new[] { "--tags" }, "The tags to be added to the post")
            {
                Name = nameof(NewPostCommandArguments.Tags)
            });
            
            AddOption(new Option<string>(new[] { "--filename", "-fn" }, "The title part of the post file name")
            {
                Name = nameof(NewPostCommandArguments.FileName)
            });
            
            AddOption(new Option<string>(new[] { "--extension" }, getDefaultValue: () => "md", "The extension to be used when creating the file")
            {
                Name = nameof(NewPostCommandArguments.FileExtension)
            });
            
            AddOption(new Option<string>(new[] { "--layout", "-l" }, getDefaultValue: () => "post", "The layout to be used when creating the post")
            {
                Name = nameof(NewPostCommandArguments.Layout)
            });
            
            AddOption(new Option<FileInfo>(new[] { "--content" }, "Loads the content of the post from the specified file")
            {
                Name = nameof(NewPostCommandArguments.ContentFile)
            });
            
            AddOption(new Option<FileInfo>(new[] { "--custom-values", "-cv" }, "Loads the content of the file and uses it to add custom values to the Front Matter part of the post")
            {
                Name = nameof(NewPostCommandArguments.CustomValuesFile)
            });

            AddOption(new Option<bool>(new[] { "--draft" }, "Creates the new blog post as a draft")
            { 
                Name = nameof(NewPostCommandArguments.IsDraft) 
            });

            AddOption(new Option<bool>(new[] { "--interactive", "-i" }, "Interactively queries the user to provide values that were not provided via other options")
            { 
                Name = nameof(NewPostCommandArguments.UseInteractive) 
            });

            AddOption(new Option<bool>(new[] { "--editor", "-e" }, "Opens the default text editor with the newly created file") 
            { 
                Name = nameof(NewPostCommandArguments.OpenEditor) 
            });
        }
    }

    public class NewPostCommandArguments
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public IReadOnlyList<string> Categories { get; set; }
        public IReadOnlyList<string> Tags { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string Layout { get; set; }

        public bool IsDraft { get; set; }
        public bool UseInteractive { get; set; }
        public bool OpenEditor { get; set; }

        public FileInfo ContentFile { get; set; }
        public FileInfo CustomValuesFile { get; set; }
    }

    public class NewPostCommandHandler : ICommandHandler<NewPostCommandArguments>
    {
        private readonly ILogger<NewPostCommandHandler> _logger;

        public NewPostCommandHandler(ILogger<NewPostCommandHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task HandleAsync(NewPostCommandArguments args, ExecutionContext context)
        {
            context.Console.Out.WriteLine($"Title: {args.Title}");
            context.Console.Out.WriteLine();
            context.Console.Out.WriteLine($"Date: {args.Date:yyyy-MM-dd}");
            context.Console.Out.WriteLine($"Categories: {args.Categories}");
            context.Console.Out.WriteLine($"Tags: {args.Tags}");
            context.Console.Out.WriteLine($"File Name: {args.FileName}");
            context.Console.Out.WriteLine($"File Extension: {args.FileExtension}");
            context.Console.Out.WriteLine($"Layout: {args.Layout}");
            context.Console.Out.WriteLine();
            context.Console.Out.WriteLine($"IsDraft: {args.IsDraft}");
            context.Console.Out.WriteLine($"UseInteractive: {args.UseInteractive}");
            context.Console.Out.WriteLine($"OpenEditor: {args.OpenEditor}");
            context.Console.Out.WriteLine();
            context.Console.Out.WriteLine($"ContentFile: {args.ContentFile?.FullName ?? "n/a" } Exists: {args.ContentFile?.Exists ?? false}");
            context.Console.Out.WriteLine($"CustomValuesFile: {args.CustomValuesFile?.FullName ?? "n/a" } Exists: {args.CustomValuesFile?.Exists ?? false}");

            return Task.CompletedTask;
        }
    }
}
