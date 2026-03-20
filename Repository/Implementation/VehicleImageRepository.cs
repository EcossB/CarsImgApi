using CarsImgApi.Models.Domain;
using CarsImgApi.Models.DTO.ImgVehicleDTOS;
using CarsImgApi.Repository.Interface;
using CarsImgApi.services;
using Dapper;
using Microsoft.AspNetCore.Components.Forms;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;

namespace CarsImgApi.Repository.Implementation
{
    public class VehicleImageRepository : IImageVehicle
    {

        private readonly IConfiguration _configuration;
        private readonly ICreateImage _imageService;
        private readonly string? _connectionString;

        public VehicleImageRepository(IConfiguration configuration, ICreateImage imageService)
        {
            _configuration = configuration;
            _imageService = imageService;
            _connectionString = configuration.GetConnectionString("OracleDb");
        }

        public async Task<ImgVehicles?> AddImagesVehicle(ImgVehicles vehicle, string user)
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
                
                if(vehicleList == null) return null;
    
                // foreach (var vehicle in vehicleList)
                // {
                    // imageList.Add(await _imageService.GetImageAsync(vehicle));
                // }

                return vehicleList;

            }
            
        }

        public async Task<ImgVehicles> GetImageVehicle(int num_order)
        {

                using (OracleConnection con = new OracleConnection(_connectionString))
                {

                    const string commandText = @"SELECT *
                                                    FROM SNAPSHOTDB.IMAGENES_VEHICULOS
                                                WHERE NUM_ORDEN = :pnum_order";

                    var vehicle = await con.QuerySingleOrDefaultAsync<ImgVehicles>(commandText, new { pnum_order = num_order });

                    if (vehicle == null) return null;
                    
                    //var vehicleImage = await _imageService.GetImageAsync(vehicle);
                    
                    return vehicle;
            }
        }


        public async Task<IEnumerable<ImgVehicles>> Get4FirstImages(string user)
        {
            var imageList = new List<ImgVehicles>();
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
                                        WHERE USUARIO = :p_user
                                            and ROWNUM <= 4
                                    ORDER BY NUM_ORDEN ASC";

                var imgVehicle = await con.QueryAsync<ImgVehicles>(commandText, new {p_user = user});
                
                if (imgVehicle == null) return null;

                // foreach (var img in imgVehicle)
                // {
                    // imageList.Add(await _imageService.GetImageAsync(img));
                // }
                    
                return imgVehicle;
            }                       
        }

        public async Task<PagedResult<ImgVehicles>> PaginateImages(string user, int pagina, int limiteRegistro)
        {
            int minRow = (pagina - 1) * limiteRegistro;
            int maxRow = pagina * limiteRegistro;
            var dataIMage = new List<ImgVehicles>();

            using(var con = new OracleConnection(_connectionString))
            {
                const string totalRegistros = "SELECT COUNT(1) FROM SNAPSHOTDB.IMAGENES_VEHICULOS WHERE USUARIO = UPPER(:p_user)";
                int totalRecords = await con.ExecuteScalarAsync<int>(totalRegistros, new { p_user = user });

                const string sqlData = @"
                                        SELECT * FROM (
                                            SELECT a.*, ROWNUM rnum FROM (
                                                SELECT * FROM SNAPSHOTDB.IMAGENES_VEHICULOS 
                                                WHERE USUARIO = upper(:p_user)
                                                ORDER BY FECHA_CREACION DESC 
                                            ) a
                                            WHERE ROWNUM <= :MaxRow
                                        )
                                        WHERE rnum > :MinRow";

                var data = await con.QueryAsync<ImgVehicles>(sqlData, new { p_user = user, MaxRow = maxRow, MinRow = minRow });
                
                if (data == null) return null;

                // foreach (var img in data)
                // {
                    // dataIMage.Add(await _imageService.GetImageAsync(img));
                // }
                

                return new PagedResult<ImgVehicles>
                {
                    Data = data,
                    TotalRecords = totalRecords,
                    PageNumber = pagina,
                    PageSize = limiteRegistro
                };

            }

        }

    }
}
