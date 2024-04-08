using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Utils.HttpClientWrapper
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        const string ApiBase = "https://localhost:7077/api";

        public async Task<HttpResponseMessage> GetAsync(string url, Dictionary<string, string>? queryParams = null)
        {
            using HttpClient httpClient = new HttpClient();

            string requestUrl;
            if (queryParams != null && queryParams.Count != 0)
            {
                string queryParamsString = string.Join("&", queryParams.Select(pair => $"{pair.Key}={pair.Value}"));
                requestUrl = $"{ApiBase}/{url}?{queryParamsString}";
            }
            else
            {
                requestUrl = $"{ApiBase}/{url}";
            }

            HttpResponseMessage response = await httpClient.GetAsync(requestUrl);
            return response;
        }

        public async Task<HttpResponseMessage> PostAsync(string url, object? content = null)
        {
            using HttpClient httpClient = new HttpClient();

            string? jsonContent = JsonSerializer.Serialize(content);
            StringContent requestContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync($"{ApiBase}/{url}", requestContent);
            return response;
        }
    }
}
