using CarsImgApi.Models.Domain;

namespace CarsImgApi.Repository.Interface
{
    public interface ICreateImage
    {
        public Task<List<string>> CreateImageAsync(ImgVehicles vehicleImages);
    }
}
