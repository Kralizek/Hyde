using AutoFixture.Idioms;
using AutoFixture.NUnit3;
using Hyde.Commands.Post;
using Spectre.Console.Cli;

namespace Tests.Commands.Post;

[TestFixture]
public class CreatePostCommandTests
{
    [Test, CustomAutoData]
    public void Constructor_is_guarded(GuardClauseAssertion assertion) => assertion.Verify(typeof(CreatePostCommand).GetConstructors());

    [Test, CustomAutoData]
    public void Validate_returns_error_if_not_in_Jekyll_site(CreatePostCommand sut, CommandContext context, string title)
    {
        var settings = new CreatePostCommand.CreatePostSettings { Title = title };
        
        Assume.That(settings.IsInJekyll, Is.False);
        
        var result = sut.Validate(context, settings);
        
        Assert.That(result.Successful, Is.False);
    }
}