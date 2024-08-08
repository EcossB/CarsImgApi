namespace CarsImgApi.Models.DTO.VehicleDTOS
{
    public class VehicleResponseDtos
    {
        /*
         this DTO wont have all the vehicles properties. because i want to show just the data that the frontend will use.
         */
        public string? Nombre { get; set; } = string.Empty;
        public string? Marca { get; set; }
        public DateTime Fecha_orden { get; set; }
        public string? Modelo { get; set; } 
        public string? Placa { get; set; } = string.Empty;
    }
}
