using Application.Features.Products.Commands.Add;
using Application.Features.Products.Queries.GetAll;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [AllowAnonymous]
    public class ProductsController : ApiControllerBase
    {
        [HttpPost("Add")]
        public async Task<IActionResult> AddAsync(ProductAddCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("Pull")]
        public async Task<IActionResult> GetByOrderIdAsync(ProductGetByOrderIdQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        //[HttpGet("Pull/{id}")]
        //public async Task<IActionResult> GetByIdAsync(Guid id)
        //{
        //    return Ok(await Mediator.Send(new ProductGetAllQuery(id, null)));
        //}
    }
}
