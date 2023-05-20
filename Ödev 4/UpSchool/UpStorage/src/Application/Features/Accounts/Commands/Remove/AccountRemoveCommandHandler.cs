using Application.Common.Interfaces;
using Application.Common.Localizations;
using Application.Resources.Common.Localizations;
using Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Accounts.Commands.Remove
{
    public class AccountRemoveCommandHandler : IRequestHandler<AccountRemoveCommand, Response<Guid>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IAccountHubService _accountHubService;
        private readonly IStringLocalizer<CommonLocalizations> _localizer;
        public AccountRemoveCommandHandler(IApplicationDbContext applicationDbContext, IAccountHubService accountHubService, IStringLocalizer<CommonLocalizations> localizer)
        {
            _applicationDbContext = applicationDbContext;
            _accountHubService = accountHubService;
            _localizer = localizer;
        }

        public async Task<Response<Guid>> Handle(AccountRemoveCommand request, CancellationToken cancellationToken)
        {
            var account = await _applicationDbContext.Accounts
                .FirstOrDefaultAsync(x=>x.Id == request.Id, cancellationToken);
            _applicationDbContext.Accounts.Remove(account);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            await _accountHubService.RemovedAsync(request.Id, cancellationToken);

            return new Response<Guid>(_localizer[CommonLocalizationKeys.HnadlerMessages.Delete], request.Id);


        }
    }
}
