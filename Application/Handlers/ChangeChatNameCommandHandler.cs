using Application.Handlers.Interfaces;
using Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class ChangeChatNameCommandHandler : ICommandHandler<ChangeChatNameCommand>
    {
        public bool Process(ChangeChatNameCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
