﻿using UpSchool.WebApi.AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using UpSchool.Domain.Dtos;
using UpSchool.Domain.Entities;
using UpSchool.Domain.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using UpSchool.Persistence.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using UpSchool.WebApi.Hubs;

namespace UpSchool.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UpStorageDbContext _dbContext;
        private readonly IHubContext<AccountsHub> _accountsHubContext;

        public AccountsController(IMapper mapper, UpStorageDbContext dbContext, IHubContext<AccountsHub> accountsHubContext)
        {
            _mapper=mapper;
            _dbContext=dbContext;
            _accountsHubContext=accountsHubContext;
        }


        [HttpGet]
        public IActionResult GetAll(bool isAscending ,string? searchKeyword)
        {
            IQueryable<Account> accountsQuery = _dbContext.Accounts.AsQueryable();

            if (!string.IsNullOrEmpty(searchKeyword)) 
                accountsQuery=accountsQuery.Where(x =>
            x.Title.Contains(searchKeyword) || x.UserName.Contains(searchKeyword));

            accountsQuery= isAscending 
                ? accountsQuery.OrderBy(x=>x.Title)
                : accountsQuery.OrderByDescending(x=>x.Title);

            var accounts = accountsQuery.ToList();

            var accountDtos = accounts.Select(account=>AccountDto.MapFromAccount(account)).ToList();
            
            return Ok(accountDtos);
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetById(Guid id)
        {
            /*var account = _accounts.FirstOrDefault(x=>x.Id==id)*/;
            var account= _dbContext.Accounts.FirstOrDefault(x=>x.Id==id);
           
            if (account is null) return NotFound("The selected account was not found.");
            return Ok(AccountDto.MapFromAccount(account));
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(AccountAddDto accountAddDto, CancellationToken cancellationToken)
        {
            var account = new Account()
            {
                Id=Guid.NewGuid(),
                Title=accountAddDto.Title,
                UserName=accountAddDto.UserName,
                Password=accountAddDto.Password,
                IsFavorite=accountAddDto.IsFavorite,
                CreatedOn=DateTimeOffset.Now,
                Url=accountAddDto.Url,

            };

            await _dbContext.Accounts.AddAsync(account, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            var accountDto = AccountDto.MapFromAccount(account);
            await _accountsHubContext.Clients.AllExcept(accountAddDto.ConnectionId).SendAsync(SignalRMethodKeys.Accounts.Added, accountDto,cancellationToken);
            
            return Ok(accountDto);
        }

        [HttpPut]
        public IActionResult Edit(AccountEditDto accountEditDto)
        {
            //var accountIndex = _accounts.FindIndex(x => x.Id==accountEditDto.Id);

            var account=_dbContext.Accounts.FirstOrDefault(x => x.Id==accountEditDto.Id);

            if (account is null) return NotFound("The selected account was not found.");
            
            var updatedAccount = _mapper.Map<AccountEditDto,Account>(accountEditDto, account);

            _dbContext.Accounts.Update(updatedAccount);
            _dbContext.SaveChanges();

            return Ok(_mapper.Map<AccountDto>(updatedAccount));
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id) {
            var account = _dbContext.Accounts.FirstOrDefault(x => x.Id==id);

            if (account is null) return NotFound("The selected account was not found.");
            _dbContext.Accounts.Remove(account);
            _dbContext.SaveChanges();
            return NoContent();
        }




    }   
}
