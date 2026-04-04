using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineWebManagement.Services.Services;

public interface IStorageService
{
    void CreateFolder(string folderPath);
    void DeleteFolder(string folderPath);
}
