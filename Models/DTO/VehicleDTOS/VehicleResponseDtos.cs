namespace CarsImgApi.Models.DTO.VehicleDTOS
{
    public class VehicleResponseDtos
    {

        public string? Compania { get; set; } = string.Empty;
        public string? Sucursal { get; set; } = string.Empty;
        public int Orden_Numero { get; set; } = 0;
        public string? Nombre { get; set; } = string.Empty;
        public string? Marca { get; set; }
        public DateTime Fecha_orden { get; set; }
        public string? Modelo { get; set; } 
        public string? Placa { get; set; } = string.Empty;
    }
}
