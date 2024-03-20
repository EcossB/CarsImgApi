using CarsImgApi.Models;

namespace CarsImgApi.Interface
{
    public interface IImageVehicle
    {
        void addImagesVehicle(ImgVehicleModel vehicle, string user);

        List<ImgVehicleModel> getAllImagesVehicles(string user);

        ImgVehicleModel getImageVehicle(int num_order, string user);

    }
}
