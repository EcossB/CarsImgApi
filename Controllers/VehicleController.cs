using CarsImgApi.Interface;
using CarsImgApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarsImgApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {

        private readonly IDatosVehiculos _interfaceVehicles;


        public VehicleController(IDatosVehiculos interfaceVehicles)
        {
            _interfaceVehicles = interfaceVehicles;
        }

        [HttpGet]
        public async Task<ActionResult<List<ModeloVehiculo>>> GetAllVehiclesData()
        {
            var vehicles = _interfaceVehicles.getAllVehiclesData();
            return Ok(vehicles);
        }

        [HttpGet("{chasis}")]

        public async Task<ActionResult<ModeloVehiculo>> getVehicleByChasis(string chasis)
        {
            var vehicle = _interfaceVehicles.getVehicleByChasis(chasis);
            if(vehicle.Chasis != "")
            {
                return Ok(vehicle);
            }
            return BadRequest("No existe vehiculo conel chasis introducido.");
        }

        [HttpGet("allChasis")]
        public async Task<ActionResult<List<ChasisModel>>> getAllChasis()
        {
            var chasis = _interfaceVehicles.getAllChasis();
            return Ok(chasis);
        }

        [HttpGet("single/{chasisString}")]
        public async Task<ActionResult<List<ChasisModel>>> getChasis(string chasisString)
        {
            var chasis = _interfaceVehicles.getChasis(chasisString);

            if(chasis.Count !> 0)
            {
                return Ok(chasis);
            }

            return Ok(chasis);
        }

    }
}
