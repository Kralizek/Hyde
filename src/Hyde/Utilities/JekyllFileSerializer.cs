using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;
using Hyde.Types;
using YamlDotNet.Serialization;

namespace Hyde.Utilities;

public interface IContentFileSerializer
{
    Task WriteToStreamAsync<TContentFile>(StreamWriter writer, TContentFile contentFile)
        where TContentFile : TextContentFile;

    Task<PostFile> ReadPostFromStreamAsync(StreamReader reader);
}

public partial class JekyllContentFileSerializer : IContentFileSerializer
{
    private readonly ISerializer _serializer;
    private readonly IDeserializer _deserializer;

    public JekyllContentFileSerializer(ISerializer serializer, IDeserializer deserializer)
    {
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        _deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
    }

    public async Task WriteToStreamAsync<TContentFile>(StreamWriter writer, TContentFile contentFile)
        where TContentFile : TextContentFile
    {
        var frontMatter = _serializer.Serialize(contentFile.GetMetadataAsDictionary());

        await writer.WriteLineAsync("---");

        await writer.WriteAsync(frontMatter);
        
        await writer.WriteLineAsync("---");

        await writer.WriteAsync(contentFile.Content);
    }

    public async Task<PostFile> ReadPostFromStreamAsync(StreamReader reader)
    {
        var fileContent = await reader.ReadToEndAsync();

        var match = PostRegex.Match(fileContent);

        if (!match.Success)
        {
            throw new ArgumentException("The file is not a valid markdown file");
        }

        var frontMatterText = match.Groups["frontMatter"].Value;

        var frontMatterDictionary = _deserializer.Deserialize<Dictionary<string, object>>(frontMatterText);
        
        var content = match.Groups["content"].Value.Trim();

        return GetPostFile(frontMatterDictionary, content);
    }

    private static PostFile GetPostFile(IReadOnlyDictionary<string,object> dictionary, string content)
    {
        var data = new Dictionary<string, object>(dictionary);

        var title = Get(data, "title", value => value switch
        {
            string str => str,
            _ => throw new NotSupportedException()
        }, string.Empty);

        var date = Get(data, "date", value => value switch
        {
            string str => DateTime.Parse(str),
            _ => throw new NotSupportedException()
        }, default);

        var isPublished = Get(data, "published", value => value switch
        {
            bool isPublished => isPublished,
            null => true,
            _ => throw new NotSupportedException()
        }, true);

        var excerpt = Get(data, "excerpt", value => value switch
        {
            string str => str,
            _ => null
        }, default);
        
        var layout = Get(data, "layout", value => value switch
        {
            string str => str,
            _ => null
        }, default);

        var categories = Get(data, "categories", value => value switch
        {
            string str => str.Split(" ", StringSplitOptions.RemoveEmptyEntries),
            List<object> list => list.Select(item => (string)item).ToArray(),
            _ => throw new NotSupportedException()
        }, Array.Empty<string>());
        
        var tags = Get(data, "tags", value => value switch
        {
            string str => str.Split(" ", StringSplitOptions.RemoveEmptyEntries),
            List<object> list => list.Select(item => (string)item).ToArray(),
            _ => throw new NotSupportedException()
        }, Array.Empty<string>());

        var customVariables = new Dictionary<string, object>(data.Select(item => new KeyValuePair<string, object>(item.Key, item.Value switch
        {
            "true" => true,
            "false" => false,
            string str => str,
            IEnumerable<string> list => list,
            IEnumerable<object> list => list,
            { } obj => obj
        })));

        return new PostFile
        {
            Title = title,
            Categories = categories,
            Content = content,
            Date = date,
            Excerpt = excerpt,
            Layout = layout,
            Tags = tags,
            IsPublished = isPublished,
            CustomVariables = customVariables
        };

        static T Get<T>(IDictionary<string, object> dictionary, string key, Func<object, T> getter, T defaultValue) => dictionary.TryPopValue(key, out var value) ? getter(value) : defaultValue;
    }

    private static readonly Regex PostRegex = GeneratePostRegex();

    [GeneratedRegex(@"^---\r?\n(?<frontMatter>(.*\n)*?)---\r?\n(\r?\n)?(?<content>[\w\W]*)$")]
    private static partial Regex GeneratePostRegex();
}

file static class LocalExtensions
{
    public static bool TryPopValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        if (dictionary.TryGetValue(key, out value))
        {
            dictionary.Remove(key);

            return true;
        }

        value = default;
        return false;
    }
}