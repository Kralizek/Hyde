# Hyde
A CLI tool to manage content of a site based on Jekyll

# Objectives
The tool will support commands like:
- hyde post new
- hyde post list
- hyde post view
- hyde post delete
- hyde page new
- hyde page list
- hyde page view
- hyde page delete

The tool will be built in .NET Core and be available as a global tool. I aim to be using the new [`System.CommandLine`](https://github.com/dotnet/command-line-api) APIs.

Ideally, the tool should expose an interactive UI similar to the one found in the GitHub CLI.

When creating a new post or a new page, users should be able to:
- provide most of the parameters as arguments
- provide parameters interactively (optional)
- use default values specified in a configuration dotfile (e.g. `.hyde.json`)
- create a blank page/post and let the user customize the content
