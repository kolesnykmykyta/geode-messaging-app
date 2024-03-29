using Application.Handlers.Interfaces;
using Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class CreateChatCommandHandler : ICommandHandler<CreateChatCommand>
    {
        public bool Process(CreateChatCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
