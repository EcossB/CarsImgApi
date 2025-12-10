using System.Data;
using System.Diagnostics;
using CarsImgApi.Models.Domain;
using CarsImgApi.Models.DTO.ImgVehicleDTOS;
using CarsImgApi.Repository.Interface;
using CarsImgApi.services;
using Microsoft.AspNetCore.Components.Forms;
using Oracle.ManagedDataAccess.Client;
using System.Drawing;

namespace CarsImgApi.Repository.Implementation
{
    public class VehicleImageRepository : BaseService, IImageVehicle
    {

        private readonly IConfiguration _configuration;
        private readonly ICreateImage _imageService;

        public VehicleImageRepository(IConfiguration configuration, ICreateImage imageService)
        {
            _configuration = configuration;
            _imageService = imageService;
        }

        public async Task<ImgVehicles> addImagesVehicle(ImgVehicles vehicle, string user)
        {
            // var stringConnection = getConnectionString(_poolSqlConnections.get(GetName(user)));
            var stringConnection = getConnectionString(BaseService._poolSqlConnections.get(user));
            var timer = Stopwatch.StartNew();
            try
            {
                using (OracleConnection con = new OracleConnection(stringConnection))
                {
                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        await con.OpenAsync();
                        cmd.CommandText = @"INSERT INTO SNAPSHOTDB.IMAGENES_VEHICULOS 
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

                        cmd.Parameters.Add(":COMPANIA", OracleDbType.Varchar2).Value = vehicle.Compania;
                        cmd.Parameters.Add(":sucursal", OracleDbType.Varchar2).Value = vehicle.Sucursal;
                        cmd.Parameters.Add(":NUM_ORDEN", OracleDbType.Int32).Value = vehicle.Orden_Numero;
                        cmd.Parameters.Add(":IMGDER", OracleDbType.Clob).Value = listImage[0];
                        cmd.Parameters.Add(":IMGIZQ", OracleDbType.Clob).Value = listImage[1];
                        cmd.Parameters.Add(":IMGFRONT", OracleDbType.Clob).Value = listImage[2];
                        cmd.Parameters.Add(":IMGTRAS", OracleDbType.Clob).Value = listImage[3];
                        cmd.Parameters.Add(":IMGANEXO1", OracleDbType.Clob).Value = listImage[4];
                        cmd.Parameters.Add(":IMGANEXO2", OracleDbType.Clob).Value = listImage[5];
                        cmd.Parameters.Add(":IMGANEXO3", OracleDbType.Clob).Value = listImage[6];
                        cmd.Parameters.Add(":KILOMETROS", OracleDbType.Int32).Value = vehicle.Kilometros;
                        cmd.Parameters.Add(":PLACA", OracleDbType.Varchar2).Value = vehicle.Placa;



                        await cmd.ExecuteNonQueryAsync();

                        var newVehicle = new ImgVehicles
                        {
                            Compania = vehicle.Compania,
                            Sucursal = vehicle.Sucursal,
                            Orden_Numero = vehicle.Orden_Numero,
                            Img_lateral_derecho = listImage[0],
                            Img_lateral_izquierdo = listImage[1],
                            Img_frontal = listImage[2],
                            Img_trasero = listImage[3],
                            Img_anexo1 = listImage[4],
                            Img_anexo2 = listImage[5],
                            Img_anexo3 = listImage[6],
                            Kilometros = vehicle.Kilometros,
                            Placa = vehicle.Placa
                        };

                        timer.Stop();
                        Console.WriteLine($"Time elapsed creating: {timer.ElapsedMilliseconds}");
                        
                        return newVehicle;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw null;
            }
        }

        public async Task<List<ImgVehicles>> getAllImagesVehicles(string user)
        {
            try 
            {
            // var stringConnection = getConnectionString(_poolSqlConnections.get(GetName(user)));
            var stringConnection = getConnectionString(BaseService._poolSqlConnections.get(user));
            var imageVehiclesList = new List<ImgVehicles>();

            var timer = new Stopwatch();
            timer.Start();

            using (OracleConnection con = new OracleConnection(stringConnection))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    await con.OpenAsync();
                    
                    // cmd.CommandText = "obtener_imagenes";
                    
                    //implementing stored procedure instead of plain sql in the code. this will bring more security
                    // cmd.CommandType = CommandType.StoredProcedure; 
                    // cmd.BindByName = true;
                    // OracleParameter outParam = new OracleParameter("p_resultado", OracleDbType.RefCursor);
                    // outParam.Direction = ParameterDirection.Output;
                    // cmd.Parameters.Add(outParam);

                    cmd.CommandText = @"SELECT COMPANIA,
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
                                             ORDER BY NUM_ORDEN ASC";

                    var reader = await cmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        var vehicle = new ImgVehicles
                        {
                            Compania = reader["COMPANIA"].ToString(),
                            Sucursal = reader["SUCURSAL"].ToString(),
                            Orden_Numero = Convert.ToInt32(reader["NUM_ORDEN"]),
                            Img_lateral_derecho = reader["IMG_LATERAL_DERECHO"].ToString(),
                            Img_lateral_izquierdo = reader["IMG_LATERAL_IZQUIERDO"].ToString(),
                            Img_frontal = reader["IMG_FRONTAL"].ToString(),
                            Img_trasero = reader["IMG_TRASERO"].ToString(),
                            Img_anexo1 = reader["IMG_ANEXO1"].ToString(),
                            Img_anexo2 = reader["IMG_ANEXO2"].ToString(),
                            Img_anexo3 = reader["IMG_ANEXO3"].ToString()
                        };

                        imageVehiclesList.Add(await _imageService.GetImageAsync(vehicle));
                    }
                }
            }
                timer.Stop();
                Console.WriteLine($"Time elapsed reading images: " + timer.ElapsedMilliseconds);
                return imageVehiclesList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        public async Task<ImgVehicles?> getImageVehicle(int num_order, string user)
        {
            // var stringConnection = getConnectionString(_poolSqlConnections.get(GetName(user)));
            var stringConnection = getConnectionString(BaseService._poolSqlConnections.get(user));
            var imgVehicleModel = new ImgVehicles();

            try
            {
                using (OracleConnection con = new OracleConnection(stringConnection))
                {
                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        await con.OpenAsync();

                        cmd.CommandText = $@"SELECT *
                                            FROM SNAPSHOTDB.IMAGENES_VEHICULOS
                                          WHERE NUM_ORDEN = {num_order} ";

                        var reader = await cmd.ExecuteReaderAsync();

                        if (reader.HasRows == false)
                        {
                            return null;
                        }

                        while (await reader.ReadAsync())
                        {
                            var imgVehicle = new ImgVehicles
                            {
                                Compania = reader["COMPANIA"].ToString(),
                                Sucursal = reader["SUCURSAL"].ToString(),
                                Orden_Numero = Convert.ToInt32(reader["NUM_ORDEN"]),
                                Img_lateral_derecho = reader["IMG_LATERAL_DERECHO"].ToString(),
                                Img_lateral_izquierdo = reader["IMG_LATERAL_IZQUIERDO"].ToString(),
                                Img_frontal = reader["IMG_FRONTAL"].ToString(),
                                Img_trasero = reader["IMG_TRASERO"].ToString(),
                                Img_anexo1 = reader["IMG_ANEXO1"].ToString(),
                                Img_anexo2 = reader["IMG_ANEXO2"].ToString(),
                                Img_anexo3 = reader["IMG_ANEXO3"].ToString()
                            };

                            imgVehicleModel = await _imageService.GetImageAsync(imgVehicle);
                        }
                    }
                }

                return imgVehicleModel;
            }
            catch (Exception e)
            {
                throw null;
            }
        }


