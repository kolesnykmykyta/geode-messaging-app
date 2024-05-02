using Application.Dtos;
using DataAccess.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Geode.Api.IntegrationTests.TestHelpers
{
    public class ChatEqualityComparer : IEqualityComparer<ChatDto>
    {
        public bool Equals(ChatDto? x, ChatDto? y)
        {
            if (x == null)
            {
                return y == null;
            }
            else
            {
                return y != null &&
                    x.Id == y.Id &&
                    x.Name == y.Name;
            }
        }

        public int GetHashCode([DisallowNull] ChatDto obj)
        {
            return obj.GetHashCode();
        }
    }
}
