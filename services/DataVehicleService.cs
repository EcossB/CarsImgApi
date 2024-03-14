using CarsImgApi.Interface;
using CarsImgApi.Models;
using Oracle.ManagedDataAccess.Client;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Drawing;
using System.Numerics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CarsImgApi.services
{
    public class DataVehicleService : IDatosVehiculos
    {

        private readonly string _connectionString = "User Id=snapshotdb; Password=snapshot123; Data Source=(DESCRIPTION =(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST = 127.0.0.1)(PORT = 1521))) (CONNECT_DATA =(SERVICE_NAME = xe)))";


        public List<ChasisModel> getChasis(string chasis)
        {
            var chasisM = new List<ChasisModel>();
            using(OracleConnection con = new OracleConnection(_connectionString))
            {
                using(OracleCommand cmd = con.CreateCommand()) 
                {
                    con.Open();
                    cmd.CommandText = @"SELECT CHASIS FROM SNAPSHOTDB.DATOS_VEHICULOS where chasis like '%" +chasis+ "%'";
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var chasisR = new ChasisModel
                        {
                            Chasis = reader["CHASIS"].ToString(),
                        };
                        chasisM.Add(chasisR);
                    }
                }
            }
            return chasisM;
        }

        public List<ChasisModel> getAllChasis()
        {
            List<ChasisModel> chasis = new List<ChasisModel>();
            using(OracleConnection con = new OracleConnection(_connectionString))
            {
                using(OracleCommand cmd = con.CreateCommand()) 
                {
                    con.Open();
                    cmd.CommandText = @"SELECT CHASIS FROM SNAPSHOTDB.DATOS_VEHICULOS";
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var chasisList = new ChasisModel
                        {
                            Chasis  =  reader["chasis"].ToString()
                        };
                        chasis.Add(chasisList);
                    }
                }
            }
            return chasis;
        }

        public List<ModeloVehiculo> getAllVehiclesData()
        {
            List<ModeloVehiculo> vehicles = new List<ModeloVehiculo>();
            using(OracleConnection con = new OracleConnection(_connectionString))
            {
                using(OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = @"select 
                                        compania, 
                                        sucursal, 
                                        orden_numero, 
                                        chasis, 
                                        marca, 
                                        modelo, 
                                        ano, 
                                        placa, 
                                        color 
                                        from snapshotdb.datos_vehiculos";
                    OracleDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var vehicle = new ModeloVehiculo
                        {
                            Compania = reader["compania"].ToString(),
                            Sucursal = reader["sucursal"].ToString(),
                            Orden_Numero = Convert.ToInt32(reader["orden_numero"]),
                            Chasis = reader["chasis"].ToString(),
                            Marca = Convert.ToInt32(reader["marca"]),
                            Modelo = Convert.ToInt32(reader["modelo"]),
                            Ano = Convert.ToInt32(reader["ano"]),
                            Placa = reader["placa"].ToString(),
                            Color = Convert.ToInt32(reader["color"])
                        };

                        vehicles.Add(vehicle);
                    }
                }
            }
            return vehicles;

        }

        public ModeloVehiculo getVehicleByChasis(string chasis)
        {
            var vehicleModel = new ModeloVehiculo();
            using(OracleConnection con = new OracleConnection(_connectionString))
            {
                using(OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = @"select * 
                                        from datos_vehiculos
                                        where chasis= '" + chasis + "'";
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var vehicle = new ModeloVehiculo
                        {
                            Compania = reader["compania"].ToString(),
                            Sucursal = reader["sucursal"].ToString(),
                            Orden_Numero = Convert.ToInt32(reader["orden_numero"]),
                            Chasis = reader["chasis"].ToString(),
                            Marca = Convert.ToInt32(reader["marca"]),
                            Modelo = Convert.ToInt32(reader["modelo"]),
                            Ano = Convert.ToInt32(reader["ano"]),
                            Placa = reader["placa"].ToString(),
                            Color = Convert.ToInt32(reader["color"])
                        };
                        vehicleModel = vehicle;
                    }
                }
            }
            return vehicleModel;
        }
    }
}
