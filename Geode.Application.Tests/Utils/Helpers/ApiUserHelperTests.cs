using Application.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Geode.Application.Tests.Utils.Helpers
{
    public class ApiUserHelperTests
    {
        [Fact]
        public void ExtractIdFromUser_UserWithId_ReturnsId()
        {
            string expected = "ClaimId";
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, expected)
            };
            ClaimsIdentity testIdentity = new ClaimsIdentity(claims);
            ClaimsPrincipal testPrincipal = new ClaimsPrincipal(testIdentity);
            ApiUserHelper sut = new ApiUserHelper();

            string actual = sut.ExtractIdFromUser(testPrincipal);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ExtractNameFromUser_UserWithName_ReturnsName()
        {
            string expected = "ClaimName";
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, expected)
            };
            ClaimsIdentity testIdentity = new ClaimsIdentity(claims);
            ClaimsPrincipal testPrincipal = new ClaimsPrincipal(testIdentity);
            ApiUserHelper sut = new ApiUserHelper();

            string actual = sut.ExtractNameFromUser(testPrincipal);

            Assert.Equal(expected, actual);
        }
    }
}
