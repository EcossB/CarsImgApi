namespace CarsImgApi.Models.DTO.ImgVehicleDTOS
{
    public class ImgVehiclePaginationDTO
    {
        public List<ImgVehicleResponseDTO>? imgCars { get; set; }
        public int pages { get; set; }
    }
}
