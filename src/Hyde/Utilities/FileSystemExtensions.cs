namespace Hyde.Utilities;

internal static class FileSystemExtensions
{
    public static FileInfo GetFile(this DirectoryInfo directory, string fileName)
    {
        ArgumentNullException.ThrowIfNull(directory);
        
        ArgumentException.ThrowIfNullOrEmpty(fileName);

        return new FileInfo(Path.Combine(directory.FullName, fileName));
    }

    public static DirectoryInfo EnsureExists(this DirectoryInfo directory)
    {
        ArgumentNullException.ThrowIfNull(directory);
        
        if (!directory.Exists)
        {
            directory.Create();
        }

        return directory;
    }
}