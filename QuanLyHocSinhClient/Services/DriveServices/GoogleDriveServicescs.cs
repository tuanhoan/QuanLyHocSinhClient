using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Web; 
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Google.Apis.Drive.v3.Data;
using Microsoft.AspNetCore.Mvc.Formatters;
using Google;
using QuanLyHocSinhClient.Services;
using QuanLyHocSinhClient.Services.Interface;


namespace QuanLyHocSinhClient.Services
{
    public class GoogleDriveServices : DriveServices
    {
        protected readonly IWebHostEnvironment _env;
        readonly IFolderIdContainer _folderIdContainer;

        //protected string CustomerData = "1zTZDpx9CF710JzjIkKu84kZpVpGANDTr";
        protected string CustomerData;
        protected string DocumentData;
        public int CustomerId { get; set; }
        public GoogleDriveServices(IWebHostEnvironment env, IFolderIdContainer folderIdContainer) : base()
        {
            _folderIdContainer = folderIdContainer;
        }
        public async Task PrepareFolderId(int id, UploadClassify classify)
        {
            await _folderIdContainer.GetUploadContainerId(id, classify);
        }
        public async Task<List<string>> UploadRangeAsync(List<IBrowserFile> files, UploadClassify classify, int id)
        {
            await PrepareFolderId(id, classify);

            List<string> result = new();
            List<Task<string>> tasks = new();
            foreach (var item in files)
            {
                var path = $"{id}";
                if (classify == UploadClassify.OrderData)
                {
                    path += "/order";
                }
                else if (item.ContentType.Contains("image"))
                {
                    path += "/img";
                }
                else
                {
                    path += "/doc";
                }
                tasks.Add(Upload(item, _folderIdContainer.GetLoadFolderId(path), item.Name));
                path += $"/{item.Name}";
                result.Add(path);
            }
            await Task.WhenAll(tasks);
            if (tasks.Any(c => string.IsNullOrEmpty(c.Result)))
            {
                tasks.ForEach(async c => await DeleteFile(c.Result));
                return new List<string>();
            }
            else
            {
                return result;
            }
        }
        public async Task<string> Upload(IBrowserFile file, string folderId, string name = null)
        {
            bool isLoadComplete = false;

            if (name == null) name = $"{DateTime.Now.ToString("ddMMyyyyHHmmss")}{Path.GetExtension(file.Name)}";

            var mimeType = file.ContentType;

            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = name,
                MimeType = mimeType,
                Parents = new[] { folderId }
            };

            FilesResource.CreateMediaUpload request;
            using (var stream = file.OpenReadStream(10240000))
            {
                request = service.Files.Create(
                    fileMetadata, stream, mimeType);
                request.Fields = "id, name, parents, createdTime, modifiedTime, mimeType";
                request.IncludePermissionsForView = "published";

                var random = new Random();
                int i = 0;
                while (!isLoadComplete || i == 10)
                {
                    try
                    {
                        await request.UploadAsync();
                    }
                    catch (Google.GoogleApiException)
                    {
                        await Task.Delay(random.Next(10, 100));
                        i++;
                        continue;
                    }
                    isLoadComplete = true;
                }

            }
            return request.ResponseBody?.Id;
            //Permission newPermission = new()
            //{
            //    Type = "anyone",
            //    Role = "reader"
            //};
            //service.Permissions.Create(newPermission, request.ResponseBody?.Id).Execute();
        }
        public async Task<List<(string Link, string PlaceHolder, string Path)>> GetFilesMetadataAsync(List<string> paths, UploadClassify classify, int id)
        {
            List<(string Link, string PlaceHolder, string Path)> result = new();
            List<Task<(string Link, string PlaceHolder, string Path)>> tasks = new();
            foreach (var item in paths)
            {
                tasks.Add(GetFileMetadataAsync(item));
            }
            await Task.WhenAll(tasks);
            return tasks.Select(c => c.Result)?.ToList();
        }
        public async Task<(string Link, string PlaceHolder, string Path)> GetFileMetadataAsync(string path)
        {
            string extension = Path.GetExtension(path);

            var pathStructure = path.Split("/");

            string fileId = await _folderIdContainer.ConvertPathToId(path);


            bool isLoadComplete = false;
            Random rnd = new();
            FilesResource.GetRequest filesResource = service.Files.Get(fileId);
            filesResource.Fields = "thumbnailLink, webContentLink, name, webViewLink, mimeType";

            Google.Apis.Drive.v3.Data.File file = new();
            while (!isLoadComplete)
            {
                try
                {
                    //file = await filesResource.ExecuteAsync();
                    file = filesResource.Execute();
                }
                catch (Google.GoogleApiException)
                {
                    await Task.Delay(rnd.Next(10, 100));
                    continue;
                }
                isLoadComplete = true;
            }
            if (file.MimeType.Contains("image"))
            {
                return (file.WebContentLink, file.ThumbnailLink, path);
            }
            return (file.WebViewLink, file.Name, path);
        }
        public async Task DeleteFile(string id)
        {
            if (id == null) return;
            var request = service.Files.Delete(id);
            await request.ExecuteAsync();
        }
        public async Task DeleteFilesWithListPath(List<string> listPaths)
        {
            foreach (var path in listPaths)
            {
                await DeleteFile(await _folderIdContainer.ConvertPathToId(path));
            }
        }
    }
}
