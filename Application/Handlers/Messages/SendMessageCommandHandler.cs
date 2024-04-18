using Application.Dtos;
using Application.Services.Messages;
using AutoMapper;
using DataAccess.Entities;
using DataAccess.UnitOfWork;
using MediatR;

namespace Application.Handlers.Messages
{
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public SendMessageCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            Message newMessage = _mapper.Map<Message>(request);
            newMessage.SentAt = DateTime.Now;
            _unitOfWork.GenericRepository<Message>().Insert(newMessage);
            _unitOfWork.SaveChanges();
        }
    }
}
