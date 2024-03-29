using Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.Interfaces
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        bool Process(TCommand command);
    }
}
