using Application.Handlers.Interfaces;
using Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils
{
    public class CommonMessageHandler
    {
        private readonly IServiceProvider _provider;

        public CommonMessageHandler(IServiceProvider provider)
        {
            _provider = provider;
        }

        public bool Handle(ICommand command)
        {
            Type type = typeof(ICommandHandler<>);
            Type[] typeArgs = { command.GetType() };
            Type handlerType = type.MakeGenericType(typeArgs);

            dynamic? handler = _provider.GetService(handlerType);
            bool result = handler!.Handle((dynamic)command);

            return result;
        }

        public T Handle<T>(IQuery<T> query)
        {
            Type type = typeof(IQueryHandler<,>);
            Type[] typeArgs = { query.GetType(), typeof(T) };
            Type handlerType = type.MakeGenericType(typeArgs);

            dynamic? handler = _provider.GetService(handlerType);
            T result = handler!.Handle((dynamic)query);

            return result;
        }
    }
}
