using Hyde.Commands.Page;
using Hyde.Commands.Post;
using Hyde.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Extensions.DependencyInjection;
using YamlDotNet.Serialization;

var services = new ServiceCollection();

services.AddSingleton(_ => new SerializerBuilder()
    .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull | DefaultValuesHandling.OmitEmptyCollections | DefaultValuesHandling.OmitDefaults));

services.AddSingleton<ISerializer>(sp => sp.GetRequiredService<SerializerBuilder>().Build());

services.AddSingleton<IFileNameGenerator, DefaultFileNameGenerator>();

services.AddTransient<IContentFileSerializer, JekyllContentFileSerializer>();

var registrar = new DependencyInjectionRegistrar(services);

var app = new CommandApp(registrar);

app.Configure(config =>
{
    config.ValidateExamples();
    
    config.AddBranch("post", post =>
    {
        post.SetDescription("Manage posts");

        post.AddCommand<CreatePostCommand>("create")
            .WithDescription("Creates a new post")
            .WithExample(new[] { "post", "create" })
            .WithExample(new[] { "post", "create", "\"Hello world, the most common title\"" })
            .WithExample(new[] { "post", "create", "--filename", "hello-world" })
            .WithExample(new[] { "post", "create", "--date", "2022-12-10" })
            .WithExample(new[] { "post", "create", "--time", "08:30" })
            .WithExample(new[] { "post", "create", "--draft" })
            .WithExample(new[] { "post", "create", "--layout", "custom-layout" })
            .WithExample(new[] { "post", "create", "--site", "/path/to/your/site" });

        post.AddCommand<ListPostCommand>("list")
            .WithDescription("Lists all available posts")
            .WithExample(new[] { "post", "list", "--site", "/path/to/your/site" })
            .WithExample(new[] { "post", "list", "--include-drafts" });

        // post.AddCommand<ViewPostCommand>("view")
        //     .WithDescription("View a post");

        post.AddCommand<DeletePostCommand>("delete")
            .WithDescription("Deletes a posts")
            .WithExample(new[] { "post", "delete", "2023-02-22-hello-world.md" })
            .WithExample(new[] { "post", "delete", "--site", "/path/to/your/site" })
            .WithExample(new[] { "post", "delete", "--draft" });
    });
    
    // config.AddBranch("page", page =>
    // {
    //     page.SetDescription("Manage pages");
    //     
    //     page.AddCommand<CreatePageCommand>("create")
    //         .WithDescription("Creates a page");
    //     
    //     page.AddCommand<ListPageCommand>("list")
    //         .WithDescription("Lists all available pages");
    //     
    //     page.AddCommand<ViewPageCommand>("view")
    //         .WithDescription("View a page");
    //     
    //     page.AddCommand<DeletePageCommand>("delete")
    //         .WithDescription("Deletes a page");
    // });
});

return app.Run(args);