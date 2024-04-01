using Application.Handlers;
using Application.Services;
using Application.Utils;

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

            services.AddSingleton<CommonMessageHandler>();
        }
    }
}
