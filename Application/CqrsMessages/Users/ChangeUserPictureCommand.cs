﻿using MediatR;

namespace Application.CqrsMessages.Users
{
    public class ChangeUserPictureCommand : IRequest
    {
        public Stream? PictureStream { get; set; }

        public string OriginalName { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;
    }
}
