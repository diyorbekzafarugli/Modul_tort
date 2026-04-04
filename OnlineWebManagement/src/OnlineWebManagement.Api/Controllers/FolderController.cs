using Microsoft.AspNetCore.Mvc;
using OnlineWebManagement.Services.Services;

namespace OnlineWebManagement.Api.Controllers;

[ApiController]
[Route("api/folders")]
public class FolderController : ControllerBase
{
    private readonly IStorageService StorageService;

    public FolderController()
    {
        StorageService = new LocalStorageService();
    }

    [HttpPost()]
    public void AddFolder(string folderPath)
    {
        StorageService.CreateFolder(folderPath);
    }

    [HttpDelete()]
    public void DeleteFolder(string folderPath)
    {
        StorageService.DeleteFolder(folderPath);
    }
}
