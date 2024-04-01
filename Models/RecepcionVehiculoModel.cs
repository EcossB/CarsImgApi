namespace CarsImgApi.Models
{
    public class RecepcionVehiculoModel
    {
        public string? Nombre { get; set; } = string.Empty;
        public int Marca { get; set; } = 0;
        public int Modelo { get; set; } = 0;
        public string? Placa { get; set; } = string.Empty;
        public int Color { get; set; } = 0;
    }
}
