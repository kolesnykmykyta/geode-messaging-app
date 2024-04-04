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
        public async Task<HttpResponseMessage> PostAsync(string url, object content)
        {
            using HttpClient httpClient = new HttpClient();

            string? jsonContent = JsonSerializer.Serialize(content);
            StringContent requestContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(url, requestContent);
            return response;
        }
    }
}
