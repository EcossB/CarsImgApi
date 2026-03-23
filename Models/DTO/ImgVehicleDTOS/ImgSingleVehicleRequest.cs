namespace CarsImgApi.Models.DTO.ImgVehicleDTOS;

public class ImgSingleVehicleRequest
{
    public string? Compania { get; set; } = string.Empty;
    public string? Sucursal { get; set; } = string.Empty;
    public int Num_orden { get; set; } = 0;
    
    public int Kilometros {get; set; } = 0;
    public string? Placa {get; set; } = string.Empty;
    public string? Usuario {get; set; } = string.Empty;
    

}