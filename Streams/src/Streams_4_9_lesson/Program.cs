namespace Streams_4_9_lesson;

internal class Program
{
    static async Task Main(string[] args)
    {

    }

    public async Task CopyLargerFileAsync(string sourcePath, string destinationPath)
    {
        const int bufferSize = 1024 * 1024 * 10;

        using FileStream sourceStream = new FileStream(
            sourcePath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            bufferSize,
            useAsync: true);

        using (FileStream destinationStream = new FileStream(
            destinationPath,
            FileMode.Create,
            FileAccess.Write,
            FileShare.None,
            bufferSize,
            useAsync: true))
        {

        }
    }
}
}
