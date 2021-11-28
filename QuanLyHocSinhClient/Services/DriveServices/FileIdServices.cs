using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 
using System.IO;
using Google.Apis.Drive.v3;
using Google;
using QuanLyHocSinhClient.Services;
using QuanLyHocSinhClient.Services.Interface;

namespace QuanLyHocSinhClient.Services
{
    public class FileIdServices : DriveServices, IFileIdInteractive
    {
        public FileIdServices() : base()
        {
        }
        public async Task<string> CheckAndCreateFolder(string path, string parentId = null)
        {
            string folderId = null;
            string[] directoryStructure = path.Split('/');

            for (int i = 0; i < directoryStructure.Length; i++)
            {
                folderId = await GetFileIdAsync($"{directoryStructure[i]}", isFolder: true, parentId);
                if (String.IsNullOrEmpty(folderId)) folderId = CreateFolder($"{directoryStructure[i]}", parentId);
                parentId = folderId;
            }
            return folderId;

        }
        public string CreateFolder(string FolderName, string parentId = null)
        {
            var FileMetaData = new Google.Apis.Drive.v3.Data.File()
            {
                Name = Path.GetFileName(FolderName),
                MimeType = "application/vnd.google-apps.folder",
                Parents = new List<string>
                   {
                       parentId
                   }
            };

            FilesResource.CreateRequest request;

            request = service.Files.Create(FileMetaData);
            request.Fields = "id";
            var file = request.Execute();
            return file.Id;
        }
        public async Task<string> GetFileIdFromPath(string path, string parentId)
        {
            string fileId = null;
            string[] directoryStructure = path.Split('/');

            for (int i = 0; i < directoryStructure.Length; i++)
            {
                if (i != directoryStructure.Length - 1)
                    fileId = await GetFileIdAsync($"{directoryStructure[i]}", isFolder: true, parentId: parentId);
                else
                    fileId = await GetFileIdAsync($"{directoryStructure[i]}", parentId: parentId);

                if (fileId == null) break;
                parentId = fileId;
            }
            return fileId;
        }

        public async Task<string> GetFileIdAsync(string name, bool isFolder = false, string parentId = null)
        {
            string fileId;
            string folderMime = "";

            Random rnd = new();
            if (isFolder) folderMime = "mimeType='application/vnd.google-apps.folder' and";
            // Define parameters of request.    
            FilesResource.ListRequest FileListRequest = service.Files.List();
            // for getting folders only.    
            FileListRequest.Q = $"{folderMime} name = '{name}' and '{parentId}' in parents";
            FileListRequest.Fields = "nextPageToken, files(id)";

            Google.Apis.Drive.v3.Data.FileList taskFiles = new();
            while (true)
            {
                try
                {
                    taskFiles = await FileListRequest.ExecuteAsync();
                    break;
                }
                catch (GoogleApiException)
                {

                    await Task.Delay(rnd.Next(10, 100));
                    continue;
                }
            }
            // List files. 
            fileId = taskFiles.Files.FirstOrDefault()?.Id;
            return fileId;
        }
    }
}
