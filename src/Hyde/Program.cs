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
    config.AddBranch("post", post =>
    {
        post.SetDescription("Manage posts");

        post.AddCommand<CreatePostCommand>("create")
            .WithDescription("Creates a new post");

        // post.AddCommand<ListPostCommand>("list")
        //     .WithDescription("Lists all available posts");
        //
        // post.AddCommand<ViewPostCommand>("view")
        //     .WithDescription("View a post");
        //
        // post.AddCommand<DeletePostCommand>("delete")
        //     .WithDescription("Deletes a posts");
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