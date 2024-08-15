using CarsImgApi.Models;
using CarsImgApi.Models.Domain;
using CarsImgApi.Repository.Interface;

namespace CarsImgApi.services
{
    public class CreateImageService : ICreateImage
    {

        public async Task<List<string>> CreateImageAsync(ImgVehicles vehicleImages)
        {
            /*
             Image list will containt all the image base64 string send it by the webcam
             */

            var ListBase64Strings = new List<string>
            {
                vehicleImages.Img_lateral_derecho,
                vehicleImages.Img_lateral_izquierdo,
                vehicleImages.Img_frontal,
                vehicleImages.Img_trasero,
                vehicleImages.Img_anexo1,
                vehicleImages.Img_anexo2,
                vehicleImages.Img_anexo3
            };

            /**
             imageSidesList is a list that contains the differents side of the image. It's purporse its to create the image path dinamically
             */
            var imageSidesList = new List<string>
            {
                "IMAGEN_DERECHA",
                "IMAGEN_IZQUIERDA",
                "IMAGEN_FRONTAL",
                "IMAGEN_TRASERA",
                "IMAGEN_ANEXO1",
                "IMAGEN_ANEXO2",
                "IMAGEN_ANEXO3"
            };

            /*this list will contain all the path of the images. these paths are the path that we are going to insert in the database*/
            var pathImageList = new List<string>();
            int iterator = 0;

            foreach (var img in ListBase64Strings)
            {
                
                //first converting the base64 string to a byte[]
                var bytesImage = Convert.FromBase64String(img.Remove(0, 23));
                //then i create the image dinamically
                await File.WriteAllBytesAsync($"c:\\ebatista\\ejemploImagen\\{vehicleImages.Compania}_{vehicleImages.Sucursal}_{vehicleImages.Orden_Numero}_{imageSidesList[iterator]}.jpg", bytesImage);
                //and at the end i add the new image path created. 
                pathImageList.Add($"c:\\ebatista\\ejemploImagen\\{vehicleImages.Compania}_{vehicleImages.Sucursal}_{vehicleImages.Orden_Numero}_{imageSidesList[iterator]}.jpg");

                iterator = iterator + 1;
            }

            return pathImageList; //returning the list. 
        }

        public async Task<ImgVehicles> GetImageAsync(ImgVehicles vehicleImages)
        {
            //storing the path retreived from the database.
            var listPathImages = new List<string>
            {
                vehicleImages.Img_lateral_derecho,
                vehicleImages.Img_lateral_izquierdo,
                vehicleImages.Img_frontal,
                vehicleImages.Img_trasero,
                vehicleImages.Img_anexo1,
                vehicleImages.Img_anexo2,
                vehicleImages.Img_anexo3
            };

            //getting the bytes of the images and adding to a new list of type bytes.
            var listBynaryFile = new List<byte[]>();

            foreach(var path in listPathImages)
            {
               listBynaryFile.Add(await File.ReadAllBytesAsync(path));
            }

            //list to store the base 64 string 
            var listBase64String = new List<string>();  

            foreach (var bytes in listBynaryFile)
            {
                listBase64String.Add(Convert.ToBase64String(bytes));
            }

            var vehicle = new ImgVehicles
            {
                Compania = vehicleImages.Compania,
                Sucursal = vehicleImages.Sucursal,
                Orden_Numero = vehicleImages.Orden_Numero,
                Img_lateral_derecho = listBase64String[0],
                Img_lateral_izquierdo = listBase64String[1],
                Img_frontal = listBase64String[2],
                Img_trasero = listBase64String[3],
                Img_anexo1 = listBase64String[4],
                Img_anexo2  = listBase64String[5],
                Img_anexo3 = listBase64String[6]
            };

            return vehicle;
        }
    }
}
