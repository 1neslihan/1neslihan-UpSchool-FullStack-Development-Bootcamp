using Application.Common.Interfaces;
using Application.Common.Models.Email;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SendEmail.Commands.Add
{
    public class SendEmailAddCommandHandler : IRequestHandler<SendEmailAddCommand, SendEmailAddDto>
    {
        private readonly IEmailService _emailService;

        public SendEmailAddCommandHandler(IEmailService emailService)
        {
            _emailService=emailService;
        }

        public async Task<SendEmailAddDto> Handle(SendEmailAddCommand request, CancellationToken cancellationToken)
        {
            var fullName = $"{request.FirstName} {request.LastName}";
            _emailService.SendEmailConfirmation(new SendEmailConfirmationDto()
            {
                Email=request.Email,
                Name=request.FirstName,
                Link=request.Link
                

            });

            return new SendEmailAddDto(request.FirstName, request.LastName, request.Email, request.Link);
        }

    }
}
