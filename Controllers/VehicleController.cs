using CarsImgApi.Entity;
using CarsImgApi.Interface;
using CarsImgApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarsImgApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    [Authorize]
    public class VehicleController : ControllerBase
    {

        private readonly IDatosVehiculos _interfaceVehicles;


        public VehicleController(IDatosVehiculos interfaceVehicles)
        {
            _interfaceVehicles = interfaceVehicles;
        }

        [HttpGet]
        public async Task<ActionResult<List<ModeloVehiculoRecepcion>>> GetAllVehiclesData(string user)
        {
            var vehicles = _interfaceVehicles.getAllVehiclesData(user);
            return Ok(vehicles);
        }

        [HttpGet("{chasis}")]

        public async Task<ActionResult<ModeloVehiculoRecepcion>> getVehicleByChasis(string placa, string user)
        {
            var vehicle = _interfaceVehicles.getVehicleByPlaca(placa,  user);
            if(vehicle.Placa != "")
            {
                return Ok(vehicle);
            }
            return BadRequest("No existe vehiculo con la placa introducida.");
        }

        [HttpGet("allChasis")]
        public async Task<ActionResult<List<ChasisModel>>> getAllChasis(string user)
        {
            var chasis = _interfaceVehicles.getAllChasis( user);
            return Ok(chasis);
        }

        [HttpGet("single/{chasisString}")]
        public async Task<ActionResult<List<ChasisModel>>> getChasis(string chasisString, string user)
        {
            var chasis = _interfaceVehicles.getChasis(chasisString, user);

            if(chasis.Count !> 0)
            {
                return Ok(chasis);
            }

            return Ok(chasis);
        }

    }
}
