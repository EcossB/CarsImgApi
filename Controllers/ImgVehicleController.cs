using CarsImgApi.Models.Domain;
using CarsImgApi.Models.DTO.ImgVehicleDTOS;
using CarsImgApi.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarsImgApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    [Authorize]
    public class ImgVehicleController : ControllerBase
    {
        private readonly IImageVehicle _imageRepository;

        public ImgVehicleController(IImageVehicle interfaceImg)
        {
            _imageRepository = interfaceImg;   
        }

        [HttpPost]
        [Route("addImage")]
        public async Task<IActionResult> saveImgData(ImgVehicleRequestDTO imgVehicle)
        {
            //dto to domain model 

            var vehicle = new ImgVehicles
            {
                Compania = imgVehicle.Compania,
                Sucursal = imgVehicle.Sucursal,
                Orden_Numero = imgVehicle.Orden_Numero,
                Img_lateral_derecho = imgVehicle.Img_lateral_derecho,
                Img_lateral_izquierdo = imgVehicle.Img_lateral_izquierdo,
                Img_frontal = imgVehicle.Img_frontal,
                Img_trasero = imgVehicle.Img_trasero,
                Img_anexo1 = imgVehicle.Img_anexo1,
                Img_anexo2 = imgVehicle.Img_anexo2,
                Img_anexo3 = imgVehicle.Img_anexo3
            };

            var newVehicle = await _imageRepository.addImagesVehicle(vehicle, imgVehicle.UserToken);

            if (newVehicle is not null) 
            {
                return Ok("New Vehicle images Save!");
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("GetAll/{user}")]
        public async Task<IActionResult> getAllVehiclesImg([FromRoute] string user)
        {
            var vehicleList = await _imageRepository.getAllImagesVehicles(user);

            var response = new List<ImgVehicleResponseDTO>();

            foreach (var vehicle in vehicleList) 
            {
                response.Add(new ImgVehicleResponseDTO
                {
                    Compania = vehicle.Compania,
                    Sucursal = vehicle.Sucursal,
                    Orden_Numero = vehicle.Orden_Numero,
                    Img_lateral_derecho = vehicle.Img_lateral_derecho,
                    Img_lateral_izquierdo = vehicle.Img_lateral_izquierdo,
                    Img_frontal = vehicle.Img_frontal,
                    Img_trasero = vehicle.Img_trasero,
                    Img_anexo1 = vehicle.Img_anexo1,
                    Img_anexo2 = vehicle.Img_anexo2,
                    Img_anexo3 = vehicle.Img_anexo3
                });
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("getByNumOrder/{num_order}/user{user}")]
        public async Task<IActionResult> getImgVehicleByNumOrder([FromRoute]int num_order, [FromRoute] string user)
        {
            var vehicle = await _imageRepository.getImageVehicle(num_order, user);

            if(vehicle is not null)
            {
                var reponse = new ImgVehicleResponseDTO
                {
                    Compania = vehicle.Compania,
                    Sucursal = vehicle.Sucursal,
                    Orden_Numero = vehicle.Orden_Numero,
                    Img_lateral_derecho = vehicle.Img_lateral_derecho,
                    Img_lateral_izquierdo = vehicle.Img_lateral_izquierdo,
                    Img_frontal = vehicle.Img_frontal,
                    Img_trasero = vehicle.Img_trasero,
                    Img_anexo1 = vehicle.Img_anexo1,
                    Img_anexo2 = vehicle.Img_anexo2,
                    Img_anexo3 = vehicle.Img_anexo3
                };

                return Ok(reponse);
            }
            return BadRequest("No existe ese numero de orden.");
            
        }

        [HttpGet]
        [Route("pagination/user{user}/page{page}/limit{limit}")]
        public async Task<ActionResult<List<ImgVehicleRequestDTO>>> imgPagination(string user, int page, int limit)
        {
            var vehicleList = await _imageRepository.paginateImages(user, page, limit);

            var response = new List<ImgVehicleResponseDTO>();

            foreach (var vehicle in vehicleList) 
            {
                response.Add(new ImgVehicleResponseDTO
                {
                    Compania = vehicle.Compania,
                    Sucursal = vehicle.Sucursal,
                    Orden_Numero = vehicle.Orden_Numero,
                    Img_lateral_derecho = vehicle.Img_lateral_derecho,
                    Img_lateral_izquierdo = vehicle.Img_lateral_izquierdo,
                    Img_frontal = vehicle.Img_frontal,
                    Img_trasero = vehicle.Img_trasero,
                    Img_anexo1 = vehicle.Img_anexo1,
                    Img_anexo2 = vehicle.Img_anexo2,
                    Img_anexo3 = vehicle.Img_anexo3
                });
            }

            return Ok(response);

        }

        [HttpGet]
        [Route("get4first/user{user}")]
        public async Task<IActionResult> getFirst4Image(string user)
        {
            var vehicleList = await _imageRepository.get4FirstImages(user);

            var response = new List<ImgVehicleResponseDTO>();

            foreach (var vehicle in vehicleList)
            {
                response.Add(new ImgVehicleResponseDTO
                {
                    Compania = vehicle.Compania,
                    Sucursal = vehicle.Sucursal,
                    Orden_Numero = vehicle.Orden_Numero,
                    Img_lateral_derecho = vehicle.Img_lateral_derecho,
                    Img_lateral_izquierdo = vehicle.Img_lateral_izquierdo,
                    Img_frontal = vehicle.Img_frontal,
                    Img_trasero = vehicle.Img_trasero,
                    Img_anexo1 = vehicle.Img_anexo1,
                    Img_anexo2 = vehicle.Img_anexo2,
                    Img_anexo3 = vehicle.Img_anexo3
                });
            }

            return Ok(response);

        }


    }
}
