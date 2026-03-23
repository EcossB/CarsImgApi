using CarsImgApi.Models.Domain;
using CarsImgApi.Models.DTO.ImgVehicleDTOS;

namespace CarsImgApi.Repository.Interface
{
    public interface ICreateImage
    {
        public Task<List<string>> CreateImageAsync(ImgVehicles vehicleImages);
        
        public Task<string> CreateSingleImageAsync(IFormFile file, ImgSingleVehicleRequest img, string lado);
        
        public Task<ImgVehicles> GetImageAsync(ImgVehicles? vehicleImages);

        public Task<string> ReadAsbase64(string path);
    }
}