        public async Task<List<ImgVehicles>> get4FirstImages(string user)
        {
            // var stringConnection = getConnectionString(_poolSqlConnections.get(GetName(user)));
            var stringConnection = getConnectionString(BaseService._poolSqlConnections.get(user));
            try
            {
                var first4Image = new List<ImgVehicles>();
                using (OracleConnection con = new OracleConnection(stringConnection))
                {
                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        await con.OpenAsync();

                        cmd.CommandText = @"SELECT COMPANIA,
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
                                               WHERE ROWNUM <= 6
                                            ORDER BY NUM_ORDEN ASC";

                        var reader = await cmd.ExecuteReaderAsync();

                        while (await reader.ReadAsync())
                        {
                            var imgVehicle = new ImgVehicles
                            {
                                Compania = reader["COMPANIA"].ToString(),
                                Sucursal = reader["SUCURSAL"].ToString(),
                                Orden_Numero = Convert.ToInt32(reader["NUM_ORDEN"]),
                                Img_lateral_derecho = reader["IMG_LATERAL_DERECHO"].ToString(),
                                Img_lateral_izquierdo = reader["IMG_LATERAL_IZQUIERDO"].ToString(),
                                Img_frontal = reader["IMG_FRONTAL"].ToString(),
                                Img_trasero = reader["IMG_TRASERO"].ToString(),
                                Img_anexo1 = reader["IMG_ANEXO1"].ToString(),
                                Img_anexo2 = reader["IMG_ANEXO2"].ToString(),
                                Img_anexo3 = reader["IMG_ANEXO3"].ToString()
                            };

                            first4Image.Add(await _imageService.GetImageAsync(imgVehicle));
                        }
                    }
                }

                return first4Image;

            }
            catch (Exception e)
            {
                throw null;
            }
        }

        public async Task<List<ImgVehicles>> paginateImages(string user, int pagina, int limiteRegistro)
        {
            var ImageList = await getAllImagesVehicles(user);

             return ImageList.Skip((pagina - 1) * limiteRegistro).Take(limiteRegistro).ToList();

        }

        public async Task<int> numberPages(string user)
        {
            int numberOfPages = 0;
            // var stringConnection = getConnectionString(_poolSqlConnections.get(GetName(user)));
            var stringConnection = getConnectionString(BaseService._poolSqlConnections.get(user));
            try
            {
                using (OracleConnection con = new OracleConnection(stringConnection))
                {
                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        await con.OpenAsync();

                        cmd.CommandText = @"SELECT CEIL(COUNT(*) / 4) PAGINAS
                                                FROM SNAPSHOTDB.IMAGENES_VEHICULOS";

                        var reader = await cmd.ExecuteReaderAsync();

                        while (await reader.ReadAsync())
                        {
                            numberOfPages = Convert.ToInt32(reader["PAGINAS"]);
                        }

                        return numberOfPages;
                    }
                }
            }
            catch (Exception)
            {
                throw null;
            }
        }

        /*public string GetName(string token)
        {
            DecryptService decryptService = new DecryptService(_configuration);
            return decryptService.GetName(token);
        }*/
    }
}
