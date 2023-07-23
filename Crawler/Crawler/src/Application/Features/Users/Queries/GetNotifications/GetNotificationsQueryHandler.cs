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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.Features.Users.Queries.GetNotifications
{
    public class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, GetNotificationsDto>
    {
        private readonly UserManager<User> _userManager;
        private readonly ICurrentUserService _currentUserService;

        public GetNotificationsQueryHandler(UserManager<User> userManager, ICurrentUserService currentUserService)
        {
            _userManager=userManager;
            _currentUserService=currentUserService;
        }

        public async Task<GetNotificationsDto> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
        {
            User user = await _userManager.FindByIdAsync(_currentUserService.UserId.ToString());

            var userPreferences = new GetNotificationsDto(user.EmailNotification, user.ToasterNotification);

            return userPreferences;

        }
    }
}
