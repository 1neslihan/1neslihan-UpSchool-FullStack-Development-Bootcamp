using Application.Common.Interfaces;
using Application.Features.OrderEvents.Queries.GetAll;
using Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<GetAllUsersDto>>
    {
        private readonly UserManager<User> _userManager;

        public GetAllUsersQueryHandler(UserManager<User> userManager)
        {
            _userManager=userManager;
        }

        public async Task<List<GetAllUsersDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var user = _userManager.Users.AsQueryable();
            var userRecords= user
                .Select(x=>new GetAllUsersDto(x.FirstName, x.LastName, x.Email)).ToList();

            return userRecords;
        }
    }
}
