using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.Utils {

    public class HttpRequest {
        private string _address;
        private Uri _uri;

        public HttpRequest(string baseAddress) {
            _address = baseAddress;
            _uri = new Uri(_address);
        }

        public async Task<T> GetSingleDataByAsync<T>(string apiUrl, string token = null) {
            if (string.IsNullOrEmpty(this._address)) throw new Exception("Base url is empty.");

            try {
                using (HttpClient client = new HttpClient()) {
                    client.BaseAddress = this._uri;

                    if (!string.IsNullOrEmpty(token)) {
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer" + token);
                    }

                    var response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode) {
                        var data = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(data))
                            return JsonConvert.DeserializeObject<T>(data);
                    }
                }
            } catch {
                throw;
            }

            return default(T);
        }

        public async Task<List<T>> GetMultipleDataByAsync<T>(string apiUrl, string token = null) {
            if (string.IsNullOrEmpty(this._uri.AbsolutePath)) throw new Exception("Base Url is null");

            try {
                using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient()) {
                    client.BaseAddress = this._uri;

                    if (!string.IsNullOrEmpty(token)) {
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer" + token);
                    }

                    var response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode) {
                        var data = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(data)) {
                            return JsonConvert.DeserializeObject<List<T>>(data);
                        }
                    }
                }
            } catch {
                throw;
            }

            return default(List<T>);
        }

        public async Task<bool> PostByAsync(object data, string apiUrl, string token = null) {
            try {
                using (var client = new HttpClient()) {
                    client.BaseAddress = this._uri;

                    if (!string.IsNullOrEmpty(token)) {
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Add("Token", token);
                    }

                    var content = JsonConvert.SerializeObject(data);

                    HttpContent contentPost = new StringContent(content, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(apiUrl, contentPost);

                    if (response.IsSuccessStatusCode) {
                        return true;
                    }
                }
            } catch {
                throw;
            }

            return false;
        }

        public async Task<bool> PutByAsync(object data, string apiUrl, string token = null) {
            try {
                using (var client = new HttpClient()) {
                    client.BaseAddress = this._uri;

                    if (!string.IsNullOrEmpty(token)) {
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer" + token);
                    }

                    var content = JsonConvert.SerializeObject(data);

                    HttpContent contentPut = new StringContent(content, Encoding.UTF8, "application/json");

                    var response = await client.PutAsync(apiUrl, contentPut);

                    if (response.IsSuccessStatusCode) {
                        return true;
                    }
                }
            } catch {
                throw;
            }

            return false;
        }

        public async Task<bool> DeleteByAsync(string apiUrl, string key, string token = null) {
            try {
                using (var client = new HttpClient()) {
                    client.BaseAddress = this._uri;

                    if (!string.IsNullOrEmpty(token)) {
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer" + token);
                    }

                    //HttpRequestMessage request = new HttpRequestMessage {
                    //	Content = new StringContent("[YOUR JSON GOES HERE]", Encoding.UTF8, "application/json"),
                    //	Method = HttpMethod.Delete,
                    //	RequestUri = new Uri("[YOUR URL GOES HERE]")
                    //};
                    //var response = await client.SendAsync(request);

                    var response = await client.DeleteAsync(apiUrl + "/" + key);

                    if (response.IsSuccessStatusCode) {
                        return true;
                    }
                }
            } catch {
                throw;
            }

            return false;
        }
    }
}