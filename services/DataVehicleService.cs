using CarsImgApi.Interface;
using CarsImgApi.Models;
using Oracle.ManagedDataAccess.Client;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Drawing;
using System.Numerics;
using System.Reflection;
using System.Text.RegularExpressions;
using CarsImgApi.Entity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace CarsImgApi.services
{
    public class DataVehicleService : PoolSqlConnections, IDatosVehiculos
    {

        private readonly IConfiguration _configuration;

        public DataVehicleService(IConfiguration configuration)
        {
            _configuration = configuration; 
        }

        public string GetName(string token)
        {
            DecryptService decryptService = new DecryptService(this._configuration);
            return decryptService.GetName(token);
        }

        public List<ChasisModel> getChasis(string chasis, string user)
        {

            var stringConnection = base.getConnectionString(BaseService._poolSqlConnections.get(GetName(user)));

            var chasisM = new List<ChasisModel>();
            using (OracleConnection con = new OracleConnection(stringConnection))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = @"SELECT CHASIS FROM SNAPSHOTDB.DATOS_VEHICULOS where chasis like '%" + chasis + "%'";
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

        public List<ChasisModel> getAllChasis(string user)
        {
            var stringConnection = base.getConnectionString(BaseService._poolSqlConnections.get(GetName(user)));

            List<ChasisModel> chasis = new List<ChasisModel>();
            using(OracleConnection con = new OracleConnection(stringConnection))
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

        public List<ModeloVehiculoRecepcion> getAllVehiclesData(string user)
        {
            var stringConnection = base.getConnectionString(BaseService._poolSqlConnections.get(GetName(user)));

            List<ModeloVehiculoRecepcion> vehicles = new List<ModeloVehiculoRecepcion>();
            using(OracleConnection con = new OracleConnection(stringConnection))
            {
                using(OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = @"select 
                                        compania, 
                                        sucursal, 
                                        orden_numero, 
                                        fecha_orden,
                                        nombre_cliente,
                                        marca, 
                                        modelo, 
                                        placa 
                                        from snapshotdb.V_ORDENES_PARA_RECEPCION";
                    OracleDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var vehicle = new ModeloVehiculoRecepcion
                        {
                            Compania = reader["compania"].ToString(),
                            Sucursal = reader["sucursal"].ToString(),
                            Orden_Numero = Convert.ToInt32(reader["orden_numero"]),
                            Fecha_orden = (DateTime)reader["fecha_orden"],
                            Nombre_cliente = reader["nombre_cliente"].ToString(),
                            Marca = reader["marca"].ToString(),
                            Modelo = reader["modelo"].ToString(),
                            Placa = reader["placa"].ToString(),
                        };

                        vehicles.Add(vehicle);
                    }
                }
            }
            return vehicles;

        }

        public ModeloVehiculoRecepcion getVehicleByPlaca(string placa, string user)
        {

            var stringConnection = base.getConnectionString(BaseService._poolSqlConnections.get(GetName(user)));
            var vehicleModel = new ModeloVehiculoRecepcion();
            using(OracleConnection con = new OracleConnection(stringConnection))
            {
                using(OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = @"select * 
                                        from datos_vehiculos
                                        where placa= '" + placa + "'";
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var vehicle = new ModeloVehiculoRecepcion
                        {
                            Compania = reader["compania"].ToString(),
                            Sucursal = reader["sucursal"].ToString(),
                            Orden_Numero = Convert.ToInt32(reader["orden_numero"]),
                            Fecha_orden = (DateTime)reader["fecha_orden"],
                            Nombre_cliente = reader["nombre_cliente"].ToString(),
                            Marca = reader["marca"].ToString(),
                            Modelo = reader["modelo"].ToString(),
                            Placa = reader["placa"].ToString(),
                        };
                        vehicleModel = vehicle;
                    }
                }
            }
            return vehicleModel;
        }


    }
}
