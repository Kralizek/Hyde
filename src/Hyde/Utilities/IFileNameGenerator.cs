using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;

namespace Hyde.Utilities;

public interface IFileNameGenerator
{
    string GeneratePostFileName(DateOnly date, string title);
}

public class MarkdownFileNameGeneratorOptions
{
    public required string MarkdownExtension { get; set; } = "md";
}

public class MarkdownFileNameGenerator : IFileNameGenerator
{
    private readonly MarkdownFileNameGeneratorOptions _options;
    
    public MarkdownFileNameGenerator(IOptions<MarkdownFileNameGeneratorOptions> options)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }
    
    public string GeneratePostFileName(DateOnly date, string title)
    {
        var result = new StringBuilder();

        result.Append(date.ToString("yyyy-MM-dd"));
        
        result.Append('-');

        result.Append(ToKebabCase(title));

        result.Append('.');

        result.Append(_options.MarkdownExtension);

        return result.ToString();
    }

    private static string ToKebabCase(string value)
    {
        // Replace all non-alphanumeric characters with a dash
        value = Regex.Replace(value, @"[^0-9a-zA-Z]", "-");

        // Replace all subsequent dashes with a single dash
        value = Regex.Replace(value, @"[-]{2,}", "-");

        // Remove any trailing dashes
        value = Regex.Replace(value, @"-+$", string.Empty);

        // Remove any dashes in position zero
        if (value.StartsWith("-")) value = value[1..];

        // Lowercase and return
        return value.ToLower();
    }
}