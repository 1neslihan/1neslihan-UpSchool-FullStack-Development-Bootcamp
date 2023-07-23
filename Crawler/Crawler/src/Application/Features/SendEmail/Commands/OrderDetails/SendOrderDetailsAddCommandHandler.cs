using Application.Common.Interfaces;
using Application.Common.Models.Email;
using Application.Features.SendEmail.Commands.Add;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SendEmail.Commands.OrderDetails
{
    public class SendOrderDetailsAddCommandHandler : IRequestHandler<SendOrderDetailsAddCommand, SendOrderDetailsAddDto>
    {
        private readonly IEmailService _emailService;

        public SendOrderDetailsAddCommandHandler(IEmailService emailService)
        {
            _emailService=emailService;
        }

        public async Task<SendOrderDetailsAddDto> Handle(SendOrderDetailsAddCommand request, CancellationToken cancellationToken)
        {
            _emailService.SendOrderDetails(new SendOrderDetailsDto()
            {
                Name = request.Name,
                Email = request.Email,
                OrderId = request.OrderId,
                GenerateDate= request.GenerateDate,
                Products = request.Products,
                OrderEvents=request.OrderEvents,

            });

            return new SendOrderDetailsAddDto(request.Name, request.Email, request.OrderId, request.Products, request.OrderEvents);
        }
    }
}
