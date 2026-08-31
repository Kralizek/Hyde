using YamlDotNet.Serialization;

namespace Hyde.Types;

public class PostFile : TextContentFile
{
    public required string Title { get; init; }

    public DateTime Date { get; init; }

    public string? Excerpt { get; init; }

    public string[] Tags { get; init; } = Array.Empty<string>();

    public string[] Categories { get; init; } = Array.Empty<string>();

    public bool IsPublished { get; init; } = true;

    public string? Layout { get; set; }

    public IReadOnlyDictionary<string, object> CustomVariables { get; init; } = new Dictionary<string, object>();
    
    public override IReadOnlyDictionary<string, object> GetMetadataAsDictionary()
    {
        var result = new Dictionary<string, object>
        {
            ["title"] = Title,
            ["date"] = Date.ToString("yyyy-MM-dd hh:mm:ss"),
            ["published"] = IsPublished
        };

        if (!string.IsNullOrEmpty(Layout))
        {
            result.Add("layout", Layout);
        }

        if (!string.IsNullOrEmpty(Excerpt))
        {
            result.Add("excerpt", Excerpt);
        }

        if (Tags.Any())
        {
            result.Add("tags", Tags);
        }

        if (Categories.Any())
        {
            result.Add("categories", Categories);
        }

        foreach (var value in CustomVariables)
        {
            result.Add(value.Key, value.Value);
        }

        return result;
    }
}
