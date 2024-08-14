namespace CarsImgApi.Models.Domain
{
    public class ImgVehicles
    {
        public string? Compania { get; set; } = string.Empty;
        public string? Sucursal { get; set; } = string.Empty;
        public int Orden_Numero { get; set; } = 0;
        public string? Img_lateral_derecho { get; set; } = string.Empty;
        public string? Img_lateral_izquierdo { get; set; } = string.Empty;
        public string? Img_frontal { get; set; } = string.Empty;
        public string? Img_trasero { get; set; } = string.Empty;
        public string? Img_anexo1 { get; set; } = string.Empty;
        public string? Img_anexo2 { get; set; } = string.Empty;
        public string? Img_anexo3 { get; set; } = string.Empty;

    }
}
