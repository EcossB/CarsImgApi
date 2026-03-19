using System.ComponentModel.DataAnnotations;

namespace CarsImgApi.Models.DTO.ImgVehicleDTOS
{
    public class ImgVehicleRequestDTO
    {
        [Required]
        public string Compania { get; set; } = string.Empty;

        [Required]
        public string Sucursal { get; set; } = string.Empty;

        [Required]
        public int Num_orden { get; set; } = 0;

        public string? Img_lateral_derecho { get; set; } = string.Empty;
        public string? Img_lateral_izquierdo { get; set; } = string.Empty;
        public string? Img_frontal { get; set; } = string.Empty;
        public string? Img_trasero { get; set; } = string.Empty;
        public string? Img_anexo1 { get; set; } = string.Empty;
        public string? Img_anexo2 { get; set; } = string.Empty;
        public string? Img_anexo3 { get; set; } = string.Empty;

        [Required]
        public string Usuario { get; set; } = string.Empty;

        public int Kilometros { get; set; } = 0;
        public string? Placa { get; set; } = string.Empty;
    }
}
