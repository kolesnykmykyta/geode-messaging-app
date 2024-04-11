using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils.Helpers.Interfaces
{
    public interface IRepositoryParametersHelper
    {
        IEnumerable<string>? SplitSelectProperties(string? rawProperties);

        Dictionary<string, string> GenerateSearchParametersDictionary(string? generalSearch);
    }
}
