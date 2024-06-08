using CarsImgApi.Interface;
using CarsImgApi.Models;
using Oracle.ManagedDataAccess.Client;

namespace CarsImgApi.services
{
    public class ImgVehiclesService: BaseService, IImageVehicle
    {

        private readonly IConfiguration _configuration;

        public ImgVehiclesService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> addImagesVehicle(ImgVehicleModel vehicle, string user)
        {
            var stringConnection = base.getConnectionString(BaseService._poolSqlConnections.get(GetName(user)));

            try
            {
                await using (OracleConnection con = new OracleConnection(stringConnection))
                {
                   await using (OracleCommand cmd = con.CreateCommand())
                    {
                        con.Open();
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

                        cmd.ExecuteNonQuery();

                        return "Imagenes Del vehiculo Guardados!";
                    }
                }
            } catch(Exception e) 
            { 
                throw new Exception(e.ToString());  
            }
        }

        public List<ImgVehicleModel> getAllImagesVehicles(string user)
        {
            var stringConnection = base.getConnectionString(BaseService._poolSqlConnections.get(GetName(user)));

            var imageVehiclesList = new List<ImgVehicleModel>();
            using(OracleConnection con = new OracleConnection(stringConnection))
            {
                using(OracleCommand cmd = con.CreateCommand())
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
                                            FROM SNAPSHOTDB.IMAGENES_VEHICULOS
                                             ORDER BY NUM_ORDEN ASC";
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var vehicle = new ImgVehicleModel
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

        public ImgVehicleModel getImageVehicle(int num_order, string user)
        {
            var stringConnection = base.getConnectionString(BaseService._poolSqlConnections.get(GetName(user)));

            var imgVehicleModel = new ImgVehicleModel();
            using(OracleConnection con = new OracleConnection(stringConnection))
            {
                using(OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = @"SELECT *
                                            FROM SNAPSHOTDB.IMAGENES_VEHICULOS
                                          WHERE NUM_ORDEN=" + num_order + "";
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var imgVehicle = new ImgVehicleModel
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


        public List<ImgVehicleModel> getFirst4(string user)
        {
            var stringConnection = base.getConnectionString(BaseService._poolSqlConnections.get(GetName(user)));

            var imageVehiclesList5 = new List<ImgVehicleModel>();
            using (OracleConnection con = new OracleConnection(stringConnection))
            {
                using(OracleCommand cmd = con.CreateCommand())
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
                                                   FROM SNAPSHOTDB.IMAGENES_VEHICULOS
                                                   order by num_orden asc)
                                            WHERE ROWNUM <= 4";

                    OracleDataReader reader = cmd.ExecuteReader();
                    while(reader.Read())
                    {
                        var imgVehicle = new ImgVehicleModel
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

        public List<ImgVehicleModel> getNext(string user, int pagina, int limiteRegistro)
        {
            return this.getAllImagesVehicles(user).Skip((pagina - 1) * limiteRegistro).Take(limiteRegistro).ToList();
        }

        public string GetName(string token)
        {
            DecryptService decryptService = new DecryptService(this._configuration);
            return decryptService.GetName(token);
        }
    }
}
