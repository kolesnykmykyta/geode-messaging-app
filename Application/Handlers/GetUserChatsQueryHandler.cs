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
    public class GetUserChatsQueryHandler : IQueryHandler<GetUserChatsQuery, IEnumerable<Chat>>
    {
        public IEnumerable<Chat> Process(GetUserChatsQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
