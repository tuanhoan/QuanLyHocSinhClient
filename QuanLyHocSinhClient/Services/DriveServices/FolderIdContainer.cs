using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using QuanLyHocSinhClient.Services;
using QuanLyHocSinhClient.Services.Interface;

namespace QuanLyHocSinhClient.Services
{
    public class FolderIdContainer : DriveServices, IFolderIdContainer
    {
        readonly IFileIdInteractive _fileIdInteractive;
        public Dictionary<string, string> _folderIds { get; set; }

        string CustomerRootId;
        string ProjectRootId;

        public FolderIdContainer(IFileIdInteractive fileIdInteractive, IWebHostEnvironment env) : base()
        {
            _fileIdInteractive = fileIdInteractive;
            _folderIds = new();
            if (env.IsDevelopment())
            {
                ProjectRootId = "1mV8NlCW1c03K_CG6CbiEdIKp64fHO1AK";
                CustomerRootId = "1mV8NlCW1c03K_CG6CbiEdIKp64fHO1AK";
            }
            else
            {
                ProjectRootId = "1mV8NlCW1c03K_CG6CbiEdIKp64fHO1AK";
                CustomerRootId = "1mV8NlCW1c03K_CG6CbiEdIKp64fHO1AK";
            }
        }
        public string GetLoadFolderId(string key)
        {
            return _folderIds[key];
        }
        public async Task<string> ConvertPathToId(string path)
        {
            //_folderIds = await GetUploadContainerId(1,UploadClassify.DocumentData);
            var folder = path.Substring(0, path.LastIndexOf('/'));
            var fileName = path.Substring(path.LastIndexOf('/') + 1);

            return await _fileIdInteractive.GetFileIdAsync(fileName, parentId: _folderIds[folder]);
        }
        public async Task<Dictionary<string, string>> GetUploadContainerId(int id, UploadClassify classify)
        {
            _folderIds = new();
            string root;
            List<string> paths = new();
            if (classify == UploadClassify.CustomerData || classify == UploadClassify.DocumentData)
            {
                root = classify == UploadClassify.CustomerData ? CustomerRootId : ProjectRootId;
                paths.Add($"{id}/doc");
                paths.Add($"{id}/img");
            }
            else
            {
                root = CustomerRootId;
                paths.Add($"{id}/order");
            }

            foreach (var item in paths)
            {
                if (!_folderIds.ContainsKey(item))
                {
                    var temp = await _fileIdInteractive.CheckAndCreateFolder(item, root);
                    _folderIds.TryAdd(item, temp);
                }
            }


            return _folderIds;
        }
    }
}
