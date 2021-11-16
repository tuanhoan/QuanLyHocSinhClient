using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace QuanLyHocSinhClient.APIHelper
{
    public class API<T>
    {
        Uri baseUrl = new Uri("https://quanlydiem.azurewebsites.net/");
        public static Dictionary<string, string> headers;
        HttpClient httpClient;
        private static API<T> qLDiem; 
        public static API<T> QLDiem
        {
            get { if (qLDiem == null) qLDiem = new API<T>("https://quanlydiem.azurewebsites.net/"); return qLDiem; }
            set => qLDiem = value;
        }

        private static API<T> qLTKB;
        public static API<T> QLTKB
        {
            get { if (qLTKB == null) qLTKB = new API<T>("https://quanlytkb.azurewebsites.net/"); return qLTKB; }
            set => qLTKB = value;
        }

        private static API<T> qLHocSinh;
        public static API<T> QLHocSinh
        {
            get { if (qLHocSinh == null) qLHocSinh = new API<T>("https://quanlyhocsinh.azurewebsites.net/"); return qLHocSinh; }
            set => qLHocSinh = value;
        }


        public API(string url)
        {
            httpClient = new HttpClient() { BaseAddress = baseUrl };
            //AddHeaders(httpClient, headers);
        }

        public async Task<T> GetAsync(string url, Dictionary<string, string> headers = null)
        {
            try
            {
                HttpResponseMessage response = httpClient.GetAsync(url).Result;
                string responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(responseContent);
                }
                throw new Exception(response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
    }
}
