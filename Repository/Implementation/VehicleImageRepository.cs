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
    public class VehicleImageRepository : BaseService, IImageVehicle
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
                   
                const string CommandText = @"INSERT INTO CONFITEC.IMAGENES_VEHICULOS 
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
                                            PLACA) 
                                            VALUES
                                        (:COMPANIA, 
                                            :SUCURSAL, 
                                            :NUM_ORDEN, 
                                            :IMGDER, 
                                            :IMGIZQ, 
                                            :IMGFRONT, 
                                            :IMGTRAS, 
                                            :IMGANEXO1, 
                                            :IMGANEXO2, 
                                            :IMGANEXO3,
                                            :KILOMETROS,
                                            :PLACA)";

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

            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                const string commandText = @$"SELECT COMPANIA,
                                            SUCURSAL,
                                            NUM_ORDEN,
                                            IMG_LATERAL_DERECHO,
                                            IMG_LATERAL_IZQUIERDO,
                                            IMG_FRONTAL,
                                            IMG_TRASERO,
                                            IMG_ANEXO1,
                                            IMG_ANEXO2,
                                            IMG_ANEXO3
                                        FROM CONFITEC.IMAGENES_VEHICULOS
                                            WHERE USUARIO = :user
                                        ORDER BY NUM_ORDEN DESC";

                return await con.QueryAsync<ImgVehicles>(commandText,new {user = user});
            }
            
        }

        public async Task<ImgVehicles> GetImageVehicle(int num_order)
        {

                using (OracleConnection con = new OracleConnection(_connectionString))
                {

                    const string commandText = @"SELECT *
                                                    FROM CONFITEC.IMAGENES_VEHICULOS
                                                WHERE NUM_ORDEN = :num_order";

                    return await con.QuerySingleAsync<ImgVehicles>(commandText, new { num_order = num_order });
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
                                        FROM CONFITEC.IMAGENES_VEHICULOS
                                        WHERE ROWNUM <= 4
                                    ORDER BY NUM_ORDEN ASC";

                var imgVehicle = await con.QuerySingleAsync(commandText, new { user = user });
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
                    
                const string commandText = $@"SELECT CEIL(COUNT(*) / 4) PAGINAS
                                        FROM CONFITEC.IMAGENES_VEHICULOS
                                    WHERE USUARIO = :user";

                return await con.QuerySingleAsync<int>(commandText, new { user = user });
                                          
            }    
        }

    }
}
