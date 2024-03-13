namespace CarsImgApi.Models
{
    public class ImgVehicleModel
    {
        public string? Compania { get; set; } = string.Empty;
        public string? Sucursal { get; set; } = string.Empty;
        public int Orden_Numero { get; set; } = 0;
        public string? Img_lateral_derecho { get; set; } = string.Empty;
        public string? Img_lateral_izquierdo { get; set; } = string.Empty;
        public string? Img_frontal { get; set; } = string.Empty;
        public string? Img_trasero { get; set; } = string.Empty;
    }
}
