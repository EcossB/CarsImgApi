using CarsImgApi.Models.DTO.VehicleDTOS;
using CarsImgApi.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
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
            var vehicles = await _interfaceVehicles.GetAllVehiclesData(user);

            var VehicleList = new List<VehicleResponseDtos>();

            foreach (var vehicle in vehicles) 
            {
                VehicleList.Add(new VehicleResponseDtos()
                {
                    Compania = vehicle.Compania,
                    Num_orden = vehicle.Num_orden,
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
            var vehicle = await _interfaceVehicles.GetVehicleByPlaca(requestChasis, requestUser);

                
            if (vehicle is not null){

                var vehicleResponse = new VehicleResponseDtos()
                {
                    Compania = vehicle.Compania,
                    Num_orden = vehicle.Num_orden,    
                    Sucursal = vehicle.Sucursal,
                    Nombre = vehicle.Nombre_cliente,
                    Marca = vehicle.Marca,
                    Modelo = vehicle.Modelo,
                    Placa = vehicle.Placa,
                    Fecha_orden = vehicle.Fecha_orden
                };

                return Ok(vehicleResponse);
            }
            
            return BadRequest(new {Message = "El Vehiculo No Existe Con Esa Placa."});
        }

    }
}
