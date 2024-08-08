using CarsImgApi.Models.DTO.ImgVehicleDTOS;

namespace CarsImgApi.Repository.Interface
{
    public interface IImageVehicle
    {
        Task<string> addImagesVehicle(ImgVehicleRequestDTO vehicle, string user);

        Task<List<ImgVehicleRequestDTO>> getAllImagesVehicles(string user);

        Task<ImgVehicleRequestDTO> getImageVehicle(int num_order, string user);

        public Task<List<ImgVehicleRequestDTO>> getNext(string user, int pagina, int limiteRegistro);

        public Task<List<ImgVehicleRequestDTO>> getFirst4(string user);

    }
}
