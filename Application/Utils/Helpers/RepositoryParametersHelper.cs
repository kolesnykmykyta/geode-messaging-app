using Application.Utils.Helpers.Interfaces;

namespace Application.Utils.Helpers
{
    public class RepositoryParametersHelper : IRepositoryParametersHelper
    {
        public Dictionary<string, string> GenerateSearchParametersDictionary(string? generalSearch)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();
            if (generalSearch != null)
            {
                output["all"] = generalSearch;
            }

            return output;
        }

        public IEnumerable<string>? SplitSelectProperties(string? rawProperties)
        {
            return rawProperties?.Split(",");
        }
    }
}
