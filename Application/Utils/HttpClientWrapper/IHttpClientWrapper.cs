using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils.HttpClientWrapper
{
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> PostAsync(string url, object? content = null, string? accessToken = null);

        Task<HttpResponseMessage> PostStreamAsync(string url, MultipartFormDataContent? content = null, string? accessToken = null);

        Task<HttpResponseMessage> PutAsync(string url, object? content = null, string? accessToken = null);

        Task<HttpResponseMessage> DeleteAsync(string url, string? accessToken = null);

        Task<HttpResponseMessage> GetAsync(string url, Dictionary<string, string>? queryParams = null, string? accessToken = null);
    }
}
