using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyHocSinhClient.Services.Interface
{
    public interface IFolderIdContainer
    {
        public Task<string> ConvertPathToId(string path);
        public Task<Dictionary<string, string>> GetUploadContainerId(int id, UploadClassify classify);
        public string GetLoadFolderId(string key);
    }
}
