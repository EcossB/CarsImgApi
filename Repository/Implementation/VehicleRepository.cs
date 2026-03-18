using Oracle.ManagedDataAccess.Client;
using CarsImgApi.services;
using CarsImgApi.Repository.Interface;
using CarsImgApi.Models.Domain;
using Dapper;

namespace CarsImgApi.Repository.Implementation
{
    public class VehicleRepository : IDataVehicle
    {

        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public VehicleRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("OracleDb");
        }

        public async Task<IEnumerable<Vehicle>> GetAllVehiclesData(string user)
        {

            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                   
                    const string sql = @"select 
                                        compania, 
                                        sucursal, 
                                        NUM_ORDEN, 
                                        fecha_orden,
                                        nombre_cliente,
                                        marca, 
                                        modelo, 
                                        placa 
                                    from snapshotdb.V_ORDENES_PARA_RECEPCION
                                    WHERE RECEPTOR = upper(:p_user) ";

                // Dapper abre la conexión, pasa el parámetro, ejecuta y mapea todo automáticamente
                // basándose en el nombre de las columnas (por eso los AS si hay diferencias de mayúsculas/minúsculas).
                var vehicles = await con.QueryAsync<Vehicle>(sql, new { p_user = user });

                return vehicles;
            }
               
        }

        public async Task<Vehicle> GetVehicleByPlaca(string placa, string user)
        {
                
            /*Utilizamos el using para abrir y cerrar los recursos y dejar la memoria libre cuando termine de correr.*/
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                    
                const string sql = @"select * 
                                    from snapshotdb.v_ordenes_para_recepcion
                                    where placa = :p_placa ";

                /*Utilizamos Dapper para hacer el mapeo de parametros y el retorno de la entidad de manera limpiar y eficiente. (sin usar ADO puro)*/
                return await con.QuerySingleAsync<Vehicle>(sql, new { p_placa = placa });

            }
        } 
    }
}
