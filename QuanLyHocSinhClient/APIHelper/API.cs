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
        static HttpClient httpClient;
        private static API<T> qLDiem; 
        public static API<T> QLHocSinh
        {
            get {  qLDiem = new API<T>("https://quanlyhocsinh.azurewebsites.net/");
                httpClient = new HttpClient() { BaseAddress = new Uri("https://quanlyhocsinh.azurewebsites.net/") }; 
                return qLDiem; }
            set => qLDiem = value;
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
