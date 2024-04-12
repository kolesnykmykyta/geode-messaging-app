﻿using Application.Dtos;
using Application.Handlers.Chats;
using Application.Handlers.Messages;
using Application.Handlers.Users;
using Application.Services.Chats;
using Application.Services.Messages;
using Application.Services.Users;
using Application.Utils;
using Application.Utils.Automapper;
using Application.Utils.Helpers;
using Application.Utils.Helpers.Interfaces;
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
            services.AddScoped<IRequestHandler<JoinChatCommand, bool>, JoinChatCommandHandler>();
            services.AddScoped<IRequestHandler<LeaveChatCommand, bool>, LeaveChatCommandHandler>();
            services.AddScoped<IRequestHandler<SendMessageCommand, bool>, SendMessageCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateUsernameCommand, bool>, UpdateUsernameCommandHandler>();
            services.AddScoped<IRequestHandler<RegisterNewUserCommand, RegisterResultDto>, RegisterNewUserCommandHandler>();

            // CQRS Queries
            services.AddScoped<IRequestHandler<LoginQuery, TokenDto?>, LoginQueryHandler>();
            services.AddScoped<IRequestHandler<RefreshTokenQuery, TokenDto?>, RefreshTokenQueryHandler>();
            services.AddScoped<IRequestHandler<GetChatMessagesQuery, IEnumerable<Message>>, GetChatMessageQueryHandler>();
            services.AddScoped<IRequestHandler<GetUserChatsQuery, IEnumerable<ChatDto>>, GetUserChatsQueryHandler>();
            services.AddScoped<IRequestHandler<GetUsersListQuery, IEnumerable<UserInfoDto>>, GetUsersListQueryHandler>();
            services.AddScoped<IRequestHandler<GetUserMessagesQuery, IEnumerable<MessageDto>>, GetUserMessagesQueryHandler>();
        }

        public static void AddHelpers(this IServiceCollection services)
        {
            // Nuget packages
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
            services.AddAutoMapper(typeof(AutomapperProfile));

            // Own helpers
            services.AddScoped<IChatRepositoryHelper, ChatRepositoryHelper>();
            services.AddScoped<IApiUserHelper, ApiUserHelper>();
            services.AddScoped<IRepositoryParametersHelper, RepositoryParametersHelper>();
        }
    }
}
