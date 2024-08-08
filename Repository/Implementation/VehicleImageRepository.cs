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

        public VehicleImageRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> addImagesVehicle(ImgVehicleRequestDTO vehicle, string user)
        {
            var stringConnection = getConnectionString(_poolSqlConnections.get(GetName(user)));

            try
            {
                using (OracleConnection con = new OracleConnection(stringConnection))
                {
                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        await con.OpenAsync();
                        cmd.CommandText = @"INSERT INTO IMAGENES_VEHICULOS 
                                            (COMPANIA,
                                             SUCURSAL,
                                             NUM_ORDEN,
                                             IMG_LATERAL_DERECHO,
                                             IMG_LATERAL_IZQUIERDO,
                                             IMG_FRONTAL, 
                                             IMG_TRASERO,
                                             IMG_ANEXO1,
                                             IMG_ANEXO2,
                                             IMG_ANEXO3) 
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
                                             :IMGANEXO3)";


                        cmd.Parameters.Add(":COMPANIA", OracleDbType.Varchar2).Value = vehicle.Compania;
                        cmd.Parameters.Add(":sucursal", OracleDbType.Varchar2).Value = vehicle.Sucursal;
                        cmd.Parameters.Add(":SUCURSAL", OracleDbType.Int32).Value = vehicle.Orden_Numero;
                        cmd.Parameters.Add(":IMGDER", OracleDbType.Clob).Value = vehicle.Img_lateral_derecho;
                        cmd.Parameters.Add(":IMGIZQ", OracleDbType.Clob).Value = vehicle.Img_lateral_izquierdo;
                        cmd.Parameters.Add(":IMGFRONT", OracleDbType.Clob).Value = vehicle.Img_frontal;
                        cmd.Parameters.Add(":IMGTRAS", OracleDbType.Clob).Value = vehicle.Img_trasero;
                        cmd.Parameters.Add(":IMGANEXO1", OracleDbType.Clob).Value = vehicle.Img_anexo1;
                        cmd.Parameters.Add(":IMGANEXO2", OracleDbType.Clob).Value = vehicle.Img_anexo2;
                        cmd.Parameters.Add(":IMGANEXO3", OracleDbType.Clob).Value = vehicle.Img_anexo3;



                        var bytesImage = Convert.FromBase64String(vehicle.Img_lateral_derecho.Remove(0, 23));
                        await File.WriteAllBytesAsync("c:\\ebatista\\ejemploImagen\\imagenEjemplo.jpeg", bytesImage);

                        await cmd.ExecuteNonQueryAsync();

                        return "Imagenes Del vehiculo Guardados!";
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<List<ImgVehicleRequestDTO>> getAllImagesVehicles(string user)
        {
            var stringConnection = getConnectionString(_poolSqlConnections.get(GetName(user)));

            var imageVehiclesList = new List<ImgVehicleRequestDTO>();

            using (OracleConnection con = new OracleConnection(stringConnection))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
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
                                            FROM IMAGENES_VEHICULOS
                                             ORDER BY NUM_ORDEN ASC";

                    var reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var vehicle = new ImgVehicleRequestDTO
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
                        imageVehiclesList.Add(vehicle);
                    }
                }
            }
            return imageVehiclesList;
        }

        public async Task<ImgVehicleRequestDTO> getImageVehicle(int num_order, string user)
        {
            var stringConnection = getConnectionString(_poolSqlConnections.get(GetName(user)));

            var imgVehicleModel = new ImgVehicleRequestDTO();
            using (OracleConnection con = new OracleConnection(stringConnection))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();

                    cmd.CommandText = @"SELECT *
                                            FROM IMAGENES_VEHICULOS
                                          WHERE NUM_ORDEN=" + num_order + "";

                    var reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var imgVehicle = new ImgVehicleRequestDTO
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
                        imgVehicleModel = imgVehicle;
                    }
                }
            }
            return imgVehicleModel;
        }


        public async Task<List<ImgVehicleRequestDTO>> getFirst4(string user)
        {
            var stringConnection = getConnectionString(_poolSqlConnections.get(GetName(user)));

            var imageVehiclesList5 = new List<ImgVehicleRequestDTO>();
            using (OracleConnection con = new OracleConnection(stringConnection))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();

                    cmd.CommandText = @"SELECT * 
                                            FROM
                                           (SELECT COMPANIA,
                                                   SUCURSAL,
                                                   NUM_ORDEN,
                                                   IMG_LATERAL_DERECHO,
                                                   IMG_LATERAL_IZQUIERDO,
                                                   IMG_FRONTAL,
                                                   IMG_TRASERO,
                                                   IMG_ANEXO1,
                                                   IMG_ANEXO2,
                                                   IMG_ANEXO3
                                                   FROM IMAGENES_VEHICULOS
                                                   order by num_orden asc)
                                            WHERE ROWNUM <= 4";

                    var reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var imgVehicle = new ImgVehicleRequestDTO
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
                        imageVehiclesList5.Add(imgVehicle);
                    }
                }
            }
            return imageVehiclesList5;

        }

        public async Task<List<ImgVehicleRequestDTO>> getNext(string user, int pagina, int limiteRegistro)
        {
            return getAllImagesVehicles(user).Result.Skip((pagina - 1) * limiteRegistro).Take(limiteRegistro).ToList();

        }

        public string GetName(string token)
        {
            DecryptService decryptService = new DecryptService(_configuration);
            return decryptService.GetName(token);
        }
    }
}
