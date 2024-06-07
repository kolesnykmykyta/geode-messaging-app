using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Utils.HttpClientWrapper
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        const string ApiBase = "https://geode-web-app.azurewebsites.net/api";

        public async Task<HttpResponseMessage> DeleteAsync(string url, string? accessToken = null)
        {
            using HttpClient httpClient = CreateHttpClient(accessToken);

            HttpResponseMessage response = await httpClient.DeleteAsync($"{ApiBase}/{url}");
            return response;
        }

        public async Task<HttpResponseMessage> GetAsync(string url, Dictionary<string, string>? queryParams = null, string? accessToken = null)
        {
            using HttpClient httpClient = CreateHttpClient(accessToken);

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

        public async Task<HttpResponseMessage> PostAsync(string url, object? content = null, string? accessToken = null)
        {
            using HttpClient httpClient = CreateHttpClient(accessToken);

            string? jsonContent = JsonSerializer.Serialize(content);
            StringContent requestContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync($"{ApiBase}/{url}", requestContent);
            return response;
        }

        public async Task<HttpResponseMessage> PutAsync(string url, object? content = null, string? accessToken = null)
        {
            using HttpClient httpClient = CreateHttpClient(accessToken);

            string? jsonContent = JsonSerializer.Serialize(content);
            StringContent requestContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PutAsync($"{ApiBase}/{url}", requestContent);
            return response;
        }

        public async Task<HttpResponseMessage> PostStreamAsync(string url, MultipartFormDataContent? content = null, string? accessToken = null)
        {
            using HttpClient httpClient = CreateHttpClient(accessToken);

            HttpResponseMessage response = await httpClient.PostAsync($"{ApiBase}/{url}", content);
            return response;
        }

        private HttpClient CreateHttpClient(string? accessToken)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return client;
        }
    }
}
