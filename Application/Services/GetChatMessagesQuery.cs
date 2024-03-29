using Application.Services.Interfaces;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class GetChatMessagesQuery : IQuery<IEnumerable<Message>>
    {
    }
}
