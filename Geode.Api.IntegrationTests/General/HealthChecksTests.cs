using Geode.API;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Geode.Api.IntegrationTests.General
{
    public class HealthChecksTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;

        public HealthChecksTests(WebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateDefaultClient();
        }

        [Fact]
        public async Task HealthCheck_ReturnsOk()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/health");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
