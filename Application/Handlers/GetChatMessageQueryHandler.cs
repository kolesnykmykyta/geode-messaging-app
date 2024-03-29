using Application.Handlers.Interfaces;
using Application.Services;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class GetChatMessageQueryHandler : IQueryHandler<GetChatMessagesQuery, IEnumerable<Message>>
    {
        public IEnumerable<Message> Process(GetChatMessagesQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
