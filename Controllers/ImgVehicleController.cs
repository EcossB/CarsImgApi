using CarsImgApi.Interface;
using CarsImgApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarsImgApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImgVehicleController : ControllerBase
    {
        private readonly IImageVehicle _interfaceImg;

        public ImgVehicleController(IImageVehicle interfaceImg)
        {
            _interfaceImg = interfaceImg;   
        }

        [HttpPost]
        public async Task<ActionResult<string>> saveImgData(ImgVehicleModel imgVehicle)
        {
            _interfaceImg.addImagesVehicle(imgVehicle);
            return Ok("Imagenes Del vehiculo Guardados!");
        }

        [HttpGet]
        public async Task<ActionResult<List<ImgVehicleModel>>> getAllVehiclesImg()
        {
            var vehicleList = _interfaceImg.getAllImagesVehicles();
            return Ok(vehicleList);
        }

        [HttpGet("{num_order}")]
        public async Task<ActionResult<ImgVehicleModel>> getImgVehicleByNumOrder(int num_order)
        {
            var vehicleImg = _interfaceImg.getImageVehicle(num_order);
            if(vehicleImg.Img_lateral_izquierdo.Length > 0)
            {
                return Ok(vehicleImg);
            }
            return BadRequest("No existe ese numero de orden.");
            
        }

    }
}
