namespace OnlineWebManagement.Broker.Services;

public class DropBoxStorageBroker : IStorageBroker
{
    public void CreateFolder(string folderPath)
    {
        throw new NotImplementedException();
    }

    public void DeleteFile(string filePath)
    {
        throw new NotImplementedException();
    }

    public void DeleteFolder(string folderPath)
    {
        throw new NotImplementedException();
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
}
