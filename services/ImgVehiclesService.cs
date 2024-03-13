using CarsImgApi.Interface;
using CarsImgApi.Models;
using Oracle.ManagedDataAccess.Client;

namespace CarsImgApi.services
{
    public class ImgVehiclesService: IImageVehicle
    {

        private readonly string _connectionString = "User Id=snapshotdb; Password=snapshot123; Data Source=(DESCRIPTION =(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST = 127.0.0.1)(PORT = 1521))) (CONNECT_DATA =(SERVICE_NAME = xe)))";

        public void addImagesVehicle(ImgVehicleModel vehicle)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(_connectionString))
                {
                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        con.Open();
                        cmd.CommandText = @"INSERT INTO SNAPSHOTDB.IMAGENES_VEHICULOS 
                                            (COMPANIA,
                                             SUCURSAL,
                                             NUM_ORDEN,
                                             IMG_LATERAL_DERECHO,
                                             IMG_LATERAL_IZQUIERDO,
                                             IMG_FRONTAL, 
                                             IMG_TRASERO) 
                                             VALUES
                                            (:compania, :sucursal, :num_orden, :imgder, :imgizq, :imgfront, :imgtras)";


                        cmd.Parameters.Add(":compania", OracleDbType.Varchar2).Value = vehicle.Compania;
                        cmd.Parameters.Add(":sucursal", OracleDbType.Varchar2).Value = vehicle.Sucursal;
                        cmd.Parameters.Add(":num_orden", OracleDbType.Int32).Value = vehicle.Orden_Numero;
                        cmd.Parameters.Add(":imgder", OracleDbType.Clob).Value = vehicle.Img_lateral_derecho;
                        cmd.Parameters.Add(":imgizq", OracleDbType.Clob).Value = vehicle.Img_lateral_izquierdo;
                        cmd.Parameters.Add(":imgfront", OracleDbType.Clob).Value = vehicle.Img_frontal;
                        cmd.Parameters.Add(":imgtras", OracleDbType.Clob).Value = vehicle.Img_trasero;

                        cmd.ExecuteNonQuery();

                    }
                }
            } catch(Exception e) 
            { 
                throw new Exception(e.ToString());  
            }
        }

        public List<ImgVehicleModel> getAllImagesVehicles()
        {
            var imageVehiclesList = new List<ImgVehicleModel>();
            using(OracleConnection con = new OracleConnection(_connectionString))
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
                                               IMG_TRASERO
                                            FROM IMAGENES_VEHICULOS";
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
                            Img_trasero = reader["IMG_TRASERO"].ToString()
                        };
                        imageVehiclesList.Add(vehicle);
                    }
                }
            }
            return imageVehiclesList;
        }

        public ImgVehicleModel getImageVehicle(int num_order)
        {
            var imgVehicleModel = new ImgVehicleModel();
            using(OracleConnection con = new OracleConnection(_connectionString))
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
                            Img_trasero = reader["IMG_TRASERO"].ToString()
                        };
                        imgVehicleModel = imgVehicle;
                    }
                }
            }
            return imgVehicleModel;
        }
    }
}
