using CarsImgApi.Models.Domain;

namespace CarsImgApi.Repository.Interface
{
    public interface ICreateImage
    {
        public Task<List<string>> CreateImageAsync(ImgVehicles vehicleImages);

        public Task<ImgVehicles> GetImageAsync(ImgVehicles vehicleImages);

        public Task<string> ReadAsbase64(string path);
    }
}
