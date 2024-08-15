using Oracle.ManagedDataAccess.Client;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Drawing;
using System.Numerics;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using CarsImgApi.services;
using CarsImgApi.Repository.Interface;
using CarsImgApi.Models.DTO.VehicleDTOS;
using CarsImgApi.Models.DTO.ImgVehicleDTOS;
using CarsImgApi.Models.Domain;

namespace CarsImgApi.Repository.Implementation
{
    public class VehicleRepository : PoolSqlConnections, IDataVehicle
    {

        private readonly IConfiguration _configuration;

        public VehicleRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetName(string token)
        {
            DecryptService decryptService = new DecryptService(_configuration);
            return decryptService.GetName(token);
        }

        /*
         @Purporse: This method purporse was to retrieve the chasis depending on what the user type. 

        Example: If i write K4352, this method will bring to me al the chasis that match with the user string. 
                  this way i can create a litlle filter to get dinamically the chasis the user want. 
         
        public async Task<IEnumerable<ChasisResponseDto>> getChasis(string chasis, string user)
        {

            var stringConnection = getConnectionString(BaseService._poolSqlConnections.get(GetName(user)));

            List<ChasisResponseDto> chasisList = new List<ChasisResponseDto>();

            using (OracleConnection con = new OracleConnection(stringConnection))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    await con.OpenAsync();
                    cmd.CommandText = @"SELECT CHASIS FROM DATOS_VEHICULOS where chasis like '%" + chasis + "%'";
                    var reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        var chasisR = new ChasisResponseDto
                        {
                            Chasis = reader["CHASIS"].ToString()
                        };
                        chasisList.Add(chasisR);
                    }
                }
            }
            return chasisList;
        }

        public async Task<IEnumerable<ChasisResponseDto>> getAllChasis(string user)
        {
            var stringConnection = getConnectionString(BaseService._poolSqlConnections.get(GetName(user)));

            List<ChasisResponseDto> chasis = new List<ChasisResponseDto>();

            using (OracleConnection con = new OracleConnection(stringConnection))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    await con.OpenAsync();
                    cmd.CommandText = @"SELECT CHASIS FROM DATOS_VEHICULOS";
                    var reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        var chasisList = new ChasisResponseDto
                        {
                            Chasis = reader["chasis"].ToString()
                        };
                        chasis.Add(chasisList);
                    }
                }
            }
            return chasis;
        } these method wont be used it. That's why are commented.*/ 

        public async Task<IEnumerable<Vehicle>> getAllVehiclesData(string user)
        {
            var stringConnection = getConnectionString(BaseService._poolSqlConnections.get(GetName(user)));

            List<Vehicle> vehicles = new List<Vehicle>();

            using (OracleConnection con = new OracleConnection(stringConnection))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    await con.OpenAsync();
                    cmd.CommandText = @"select 
                                        compania, 
                                        sucursal, 
                                        orden_numero, 
                                        fecha_orden,
                                        nombre_cliente,
                                        marca, 
                                        modelo, 
                                        placa 
                                        from SNAPSHOTDB.V_ORDENES_PARA_RECEPCION";
                    var reader = await cmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        var vehicle = new Vehicle
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

        public async Task<Vehicle> getVehicleByPlaca(string placa, string user)
        {

            var stringConnection = getConnectionString(BaseService._poolSqlConnections.get(GetName(user)));

            var Vehicle = new Vehicle();

            using (OracleConnection con = new OracleConnection(stringConnection))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    await con.OpenAsync();
                    cmd.CommandText = @"select * 
                                        from SNAPSHOTDB.v_ordenes_para_recepcion
                                        where placa= '" + placa + "'";
                    var reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        var vehicleRead = new Vehicle
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
                        Vehicle = vehicleRead;
                    }
                }
            }
            return Vehicle;
        }


    }
}
