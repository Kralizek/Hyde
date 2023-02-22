namespace Hyde.Types;

public abstract class TextContentFile
{
    public required string Content { get; init; }

    public abstract IReadOnlyDictionary<string, object> GetMetadataAsDictionary();
}
