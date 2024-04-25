using Application.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Geode.Application.Tests.Utils.Helpers
{
    public class RepositoryParametersHelperTests
    {
        [Theory]
        [InlineData(" ")]
        [InlineData(".")]
        [InlineData("")]
        [InlineData("Test")]
        public void GenerateSearchParametersDictionary_AnyString_ReturnsDictionaryWithOnePair(string searchParam)
        {
            Dictionary<string, string> expected = new Dictionary<string, string>
            {
                {"all", searchParam},
            };
            RepositoryParametersHelper sut = new RepositoryParametersHelper();

            Dictionary<string, string> actual = sut.GenerateSearchParametersDictionary(searchParam);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GenerateSearchParametersDictionary_NullArgument_ReturnsEmptyDictionary()
        {
            Dictionary<string, string> expected = new Dictionary<string, string>();
            RepositoryParametersHelper sut = new RepositoryParametersHelper();

            Dictionary<string, string> actual = sut.GenerateSearchParametersDictionary(null);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("test1,test2,test3")]
        [InlineData("")]
        [InlineData(",")]
        [InlineData("nocomma")]
        public void SplitSelectProperties_AnyString_SplitsStringByComma(string selectProps)
        {
            IEnumerable<string> expected = selectProps.Split(",");
            RepositoryParametersHelper sut = new RepositoryParametersHelper();

            IEnumerable<string> actual = sut.SplitSelectProperties(selectProps);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SplitSelectProperties_NullArgument_ReturnsNull()
        {
            RepositoryParametersHelper sut = new RepositoryParametersHelper();

            IEnumerable<string>? actual = sut.SplitSelectProperties(null);

            Assert.Null(actual);
        }
    }
}
