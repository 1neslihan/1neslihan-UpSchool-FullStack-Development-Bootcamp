using Application.Common.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth.Commands.Login
{
    public class AuthLoginCommandValidator : AbstractValidator<AuthLoginCommand>
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthLoginCommandValidator(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            RuleFor(x=>x.Email)
                .NotEmpty()
                .WithMessage("your email or password is incorrect");
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("your email or password is incorrect");
            RuleFor(x => x.Email)
                .MustAsync(CheckIfUserExists)
                .WithMessage("your email or password is incorrect");
        }
        private Task<bool> CheckIfUserExists(string email, CancellationToken cancellationToken)
        {
            return _authenticationService.CheckIfUserExists(email, cancellationToken);
        }
    }
}
