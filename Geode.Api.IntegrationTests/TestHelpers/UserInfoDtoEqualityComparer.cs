using Application.Dtos;
using System.Diagnostics.CodeAnalysis;

namespace Geode.Api.IntegrationTests.TestHelpers
{
    public class UserInfoDtoEqualityComparer : IEqualityComparer<UserInfoDto>
    {
        public bool Equals(UserInfoDto? x, UserInfoDto? y)
        {
            if (x == null)
            {
                return y == null;
            }
            else
            {
                return y != null &&
                    x.UserName == y.UserName &&
                    x.Email == y.Email &&
                    x.PhoneNumber == y.PhoneNumber;
            }
        }

        public int GetHashCode([DisallowNull] UserInfoDto obj)
        {
            throw new NotImplementedException();
        }
    }
}
