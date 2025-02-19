﻿using Application.Common.Models.Auth;
using Application.Features.Auth.Commands.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IAuthenticationService
    {
     
        Task<string> CreateUserAsync(CreateUserDto createUserDto,CancellationToken cancellationToken);
        Task<string> GenerateEmailActivationTokenAsync(string UserId, CancellationToken cancellationToken);
        Task<bool> CheckIfUserExists(string email, CancellationToken cancellationToken);
        Task<JwtDto> LoginAsync(AuthLoginRequest authLoginRequest, CancellationToken cancellationToken);
    }
}
