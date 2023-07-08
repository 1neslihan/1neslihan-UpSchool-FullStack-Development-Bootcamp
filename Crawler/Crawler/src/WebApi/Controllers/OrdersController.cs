using Application.Features.Orders.Commands.Add;
using Application.Features.Orders.Commands.Delete;
using Application.Features.Orders.Commands.HardDelete;
using Application.Features.Orders.Queries.GetAll;
using Application.Features.Products.Commands.Add;
using Application.Features.Products.Queries.GetAll;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [AllowAnonymous]
    public class OrdersController : ApiControllerBase
    {
        
        [HttpPost("Add")]
        public async Task<IActionResult> AddAsync(OrderAddCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("Pull")]
        public async Task<IActionResult> GetAllAsync(OrderGetAllQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpPut("SoftDelete")]
        public async Task<IActionResult> SoftDeleteAsync(Guid id, OrderSoftDeleteCommand command)
        {

            return Ok(await Mediator.Send(command));

        }

        [HttpDelete("HardDelete")]
        public async Task<IActionResult> HardDeleteAsync(Guid id, OrderHardDeleteCommand command)
        {

            return Ok(await Mediator.Send(command));

        }
    }
}
