using OnlineWebManagement.Broker.Services;

namespace OnlineWebManagement.Services.Services;

public class LocalStorageService : IStorageService
{
    private readonly IStorageBroker Storagebroker;

    public LocalStorageService()
    {
        Storagebroker = new LocalStorageBroker();
    }

    public void CreateFolder(string folderPath)
    {
        Storagebroker.CreateFolder(folderPath);
    }

    public void DeleteFolder(string folderPath)
    {
        Storagebroker.DeleteFolder(folderPath);
    }
}
