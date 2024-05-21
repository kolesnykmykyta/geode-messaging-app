using MediatR;

namespace Application.CqrsMessages.Users
{
    public class UpdateUserDataCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;
    }
}
