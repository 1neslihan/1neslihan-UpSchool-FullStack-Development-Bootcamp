using Application.Features.Orders.Commands.Add;
using Application.Features.Orders.Commands.Delete;
using Application.Features.Orders.Commands.HardDelete;
using Application.Features.Orders.Commands.UndoDelete;
using Application.Features.Orders.Queries.GetAll;
using Application.Features.Orders.Queries.GetByDate;
using Application.Features.Products.Commands.Add;
using Application.Features.Products.Queries.GetAll;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [Authorize]
    public class OrdersController : ApiControllerBase
    {
        
        [HttpPost("Add")]
        public async Task<IActionResult> AddAsync(OrderAddCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("Pull")]
        public async Task<IActionResult> GetByIdAsync(OrderGetByIdQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpPost("GetByDate")]
        public async Task<IActionResult> GetByDateAsync(OrderGetByDateQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpPut("SoftDelete")]
        public async Task<IActionResult> SoftDeleteAsync(OrderSoftDeleteCommand command)
        {

            return Ok(await Mediator.Send(command));

        }

        [HttpPut("UndoDelete")]
        public async Task<IActionResult> UndoDeleteAsync(OrderUndoDeleteCommand command)
        {

            return Ok(await Mediator.Send(command));

        }

        [HttpDelete("HardDelete")]
        public async Task<IActionResult> HardDeleteAsync(OrderHardDeleteCommand command)
        {

            return Ok(await Mediator.Send(command));

        }
    }
}
