using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorClient.Client.Extensions {
    public static class ServiceExtensions {
        public static async Task<T> GetJsonAsync<T>(this HttpClient httpClient, string url, AuthenticationHeaderValue authorization) {
            var requestMessage = new HttpRequestMessage() {
                Method = new HttpMethod("GET"),
                RequestUri = new Uri(url)
            };
            requestMessage.Headers.Clear();
            requestMessage.Headers.Authorization = authorization;
            var response = await httpClient.SendAsync(requestMessage);
            var contents = response.Content;
            var responseString = await contents.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseString, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }
    }
}
