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
}