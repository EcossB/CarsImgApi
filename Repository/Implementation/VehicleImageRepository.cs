using System.Data;
using System.Diagnostics;
using CarsImgApi.Models.Domain;
using CarsImgApi.Models.DTO.ImgVehicleDTOS;
using CarsImgApi.Repository.Interface;
using CarsImgApi.services;
using Microsoft.AspNetCore.Components.Forms;
using Oracle.ManagedDataAccess.Client;
using System.Drawing;
using Dapper;

namespace CarsImgApi.Repository.Implementation
{
    public class VehicleImageRepository : IImageVehicle
    {

        private readonly IConfiguration _configuration;
        private readonly ICreateImage _imageService;
        private readonly string _connectionString;

        public VehicleImageRepository(IConfiguration configuration, ICreateImage imageService)
        {
            _configuration = configuration;
            _imageService = imageService;
            _connectionString = configuration.GetConnectionString("OracleDb");
        }

        public async Task<ImgVehicles> AddImagesVehicle(ImgVehicles vehicle, string user)
        {

            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                   
                const string CommandText = @"INSERT INTO snapshotdb.IMAGENES_VEHICULOS 
                                        (COMPANIA,
                                            SUCURSAL,
                                            NUM_ORDEN,
                                            IMG_LATERAL_DERECHO,
                                            IMG_LATERAL_IZQUIERDO,
                                            IMG_FRONTAL, 
                                            IMG_TRASERO,
                                            IMG_ANEXO1,
                                            IMG_ANEXO2,
                                            IMG_ANEXO3,
                                            KILOMETROS,
                                            PLACA,
                                            USUARIO) 
                                            VALUES
                                        (:COMPANIA, 
                                            :SUCURSAL, 
                                            :NUM_ORDEN, 
                                            :Img_lateral_derecho, 
                                            :Img_lateral_izquierdo, 
                                            :Img_frontal, 
                                            :Img_trasero, 
                                            :Img_anexo1, 
                                            :Img_anexo2, 
                                            :Img_anexo3,
                                            :KILOMETROS,
                                            :PLACA,
                                            :USUARIO)";

                var listImage = await _imageService.CreateImageAsync(vehicle);
                vehicle.Img_lateral_derecho = listImage[0];
                vehicle.Img_lateral_izquierdo = listImage[1];
                vehicle.Img_frontal = listImage[2];
                vehicle.Img_trasero = listImage[3];
                vehicle.Img_anexo1 = listImage[4];
                vehicle.Img_anexo2 = listImage[5];
                vehicle.Img_anexo3 = listImage[6];
                

                await con.ExecuteAsync(CommandText, vehicle);
            }

            return vehicle;

        }

        public async Task<IEnumerable<ImgVehicles>> GetAllImagesVehicles(string user)
        {

            var imageList = new List<ImgVehicles>();

            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                const string commandText = @"SELECT COMPANIA,
                                            SUCURSAL,
                                            NUM_ORDEN,
                                            IMG_LATERAL_DERECHO,
                                            IMG_LATERAL_IZQUIERDO,
                                            IMG_FRONTAL,
                                            IMG_TRASERO,
                                            IMG_ANEXO1,
                                            IMG_ANEXO2,
                                            IMG_ANEXO3
                                        FROM SNAPSHOTDB.IMAGENES_VEHICULOS
                                            WHERE USUARIO = upper(:p_user)
                                        ORDER BY NUM_ORDEN DESC";

                var vehicleList = await con.QueryAsync<ImgVehicles>(commandText, new { p_user = user });
    
                foreach (var vehicle in vehicleList)
                {
                    imageList.Add(await _imageService.GetImageAsync(vehicle));
                }

                return imageList;

            }
            
        }

        public async Task<ImgVehicles> GetImageVehicle(int num_order)
        {

                using (OracleConnection con = new OracleConnection(_connectionString))
                {

                    const string commandText = @"SELECT *
                                                    FROM SNAPSHOTDB.IMAGENES_VEHICULOS
                                                WHERE NUM_ORDEN = :pnum_order";

                    var vehicle = await con.QuerySingleAsync<ImgVehicles>(commandText, new { pnum_order = num_order });
                    await _imageService.GetImageAsync(vehicle);
                    return vehicle; 
            }
        }


        public async Task<IEnumerable<ImgVehicles>> Get4FirstImages(string user)
        {
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                const string commandText = $@"SELECT COMPANIA,
                                        SUCURSAL,
                                        NUM_ORDEN,
                                        IMG_LATERAL_DERECHO,
                                        IMG_LATERAL_IZQUIERDO,
                                        IMG_FRONTAL,
                                        IMG_TRASERO,
                                        IMG_ANEXO1,
                                        IMG_ANEXO2,
                                        IMG_ANEXO3
                                        FROM SNAPSHOTDB.IMAGENES_VEHICULOS
                                        WHERE ROWNUM <= 4
                                            and usuario = :p_user
                                    ORDER BY NUM_ORDEN ASC";

                var imgVehicle = await con.QuerySingleAsync(commandText, new { p_user = user });
                imgVehicle = await _imageService.GetImageAsync(imgVehicle);

                return imgVehicle;
            }                       
        }

        public async Task<List<ImgVehicles>> PaginateImages(string user, int pagina, int limiteRegistro)
        {
            var ImageList = await GetAllImagesVehicles(user);
            return ImageList.Skip((pagina - 1) * limiteRegistro).Take(limiteRegistro).ToList();
        }

        public async Task<int> NumberPages(string user)
        {

            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                    
                const string commandText = @"SELECT CEIL(COUNT(*) / 4) PAGINAS
                                        FROM SNAPSHOTDB.IMAGENES_VEHICULOS
                                    WHERE USUARIO = upper(:p_user)";

                return await con.QuerySingleAsync<int>(commandText, new { p_user = user });
                                          
            }    
        }

    }
}
