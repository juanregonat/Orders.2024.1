using Microsoft.AspNetCore.Mvc;
using Orders.Backend.UnitOfWork.Interfaces;
using Orders.Shared.Entities;

namespace Orders.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class CitiesControllers : GenericController<City>
    {
        public CitiesControllers(IGenericUnitOfWork<City> unitOfWork) : base(unitOfWork)
        {
        }
    }
}
