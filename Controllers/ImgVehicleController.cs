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

    }
}
