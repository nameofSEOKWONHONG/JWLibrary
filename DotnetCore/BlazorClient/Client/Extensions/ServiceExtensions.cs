using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = authorization;

            var response = await httpClient.SendAsync(request);
            var responseBytes = await response.Content.ReadAsByteArrayAsync();
            return JsonSerializer.Deserialize<T>(responseBytes, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }
    }
}
