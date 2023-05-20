using Application.Features.Cities.Commands.Add;
using Application.Features.Cities.Queries.GetAll;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebApi.Controllers.Filters;

namespace WebApi.Controllers
{
    //[ValidationFilter]
    [Authorize]
    public class CitiesController : ApiControllerBase
    {
        [HttpPost("Add")]
        public async Task<IActionResult> AddAsync(CityAddCommand command)
        {
            
            return Ok(await Mediator.Send(command));
        }

        
        [HttpPost("GetAll")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync(CityGetAllQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            return Ok(await Mediator.Send(new CityGetAllQuery(id,null)));
        }

    }
}
