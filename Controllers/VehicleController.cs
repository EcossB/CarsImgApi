using CarsImgApi.Models.DTO;
using CarsImgApi.Models.DTO.ImgVehicleDTOS;
using CarsImgApi.Models.DTO.VehicleDTOS;
using CarsImgApi.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarsImgApi.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]

    [Authorize]
    public class VehicleController : ControllerBase
    {

        private readonly IDataVehicle _interfaceVehicles;


        public VehicleController(IDataVehicle interfaceVehicles)
        {
            _interfaceVehicles = interfaceVehicles;
        }

        [HttpGet]
        [Route("orders/{user}")]
        public async Task<IActionResult> GetAllVehiclesData([FromRoute] string user)
        {
            var vehicles = await _interfaceVehicles.getAllVehiclesData(user);

            var VehicleList = new List<VehicleResponseDtos>();

            foreach (var vehicle in vehicles) 
            {
                VehicleList.Add(new VehicleResponseDtos()
                {
                    Compania = vehicle.Compania,
                    Orden_Numero = vehicle.Orden_Numero,
                    Sucursal = vehicle.Sucursal,
                    Nombre = vehicle.Nombre_cliente,
                    Marca = vehicle.Marca,
                    Modelo = vehicle.Modelo,
                    Placa = vehicle.Placa,
                    Fecha_orden = vehicle.Fecha_orden
                });
            }

            return Ok(VehicleList);
        }

        [HttpGet]
        [Route("chasis/{requestChasis}/user{requestUser}")]
        public async Task<IActionResult> getVehicleByPlaca([FromRoute] string requestChasis, [FromRoute]string requestUser)
        {
            var vehicle = await _interfaceVehicles.getVehicleByPlaca(requestChasis, requestUser);

                
            if (vehicle is not null){

                var vehicleResponse = new VehicleResponseDtos()
                {
                    Compania = vehicle.Compania,
                    Orden_Numero = vehicle.Orden_Numero,    
                    Sucursal = vehicle.Sucursal,
                    Nombre = vehicle.Nombre_cliente,
                    Marca = vehicle.Marca,
                    Modelo = vehicle.Modelo,
                    Placa = vehicle.Placa,
                    Fecha_orden = vehicle.Fecha_orden
                };

                return Ok(vehicleResponse);
            }
            
            return BadRequest("The vehicle doesn't exists with the required chasis.");
        }

        /*
        [HttpGet]
        [Route(("allChasis/{user}"))]
        public async Task<IActionResult> getAllChasis([FromRoute] UserRequestDto user)
        {
            var chasis = await _interfaceVehicles.getAllChasis(user.User);

            if (chasis is not null)
            {
                return Ok(chasis);
            }

            return BadRequest("You Don't Have the permissions to retrieve Data.");
        }

        [HttpGet("SimilarsChasis")]
        public async Task<IActionResult> getChasis(GetVehicleByChasisRequestDTO request)
        {
            var chasis = await _interfaceVehicles.getChasis(request.Chasis, request.User);

            if(chasis is not null)
            {
                return Ok(chasis);
            }

            return BadRequest($"The Chasis {request.Chasis} doesn't exists");
        }*/

    }
}
