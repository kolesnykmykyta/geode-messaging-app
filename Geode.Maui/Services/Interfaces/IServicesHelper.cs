using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geode.Maui.Services.Interfaces
{
    internal interface IServicesHelper
    {
        Dictionary<string, string> CreateDictionaryFromObject(object obj);

        Task<TResult?> DeserializeJsonAsync<TResult>(HttpResponseMessage responseMessage);
    }
}
