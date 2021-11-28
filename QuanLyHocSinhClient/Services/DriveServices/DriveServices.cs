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
 

namespace QuanLyHocSinhClient.Services
{
    public enum UploadClassify
    {
        CustomerData,
        DocumentData,
        OrderData
    }
    public class DriveServices
    {
        protected static string[] scopes = { DriveService.Scope.Drive };
        protected readonly UserCredential credential;
        static string ApplicationName = "MyApplicationName";
        protected readonly DriveService service;

        public DriveServices()
        {
            using (var stream =
                new FileStream("./wwwroot/credentials/gg_credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "./wwwroot/credentials/token.json";

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    scopes,
                    "tuanhoan.tgl@gmail.com", // use a const or read it from a config file
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }
    }
}
