using CarsImgApi.Models;
using CarsImgApi.Models.Domain;
using CarsImgApi.Repository.Interface;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace CarsImgApi.services
{
    public class CreateImageService : ICreateImage
    {
        /*Route disk where i save the images.*/
        private readonly string _diskRoute;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public CreateImageService(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
            _diskRoute = _configuration.GetValue<string>("DiskRoute") ?? throw new ArgumentNullException("DiskRoute configuration is missing.");
        }


        private string SanitizeFileName(string? input)
        {
            if (string.IsNullOrEmpty(input)) return "DEFAULT";
            string pattern = @"[^a-zA-Z0-9\-\.]";
            return Regex.Replace(input, pattern, "_");
        }

        public async Task<List<string>> CreateImageAsync(ImgVehicles vehicleImages)
        {

            string companiaSana = SanitizeFileName(vehicleImages.Compania);
            string sucursalSana = SanitizeFileName(vehicleImages.Sucursal);
            string numOrdenSana = SanitizeFileName(vehicleImages.Num_orden.ToString());

            var imagesToProcces = new Dictionary<string, string?>
            {
                {"IMAGEN_DERECHA", vehicleImages.Img_lateral_derecho},
                {"IMAGEN_IZQUIERDA", vehicleImages.Img_lateral_izquierdo},
                {"IMAGEN_FRONTAL", vehicleImages.Img_frontal},
                {"IMAGEN_TRASERA", vehicleImages.Img_trasero},
                {"IMAGEN_ANEXO1", vehicleImages.Img_anexo1},
                {"IMAGEN_ANEXO2", vehicleImages.Img_anexo2},
                {"IMAGEN_ANEXO3", vehicleImages.Img_anexo3}
            };


            /*this list will contain all the path of the images. these paths are the path that we are going to insert in the database*/
            var savedPath = new List<string>();

            foreach (var img in imagesToProcces.Where(x => !string.IsNullOrEmpty(x.Value)))
            {

                // Limpieza segura del prefijo Base64 (data:image/jpeg;base64,...)
                var base64Data = img.Value.Split(',')[1]; // Split the string to get the base64 part
                var bytesImage = Convert.FromBase64String(base64Data);
                
                var rutaImagenes = Path.Combine(_env.WebRootPath ?? _env.ContentRootPath, _diskRoute);
                
                if (!Directory.Exists(rutaImagenes))
                {
                    Directory.CreateDirectory(rutaImagenes);
                }

                string imageName = $"{companiaSana}_{sucursalSana}_{numOrdenSana}_{img.Key}.jpg";
                string fullPath = Path.Combine(rutaImagenes, imageName);

                var rutaNormalizada = Path.GetFullPath(fullPath);
                if (!rutaNormalizada.StartsWith(Path.GetFullPath(_diskRoute), StringComparison.OrdinalIgnoreCase))
                {
                    // Esto es un intento de ataque. Lo bloqueamos y podemos loguearlo como FATAL.
                    throw new UnauthorizedAccessException("Intento de Path Traversal detectado.");
                }


                //Se esta utilzando el disco E para que guarde las imagenes.
                await File.WriteAllBytesAsync(fullPath,bytesImage);

                savedPath.Add(fullPath); //adding the path to the list.

            }
            
            return savedPath; //returning the list. 
        }

        public async Task<ImgVehicles> GetImageAsync(ImgVehicles vehicleImages)
        {
           

            var vehicle = new ImgVehicles
            {
                Compania = vehicleImages.Compania,
                Sucursal = vehicleImages.Sucursal,
                Num_orden = vehicleImages.Num_orden,
                Img_lateral_derecho = await ReadAsbase64(vehicleImages.Img_lateral_derecho),
                Img_lateral_izquierdo = await ReadAsbase64(vehicleImages.Img_lateral_izquierdo),
                Img_frontal = await ReadAsbase64(vehicleImages.Img_frontal),
                Img_trasero = await ReadAsbase64(vehicleImages.Img_trasero),
                Img_anexo1 = await ReadAsbase64(vehicleImages.Img_anexo1),
                Img_anexo2  = await ReadAsbase64(vehicleImages.Img_anexo2),
                Img_anexo3 = await ReadAsbase64(vehicleImages.Img_anexo3)
            };
            
            return vehicle;
        }

        public async Task<string> ReadAsbase64(string path)
        {

            
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                throw new FileNotFoundException($"The file at path {path} was not found.");
            }

            byte[] bytes = await File.ReadAllBytesAsync(path);
            return Convert.ToBase64String(bytes);

        }
    }
}
