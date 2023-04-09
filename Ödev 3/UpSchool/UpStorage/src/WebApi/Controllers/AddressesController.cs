using Application.Features.Addresses.Commands.Add;
using Application.Features.Addresses.Commands.Delete;
using Application.Features.Addresses.Commands.SoftDelete;
using Application.Features.Addresses.Commands.Update;
using Application.Features.Addresses.Queries.GetAll;
using Application.Features.Cities.Commands.Add;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class AddressesController : ApiControllerBase
    {
        [HttpPost("Add")]
        public async Task<IActionResult> AddAsync(AddressAddCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAllAsync(AddressGetAllQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            return Ok(await Mediator.Send(new AddressGetAllQuery(id, null)));
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id,AddressUpdateCommand command)
        {
            
            return Ok(await Mediator.Send(command));
            
        }
        [HttpPut("SoftDelete/{id}")]
        public async Task<IActionResult> SoftDeleteAsync(Guid id,AddressSoftDeleteCommand command)
        {

            return Ok(await Mediator.Send(command));

        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id, AddressDeleteCommand command)
        {
            
             return Ok(await Mediator.Send(command));
            
        }
    }
}
