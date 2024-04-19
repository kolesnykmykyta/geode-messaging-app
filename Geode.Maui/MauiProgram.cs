using Application.Utils.HttpClientWrapper;
using Blazored.LocalStorage;
using Geode.Maui.Authentication;
using Geode.Maui.Services;
using Geode.Maui.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;

namespace Geode.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            builder.Services.AddAuthorizationCore();

            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

            builder.Services.AddScoped<IHttpClientWrapper, HttpClientWrapper>();

            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

            builder.Services.AddScoped<IUsersService, UserService>();
            builder.Services.AddScoped<IMessagesService, MessagesService>();
            builder.Services.AddScoped<IChatsService, ChatsService>();
            builder.Services.AddScoped<IServicesHelper, ServicesHelper>();
            builder.Services.AddTransient<IChatHubService, ChatHubService>();

            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddMudServices();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
