using Application.Handlers;
using Application.Services;
using Application.Utils;
using Auth.Dtos;
using MediatR;

namespace Geode.API.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddCqrsServices(this IServiceCollection services)
        {
            services.AddScoped<ChangeChatNameCommandHandler>();
            services.AddScoped<CreateChatCommandHandler>();
            services.AddScoped<GetChatMessageQueryHandler>();
            services.AddScoped<GetUserChatsQueryHandler>();
            services.AddScoped<JoinChatCommandHandler>();
            services.AddScoped<SendMessageCommandHandler>();
            services.AddScoped<UpdateUsernameCommand>();
            services.AddScoped<IRequestHandler<RegisterNewUserCommand, RegisterResultDto>, RegisterNewUserCommandHandler>();
            services.AddScoped<IRequestHandler<LoginQuery, string?>, LoginQueryHandler>();

            services.AddSingleton<CommonMessageHandler>();
        }
    }
}
