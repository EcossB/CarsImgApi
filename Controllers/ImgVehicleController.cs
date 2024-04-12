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
    public class ImgVehicleController : ControllerBase
    {
        private readonly IImageVehicle _interfaceImg;

        public ImgVehicleController(IImageVehicle interfaceImg)
        {
            _interfaceImg = interfaceImg;   
        }

        [HttpPost]
        public async Task<ActionResult<string>> saveImgData(ImgVehicleModel imgVehicle, string user)
        {
            if (imgVehicle.Img_frontal.Length > 0 &&
                imgVehicle.Img_lateral_izquierdo.Length > 0 &&
                imgVehicle.Img_trasero.Length > 0 &&
                imgVehicle.Img_lateral_derecho.Length > 0)
            {
                _interfaceImg.addImagesVehicle(imgVehicle, user);
                return Ok(new { mensaje = "Imagenes Del vehiculo Guardados!" });
            } else
                return BadRequest(new { mensaje = "Debes de seleccionar un vehiculo y tirar las 4 fotos." });

        }

        [HttpGet]
        public async Task<ActionResult<List<ImgVehicleModel>>> getAllVehiclesImg(string user)
        {
            var vehicleList = _interfaceImg.getAllImagesVehicles(user);
            return Ok(vehicleList);
        }

        [HttpGet("{num_order}")]
        public async Task<ActionResult<ImgVehicleModel>> getImgVehicleByNumOrder(int num_order, string user)
        {
            var vehicleImg = _interfaceImg.getImageVehicle(num_order, user);
            if(vehicleImg.Img_lateral_izquierdo.Length > 0)
            {
                return Ok(vehicleImg);
            }
            return BadRequest("No existe ese numero de orden.");
            
        }

        [HttpGet("/api/pagination")]
        public async Task<ActionResult<List<ImgVehicleModel>>> imgPagination(string user, int pagina, int limiteRegistro)
        {
            var vehicleImg = _interfaceImg.getNext(user, pagina, limiteRegistro);

            return Ok(vehicleImg);

        }

        [HttpGet("/api/get4first")]
        public async Task<ActionResult<List<ImgVehicleModel>>> getFirst5(string user)
        {
            var vehicleImg = _interfaceImg.getFirst4(user);
            return Ok(vehicleImg);

        }


    }
}
