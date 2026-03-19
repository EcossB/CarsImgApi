using CarsImgApi.Models.Domain;

namespace CarsImgApi.Repository.Interface
{
    public interface IDataVehicle
    {
        public Task<IEnumerable<Vehicle>> GetAllVehiclesData(string user);

        public Task<Vehicle?> GetVehicleByPlaca(string placa, string user);
    }
}
