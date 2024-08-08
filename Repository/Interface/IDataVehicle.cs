using CarsImgApi.Models.Domain;
using CarsImgApi.Models.DTO.ImgVehicleDTOS;
using CarsImgApi.Models.DTO.VehicleDTOS;
using CarsImgApi.services;

namespace CarsImgApi.Repository.Interface
{
    public interface IDataVehicle
    {

        public Task<IEnumerable<Vehicle>> getAllVehiclesData(string user);

        public Task<Vehicle> getVehicleByPlaca(string placa, string user);

        /*
         * these methods wont be used it. 
         * public Task<IEnumerable<ChasisResponseDto>> getAllChasis(string user);

        public Task<IEnumerable<ChasisResponseDto>> getChasis(string chasis, string user);*/
    }
}
