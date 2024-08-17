using CarsImgApi.Models.Domain;
using CarsImgApi.Models.DTO.ImgVehicleDTOS;

namespace CarsImgApi.Repository.Interface
{
    public interface IImageVehicle
    {
        public Task<ImgVehicles> addImagesVehicle(ImgVehicles vehicle, string user);

        public Task<List<ImgVehicles>> getAllImagesVehicles(string user);

        public Task<ImgVehicles> getImageVehicle(int num_order, string user);

        public Task<List<ImgVehicles>> paginateImages(string user, int pagina, int limiteRegistro);

        public Task<List<ImgVehicles>> get4FirstImages(string user);

        public Task<int> numberPages(string user);

    }
}
