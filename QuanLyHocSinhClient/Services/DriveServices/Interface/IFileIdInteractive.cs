using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyHocSinhClient.Services.Interface
{
    public interface IFileIdInteractive
    {
        public Task<string> GetFileIdAsync(string name, bool isFolder = false, string parentId = null);
        public string CreateFolder(string FolderName, string parentId = null);
        public Task<string> GetFileIdFromPath(string path, string parentId);
        public Task<string> CheckAndCreateFolder(string path, string parentId);
    }
}
