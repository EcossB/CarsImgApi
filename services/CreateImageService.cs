using CarsImgApi.Models.Domain;
using CarsImgApi.Repository.Interface;

namespace CarsImgApi.services
{
    public class CreateImageService : ICreateImage
    {
        private readonly string _diskRoute;
        private readonly IWebHostEnvironment _env;

        public CreateImageService(IConfiguration configuration, IWebHostEnvironment env)
        {
            _env = env;
            _diskRoute = configuration.GetValue<string>("DiskRoute")
                ?? throw new InvalidOperationException("DiskRoute configuration is missing.");
        }

        public async Task<ImgVehicles> CreateImageAsync(ImgVehicles vehicleImages)
        {
            var imagesToProcess = new Dictionary<string, string?>
            {
                { "IMG_LATERAL_DERECHO",  vehicleImages.Img_lateral_derecho },
                { "IMG_LATERAL_IZQUIERDO", vehicleImages.Img_lateral_izquierdo },
                { "IMG_FRONTAL",          vehicleImages.Img_frontal },
                { "IMG_TRASERO",          vehicleImages.Img_trasero },
                { "IMG_ANEXO1",           vehicleImages.Img_anexo1 },
                { "IMG_ANEXO2",           vehicleImages.Img_anexo2 },
                { "IMG_ANEXO3",           vehicleImages.Img_anexo3 }
            };

            var rutaImagenes = Path.Combine(_env.WebRootPath ?? _env.ContentRootPath, _diskRoute);

            if (!Directory.Exists(rutaImagenes))
            {
                Directory.CreateDirectory(rutaImagenes);
            }

            foreach (var img in imagesToProcess.Where(x => !string.IsNullOrEmpty(x.Value)))
            {
                var commaIndex = img.Value!.IndexOf(',');
                if (commaIndex < 0)
                    throw new ArgumentException($"Invalid Base64 image format for field '{img.Key}'. Expected 'data:<type>;base64,<data>'.");

                var base64Data = img.Value[(commaIndex + 1)..];
                var imageBytes = Convert.FromBase64String(base64Data);

                var imageName = $"{vehicleImages.Compania}_{vehicleImages.Sucursal}_{vehicleImages.Num_orden}_{img.Key}.jpg";
                var fullPath = Path.Combine(rutaImagenes, imageName);

                await File.WriteAllBytesAsync(fullPath, imageBytes);

                switch (img.Key)
                {
                    case "IMG_LATERAL_DERECHO":  vehicleImages.Img_lateral_derecho  = fullPath; break;
                    case "IMG_LATERAL_IZQUIERDO": vehicleImages.Img_lateral_izquierdo = fullPath; break;
                    case "IMG_FRONTAL":           vehicleImages.Img_frontal           = fullPath; break;
                    case "IMG_TRASERO":           vehicleImages.Img_trasero           = fullPath; break;
                    case "IMG_ANEXO1":            vehicleImages.Img_anexo1            = fullPath; break;
                    case "IMG_ANEXO2":            vehicleImages.Img_anexo2            = fullPath; break;
                    case "IMG_ANEXO3":            vehicleImages.Img_anexo3            = fullPath; break;
                }
            }

            return vehicleImages;
        }

        public async Task<ImgVehicles> GetImageAsync(ImgVehicles vehicleImages)
        {
            return new ImgVehicles
            {
                Compania             = vehicleImages.Compania,
                Sucursal             = vehicleImages.Sucursal,
                Num_orden            = vehicleImages.Num_orden,
                Kilometros           = vehicleImages.Kilometros,
                Placa                = vehicleImages.Placa,
                Usuario              = vehicleImages.Usuario,
                Img_lateral_derecho  = await ReadAsBase64(vehicleImages.Img_lateral_derecho),
                Img_lateral_izquierdo = await ReadAsBase64(vehicleImages.Img_lateral_izquierdo),
                Img_frontal          = await ReadAsBase64(vehicleImages.Img_frontal),
                Img_trasero          = await ReadAsBase64(vehicleImages.Img_trasero),
                Img_anexo1           = await ReadAsBase64(vehicleImages.Img_anexo1),
                Img_anexo2           = await ReadAsBase64(vehicleImages.Img_anexo2),
                Img_anexo3           = await ReadAsBase64(vehicleImages.Img_anexo3)
            };
        }

        public async Task<string> ReadAsbase64(string path) => await ReadAsBase64(path) ?? string.Empty;

        private static async Task<string?> ReadAsBase64(string? path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
                return null;

            var bytes = await File.ReadAllBytesAsync(path);
            return Convert.ToBase64String(bytes);
        }
    }
}
