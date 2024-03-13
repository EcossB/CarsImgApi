using CarsImgApi.Models;

namespace CarsImgApi.Interface
{
    public interface IImageVehicle
    {
        void addImagesVehicle(ImgVehicleModel vehicle);

        List<ImgVehicleModel> getAllImagesVehicles();

        ImgVehicleModel getImageVehicle(int num_order);
    }
}
