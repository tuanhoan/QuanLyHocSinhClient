using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace QuanLyHocSinhClient.APIHelper
{
    public class API<T>
    {
        readonly HttpClient _httpClient;
        private static API<T> instance;
        private T _data;
        private string URL_API = "https://quanlyhocsinh.azurewebsites.net/";
        public static API<T> Instance
        {
            get { return instance ??= new API<T>(); }
            set => instance = value;
        }

        private API()
        {
            _httpClient = new HttpClient() { BaseAddress = new Uri(URL_API) }; 
        }
        public async Task<T> GetAsync(string url, Dictionary<string, string> headers = null)
        {
            try
            {
                HttpResponseMessage response = _httpClient.GetAsync(url).Result;
                string responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    _data = JsonConvert.DeserializeObject<T>(responseContent);
                }

                return _data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task<T> PostAsync(string url, T request, Dictionary<string, string> headers = null)
        {
            try
            { 
                HttpContent httpContent = null;
                if (request != null)
                {
                    string json = JsonConvert.SerializeObject(request);
                    httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                }
                HttpResponseMessage responseMessage = await _httpClient.PostAsync(url, httpContent);
                var responseContent = await responseMessage.Content.ReadAsStringAsync();

                if (responseMessage.IsSuccessStatusCode)
                {
                    _data = JsonConvert.DeserializeObject<T>(responseContent);
                }

                return _data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<string> DeleteAsync(string url, Dictionary<string, string> headers = null)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();
                 
                if (response.IsSuccessStatusCode)
                {
                    return "OK"; 
                } 
                return "Error";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<T> PutAsync(string url, T request, Dictionary<string, string> headers = null)
        {
            try
            { 
                HttpContent httpContent = null;
                if (request != null)
                {
                    string json = JsonConvert.SerializeObject(request);
                    httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                }

                HttpResponseMessage responseMessage = await _httpClient.PutAsync(url, httpContent);

                var responseContent = await responseMessage.Content.ReadAsStringAsync();

                if (responseMessage.IsSuccessStatusCode)
                {
                    _data = JsonConvert.DeserializeObject<T>(responseContent);
                } 
                return _data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
    }
}
