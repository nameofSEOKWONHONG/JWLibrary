using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.Utils {

    public class HttpRequest {
        private readonly string _address;
        private readonly Uri _uri;

        public HttpRequest(string baseAddress) {
            _address = baseAddress;
            _uri = new Uri(_address);
        }

        public async Task<T> GetSingleDataAsync<T>(string apiUrl, string token = null) {
            if (string.IsNullOrEmpty(_address)) throw new Exception("Base url is empty.");

            using (var client = new HttpClient()) {
                client.BaseAddress = _uri;

                if (!string.IsNullOrEmpty(token)) {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer" + token);
                }

                var response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode) {
                    var data = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(data))
                        return JsonConvert.DeserializeObject<T>(data);
                }
            }

            return default;
        }

        public async Task<List<T>> GetMultipleDataAsync<T>(string apiUrl, string token = null) {
            if (string.IsNullOrEmpty(_uri.AbsolutePath)) throw new Exception("Base Url is null");

            using (var client = new HttpClient()) {
                client.BaseAddress = _uri;

                if (!string.IsNullOrEmpty(token)) {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer" + token);
                }

                var response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode) {
                    var data = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(data)) return JsonConvert.DeserializeObject<List<T>>(data);
                }
            }

            return default;
        }

        public async Task<bool> PostAsync(object data, string apiUrl, string token = null) {
            using (var client = new HttpClient()) {
                client.BaseAddress = _uri;

                if (!string.IsNullOrEmpty(token)) {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Token", token);
                }

                var content = JsonConvert.SerializeObject(data);

                HttpContent contentPost = new StringContent(content, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, contentPost);

                if (response.IsSuccessStatusCode) return true;
            }

            return false;
        }

        public async Task<bool> PutAsync(object data, string apiUrl, string token = null) {
            using (var client = new HttpClient()) {
                client.BaseAddress = _uri;

                if (!string.IsNullOrEmpty(token)) {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer" + token);
                }

                var content = JsonConvert.SerializeObject(data);

                HttpContent contentPut = new StringContent(content, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(apiUrl, contentPut);

                if (response.IsSuccessStatusCode) return true;
            }

            return false;
        }

        public async Task<bool> DeleteAsync(string apiUrl, string key, string token = null) {
            using (var client = new HttpClient()) {
                client.BaseAddress = _uri;

                if (!string.IsNullOrEmpty(token)) {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer" + token);
                }

                //HttpRequestMessage request = new HttpRequestMessage {
                //	Content = new StringContent("[YOUR JSON GOES HERE]", Encoding.UTF8, "application/json"),
                //	Method = HttpMethod.Delete,
                //	RequestUri = new Uri("[YOUR URL GOES HERE]")
                //};
                //var response = await client.SendAsync(request);

                var response = await client.DeleteAsync(apiUrl + "/" + key);

                if (response.IsSuccessStatusCode) return true;
            }

            return false;
        }
    }
}