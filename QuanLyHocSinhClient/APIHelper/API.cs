using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuanLyHocSinhClient.Models;

namespace QuanLyHocSinhClient.APIHelper
{
    public class API<T>
    {
        readonly HttpClient _httpClient;
        private static API<T> instance;
        private T _data;
        private string URL_API = "https://quanlyhocsinh.azurewebsites.net/";
        public static Dictionary<string, string> headers; 
        public static API<T> Instance
        {
            get { if (instance == null) instance = new API<T>(); return instance; }
            set => instance = value;
        }

        private API()
        {
            _httpClient = new HttpClient() { BaseAddress = new Uri(URL_API) };
            AddHeaders(_httpClient, headers);
        }
        public async Task<string> Login(string url, Account request, Dictionary<string, string> headers = null)
        {
            try
            {
                HttpContent httpContent = null;
                if (request != null)
                {
                    string json = JsonConvert.SerializeObject(request);
                    httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                }
                HttpResponseMessage responseMessage= await _httpClient.PostAsync(url, httpContent);
                var responseContent = await responseMessage.Content.ReadAsStringAsync();

                if (responseMessage.IsSuccessStatusCode)
                {
                    return responseContent.Replace("\"", "");
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public void AddHeaders(HttpClient httpClient, Dictionary<string, string> headers)
        {
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            if (!string.IsNullOrEmpty(Program.token))
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Program.token);
            }
            //No additional headers to be added
            if (headers == null)
                return;
            foreach (KeyValuePair<string, string> current in headers)
            {
                httpClient.DefaultRequestHeaders.Add(current.Key, current.Value);
            }
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
