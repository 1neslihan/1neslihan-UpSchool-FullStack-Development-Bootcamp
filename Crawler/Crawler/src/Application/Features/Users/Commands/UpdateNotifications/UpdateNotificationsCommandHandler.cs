using Application.Common.Interfaces;
using Domain.Common;
using Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.UpdateNotifications
{
    public class UpdateNotificationsCommandHandler : IRequestHandler<UpdateNotificationsCommand, Response<Guid>>
    {
        private readonly UserManager<User> _userManager;
        private readonly ICurrentUserService _currentUserService;

        public UpdateNotificationsCommandHandler(UserManager<User> userManager, ICurrentUserService currentUserService)
        {
            _userManager=userManager;
            _currentUserService=currentUserService;
        }

        public async Task<Response<Guid>> Handle(UpdateNotificationsCommand request, CancellationToken cancellationToken)
        {

            //var dbQuery = _userManager.Users
            //    .Where(x => x.Id== _currentUserService.UserId).FirstOrDefault();
            User user = await _userManager.FindByIdAsync(_currentUserService.UserId.ToString());

            if (user != null)
            {
                user.EmailNotification=request.EmailNotificationsEnable;
                user.ToasterNotification=request.ToasterNotificationsEnable;
                await _userManager.UpdateAsync(user);
                return new Response<Guid>("wish granted");
            }

            return new Response<Guid>("Ups that person doesn't exist.");
            


        }
    }
}
