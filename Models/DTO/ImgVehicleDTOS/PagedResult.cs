namespace CarsImgApi.Models.DTO.ImgVehicleDTOS
{
    public class PagedResult<T>
    {
        public IEnumerable<T>? Data { get; set; } // Los 10 vehículos
        public int TotalRecords { get; set; }    // Ej: 1540 (Total en la BD)
        public int PageNumber { get; set; }      // Ej: 1
        public int PageSize { get; set; }        // Ej: 10
        public int TotalPages => (int) Math.Ceiling((double)TotalRecords / PageSize);
        
    }
}
