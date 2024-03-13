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
            return Ok(vehicle);
        }

        [HttpGet("/chasis")]
        public async Task<ActionResult<List<ChasisModel>>> getAllChasis()
        {
            var chasis = _interfaceVehicles.getAllChasis();
            return Ok(chasis);
        }

    }
}
