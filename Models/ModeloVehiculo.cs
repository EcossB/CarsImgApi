namespace CarsImgApi.Models
{
    public class ModeloVehiculoRecepcion
    {
        public string? Compania { get; set; } = string.Empty;
        public string? Sucursal {  get; set; } = string.Empty;
        public int Orden_Numero { get; set; } = 0;
        public DateTime Fecha_orden { get; set; }
        public string? Nombre_cliente { get; set; } = string.Empty;
        public string? Marca { get; set; } = string.Empty;
        public string? Modelo { get; set; } = string.Empty;
        public string? Placa { get; set; } = string.Empty;
        
    }
}
