using Application.Common.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth.Commands.Register
{
    public class AuthRegisterCommandValidator : AbstractValidator<AuthRegisterCommand>
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthRegisterCommandValidator(IAuthenticationService authenticationService)
        {
            _authenticationService= authenticationService;
            RuleFor(x => x.Email)
                .MustAsync(CheckIfUserExists)
                .WithMessage("This email all ready been registered.");
        }

        private async Task<bool> CheckIfUserExists(string email, CancellationToken cancellationToken)
        {
            var doesExist=await _authenticationService.CheckIfUserExists(email, cancellationToken);
            if(doesExist)
            {
                return false;
            }
            return true;
        }
    }
}
