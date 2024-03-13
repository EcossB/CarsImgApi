namespace CarsImgApi.Entity
{
    public class EntiddadVehiculo
    {
        public string Chasis { get; set; } = string.Empty;
        public int Marca {  get; set; } = 0;
        public int Modelo {  get; set; } = 0;
        public int Ano {  get; set; } = 0;

        public int Color { get; set; } = 0;
        public string Placa { get; set; } = string.Empty;

        public int Kilometraje { get; set; } = 0;
    }
}
