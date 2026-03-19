using Oracle.ManagedDataAccess.Client;
using CarsImgApi.Repository.Interface;
using CarsImgApi.Models.Domain;
using Dapper;

namespace CarsImgApi.Repository.Implementation
{
    public class VehicleRepository : IDataVehicle
    {
        private readonly string _connectionString;

        public VehicleRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("OracleDb")
                ?? throw new InvalidOperationException("OracleDb connection string is not configured.");
        }

        public async Task<IEnumerable<Vehicle>> GetAllVehiclesData(string user)
        {
            using var con = new OracleConnection(_connectionString);

            const string sql = @"SELECT compania,
                                        sucursal,
                                        NUM_ORDEN,
                                        fecha_orden,
                                        nombre_cliente,
                                        marca,
                                        modelo,
                                        placa
                                   FROM snapshotdb.V_ORDENES_PARA_RECEPCION
                                  WHERE RECEPTOR = upper(:p_user)";

            return await con.QueryAsync<Vehicle>(sql, new { p_user = user });
        }

        public async Task<Vehicle?> GetVehicleByPlaca(string placa, string user)
        {
            using var con = new OracleConnection(_connectionString);

            const string sql = @"SELECT *
                                   FROM snapshotdb.v_ordenes_para_recepcion
                                  WHERE placa = :p_placa";

            return await con.QuerySingleOrDefaultAsync<Vehicle>(sql, new { p_placa = placa });
        }
    }
}
