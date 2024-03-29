using Application.Handlers.Interfaces;
using Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    internal class UpdateUsernameCommandHandler : ICommandHandler<UpdateUsernameCommand>
    {
        public bool Process(UpdateUsernameCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
