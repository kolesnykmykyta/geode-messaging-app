using Application.Handlers;
using Application.Services;
using Application.Utils;
using Auth.Dtos;
using DataAccess.Entities;
using MediatR;

namespace Geode.API.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddCqrsServices(this IServiceCollection services)
        {
            // CQRS Commands
            services.AddScoped<IRequestHandler<ChangeChatNameCommand, bool>, ChangeChatNameCommandHandler>();
            services.AddScoped<IRequestHandler<CreateChatCommand, bool>, CreateChatCommandHandler>();
            services.AddScoped<IRequestHandler<GetChatMessagesQuery, IEnumerable<Message>>, GetChatMessageQueryHandler>();
            services.AddScoped<IRequestHandler<GetUserChatsQuery, IEnumerable<Chat>>, GetUserChatsQueryHandler>();
            services.AddScoped<IRequestHandler<JoinChatCommand, bool>, JoinChatCommandHandler>();
            services.AddScoped<IRequestHandler<LeaveChatCommand>, LeaveChatCommandHandler>();
            services.AddScoped<IRequestHandler<SendMessageCommand, bool>, SendMessageCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateUsernameCommand, bool>, UpdateUsernameCommandHandler>();
            services.AddScoped<IRequestHandler<RegisterNewUserCommand, RegisterResultDto>, RegisterNewUserCommandHandler>();

            // CQRS Queries
            services.AddScoped<IRequestHandler<LoginQuery, string?>, LoginQueryHandler>();
            services.AddScoped<IRequestHandler<GetChatMessagesQuery, IEnumerable<Message>>, GetChatMessageQueryHandler>();
            services.AddScoped<IRequestHandler<GetUserChatsQuery, IEnumerable<Chat>>, GetUserChatsQueryHandler>();
        }
    }
}
