using Application.Dtos;
using Azure;
using Geode.Maui.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Geode.Maui.Services
{
    internal class ServicesHelper : IServicesHelper
    {
        public Dictionary<string, string> CreateDictionaryFromObject(object obj)
        {
            Type type = obj.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var output = new Dictionary<string, string>();

            foreach (var prop in properties)
            {
                var value = prop.GetValue(obj);
                if (value != null)
                {
                    output.Add(prop.Name, value.ToString());
                }
            }

            return output;
        }

        public async Task<TResult?> DeserializeJsonAsync<TResult>(HttpResponseMessage responseMessage)
        {
            string jsonResponseString = await responseMessage.Content.ReadAsStringAsync();
            TResult? responseBody = JsonSerializer.Deserialize<TResult>(jsonResponseString);
            return responseBody;
        }
    }
}
