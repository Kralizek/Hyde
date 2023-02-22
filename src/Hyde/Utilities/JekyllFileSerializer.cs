using Hyde.Types;
using YamlDotNet.Serialization;

namespace Hyde.Utilities;

public interface IContentFileSerializer
{
    Task WriteToStreamAsync<TContentFile>(StreamWriter writer, TContentFile contentFile)
        where TContentFile : TextContentFile;
}

public class JekyllContentFileSerializer : IContentFileSerializer
{
    private readonly ISerializer _serializer;

    public JekyllContentFileSerializer(ISerializer serializer)
    {
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
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
}