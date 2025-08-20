using Microsoft.AspNetCore.Mvc;
using Orders.Backend.UnitOfWork.Interfaces;
using Orders.Shared.Entities;

namespace Orders.Backend.Controllers
{
 
    [ApiController]
    [Route("api/[controller]")]
    public class StateController : GenericController<State>
    {
        private readonly IStatesUnitOfWork _statesUnitOfWork;

        public StateController(IGenericUnitOfWork<State> unitOfWork, IStatesUnitOfWork statesUnitOfWork) : base(unitOfWork)
        {
            _statesUnitOfWork = statesUnitOfWork;
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync()
        {
            var response = await _statesUnitOfWork.GetAsync();
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var response = await _statesUnitOfWork.GetAsync(id);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Messagge);
        }
    }
}