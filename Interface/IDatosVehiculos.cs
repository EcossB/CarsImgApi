using CarsImgApi.Models;
using CarsImgApi.services;

namespace CarsImgApi.Interface
{
    public interface IDatosVehiculos 
    {
       
        List<ModeloVehiculoRecepcion> getAllVehiclesData(string user);

        ModeloVehiculoRecepcion getVehicleByPlaca(string placa, string user);

        List<ChasisModel> getAllChasis(string user);

        List<ChasisModel> getChasis(string chasis, string user);
    }
}
