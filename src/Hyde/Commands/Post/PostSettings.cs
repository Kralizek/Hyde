using Spectre.Console;

namespace Hyde.Commands.Post;

public class PostSettings : Settings
{
    public DirectoryInfo PostsDirectory => new DirectoryInfo(Path.Combine(SiteDirectory.FullName, JekyllFolders.Posts));
    
    public DirectoryInfo DraftsDirectory => new DirectoryInfo(Path.Combine(SiteDirectory.FullName, JekyllFolders.Drafts));
    
    public override ValidationResult Validate()
    {
        if (!IsInJekyll)
        {
            return ValidationResult.Error("This command can only be executed while in a Jekyll site");
        }

        return base.Validate();
    }
}