using CarsImgApi.Models;

namespace CarsImgApi.Interface
{
    public interface IDatosVehiculos
    {
        List<ModeloVehiculo> getAllVehiclesData();

        ModeloVehiculo getVehicleByChasis(string chasis);

        List<ChasisModel> getAllChasis();
    }
}
