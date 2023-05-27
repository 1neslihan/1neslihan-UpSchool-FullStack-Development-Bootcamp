using Application.Features.Orders.Commands.Add;
using Application.Features.Orders.Queries.GetAll;
using Application.Features.Products.Commands.Add;
using Application.Features.Products.Queries.GetAll;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    
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
    }
}
