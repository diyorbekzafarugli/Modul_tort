namespace OnlineWebManagement.Broker.Services;

public class LocalStorageBroker : IStorageBroker
{
    private readonly string BasePath;
    public LocalStorageBroker()
    {
        BasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

        if (!Directory.Exists(BasePath))
            Directory.CreateDirectory(BasePath);
    }
    public void CreateFolder(string folderPath)
    {
        var currentPath = Path.Combine(BasePath, folderPath);
        var parentPath = Directory.GetParent(currentPath);

        EnsureDirectoryNotExists(parentPath.FullName);
        EnsureDirectoryExists(currentPath);

        Directory.CreateDirectory(currentPath);
    }

    public void DeleteFile(string filePath)
    {
        throw new NotImplementedException();
    }

    public void DeleteFolder(string folderPath)
    {
        var currentPath = Path.Combine(BasePath, folderPath);
        EnsureDirectoryNotExists(currentPath);

        Directory.Delete(currentPath, recursive: true);
    }

    public Task<Stream> DownloadFileAsync(string filePath)
    {
        throw new NotImplementedException();
    }

    public Task<Stream> DownloadFolderAsZipAsync(string folderPath)
    {
        throw new NotImplementedException();
    }

    public Task EditFileAsync(string filePath, string content)
    {
        throw new NotImplementedException();
    }

    public List<string> GetAll(string folderPath)
    {
        throw new NotImplementedException();
    }

    public Task<List<string>> GetTextOfFileAsync(string filePath)
    {
        throw new NotImplementedException();
    }

    public Task UploadFileAsync(string filePath, Stream stream)
    {
        throw new NotImplementedException();
    }

    private void EnsureDirectoryExists(string path)
    {
        if (Directory.Exists(path))
        {
            throw new Exception($"Directory '{path}' already exists.");
        }
    }

    private void EnsureFileExists(string path)
    {
        if (File.Exists(path))
        {
            throw new Exception($"File '{path}' already exists.");
        }
    }

    private void EnsureDirectoryNotExists(string path)
    {
        if (!Directory.Exists(path))
        {
            throw new Exception($"Directory '{path}' does not exist.");
        }
    }
}
