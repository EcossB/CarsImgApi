using CarsImgApi.Models.Domain;

namespace CarsImgApi.Repository.Interface
{
    public interface IImageVehicle
    {
        public Task<ImgVehicles> AddImagesVehicle(ImgVehicles vehicle, string user);

        public Task<IEnumerable<ImgVehicles>> GetAllImagesVehicles(string user);

        public Task<ImgVehicles?> GetImageVehicle(int num_order);

        public Task<List<ImgVehicles>> PaginateImages(string user, int pagina, int limiteRegistro);

        public Task<IEnumerable<ImgVehicles>> Get4FirstImages(string user);

        public Task<int> NumberPages(string user);
    }
}
