using Application.Dtos;
using System.Diagnostics.CodeAnalysis;

namespace Geode.Api.IntegrationTests.TestHelpers
{
    public class MessageDtoEqualityComparer : IEqualityComparer<MessageDto>
    {
        public bool Equals(MessageDto? x, MessageDto? y)
        {
            if (x == null)
            {
                return y == null;
            }
            else
            {
                return y != null &&
                    x.Content == y.Content;
            }
        }

        public int GetHashCode([DisallowNull] MessageDto obj)
        {
            return obj.GetHashCode();
        }
    }
}
