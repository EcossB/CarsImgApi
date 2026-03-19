using System.Data;
using CarsImgApi.Models.Domain;
using CarsImgApi.Models.DTO.ImgVehicleDTOS;
using CarsImgApi.Repository.Interface;
using Oracle.ManagedDataAccess.Client;
using Dapper;

namespace CarsImgApi.Repository.Implementation
{
    public class VehicleImageRepository : IImageVehicle
    {
        private readonly ICreateImage _imageService;
        private readonly string _connectionString;

        public VehicleImageRepository(IConfiguration configuration, ICreateImage imageService)
        {
            _imageService = imageService;
            _connectionString = configuration.GetConnectionString("OracleDb")
                ?? throw new InvalidOperationException("OracleDb connection string is not configured.");
        }

        public async Task<ImgVehicles> AddImagesVehicle(ImgVehicles vehicle, string user)
        {
            var vehicleWithPaths = await _imageService.CreateImageAsync(vehicle);

            using var con = new OracleConnection(_connectionString);

            const string sql = @"INSERT INTO snapshotdb.IMAGENES_VEHICULOS 
                                    (COMPANIA, SUCURSAL, NUM_ORDEN,
                                     IMG_LATERAL_DERECHO, IMG_LATERAL_IZQUIERDO,
                                     IMG_FRONTAL, IMG_TRASERO,
                                     IMG_ANEXO1, IMG_ANEXO2, IMG_ANEXO3,
                                     KILOMETROS, PLACA, USUARIO)
                                 VALUES
                                    (:Compania, :Sucursal, :Num_orden,
                                     :Img_lateral_derecho, :Img_lateral_izquierdo,
                                     :Img_frontal, :Img_trasero,
                                     :Img_anexo1, :Img_anexo2, :Img_anexo3,
                                     :Kilometros, :Placa, :Usuario)";

            await con.ExecuteAsync(sql, vehicleWithPaths);

            return vehicleWithPaths;
        }

        public async Task<IEnumerable<ImgVehicles>> GetAllImagesVehicles(string user)
        {
            using var con = new OracleConnection(_connectionString);

            const string sql = @"SELECT COMPANIA, SUCURSAL, NUM_ORDEN,
                                         IMG_LATERAL_DERECHO, IMG_LATERAL_IZQUIERDO,
                                         IMG_FRONTAL, IMG_TRASERO,
                                         IMG_ANEXO1, IMG_ANEXO2, IMG_ANEXO3,
                                         KILOMETROS, PLACA, USUARIO
                                    FROM SNAPSHOTDB.IMAGENES_VEHICULOS
                                   WHERE USUARIO = upper(:p_user)
                                   ORDER BY NUM_ORDEN DESC";

            var rows = await con.QueryAsync<ImgVehicles>(sql, new { p_user = user });

            return await Task.WhenAll(rows.Select(_imageService.GetImageAsync));
        }

        public async Task<ImgVehicles?> GetImageVehicle(int num_order)
        {
            using var con = new OracleConnection(_connectionString);

            const string sql = @"SELECT * FROM SNAPSHOTDB.IMAGENES_VEHICULOS
                                  WHERE NUM_ORDEN = :p_num_order";

            var vehicle = await con.QuerySingleOrDefaultAsync<ImgVehicles>(sql, new { p_num_order = num_order });
            if (vehicle is null)
                return null;

            return await _imageService.GetImageAsync(vehicle);
        }

        public async Task<IEnumerable<ImgVehicles>> Get4FirstImages(string user)
        {
            using var con = new OracleConnection(_connectionString);

            const string sql = @"SELECT COMPANIA, SUCURSAL, NUM_ORDEN,
                                         IMG_LATERAL_DERECHO, IMG_LATERAL_IZQUIERDO,
                                         IMG_FRONTAL, IMG_TRASERO,
                                         IMG_ANEXO1, IMG_ANEXO2, IMG_ANEXO3,
                                         KILOMETROS, PLACA, USUARIO
                                    FROM (
                                        SELECT * FROM SNAPSHOTDB.IMAGENES_VEHICULOS
                                         WHERE USUARIO = :p_user
                                         ORDER BY NUM_ORDEN DESC
                                    )
                                   WHERE ROWNUM <= 4";

            var rows = await con.QueryAsync<ImgVehicles>(sql, new { p_user = user });

            return await Task.WhenAll(rows.Select(_imageService.GetImageAsync));
        }

        public async Task<List<ImgVehicles>> PaginateImages(string user, int pagina, int limiteRegistro)
        {
            using var con = new OracleConnection(_connectionString);

            int offset = (pagina - 1) * limiteRegistro;

            const string sql = @"SELECT COMPANIA, SUCURSAL, NUM_ORDEN,
                                         IMG_LATERAL_DERECHO, IMG_LATERAL_IZQUIERDO,
                                         IMG_FRONTAL, IMG_TRASERO,
                                         IMG_ANEXO1, IMG_ANEXO2, IMG_ANEXO3,
                                         KILOMETROS, PLACA, USUARIO
                                    FROM SNAPSHOTDB.IMAGENES_VEHICULOS
                                   WHERE USUARIO = upper(:p_user)
                                   ORDER BY NUM_ORDEN DESC
                                   OFFSET :p_offset ROWS FETCH NEXT :p_limit ROWS ONLY";

            var rows = await con.QueryAsync<ImgVehicles>(sql, new { p_user = user, p_offset = offset, p_limit = limiteRegistro });

            return [.. await Task.WhenAll(rows.Select(_imageService.GetImageAsync))];
        }

        public async Task<int> NumberPages(string user)
        {
            using var con = new OracleConnection(_connectionString);

            const string sql = @"SELECT CEIL(COUNT(*) / 4) PAGINAS
                                    FROM SNAPSHOTDB.IMAGENES_VEHICULOS
                                   WHERE USUARIO = upper(:p_user)";

            return await con.QuerySingleAsync<int>(sql, new { p_user = user });
        }
    }
}
