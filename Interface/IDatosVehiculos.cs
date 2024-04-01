using CarsImgApi.Models;
using CarsImgApi.services;

namespace CarsImgApi.Interface
{
    public interface IDatosVehiculos 
    {
       
        List<RecepcionVehiculoModel> getAllVehiclesData(string user);

        ModeloVehiculo getVehicleByChasis(string chasis, string user);

        List<ChasisModel> getAllChasis(string user);

        List<ChasisModel> getChasis(string chasis, string user);
    }
}
