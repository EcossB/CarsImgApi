namespace CarsImgApi.Models
{
    public class ModeloVehiculo
    {
        public string? Compania { get; set; } = string.Empty;
        public string? Sucursal {  get; set; } = string.Empty;
        public int Orden_Numero { get; set; } = 0;
        public string? Chasis { get; set; } = string.Empty;
        public int Marca { get; set; } = 0;
        public int Modelo { get; set; } = 0;
        public int Ano { get; set; } = 0;
        public string? Placa { get; set; } = string.Empty;
        public int Color { get; set; } = 0;
        
    }
}
