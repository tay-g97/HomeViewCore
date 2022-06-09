using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using HomeView.Models.Profilephoto;
using Microsoft.Extensions.Configuration;

namespace HomeView.Repository
{
    public class ProfilephotoRepository : IProfilephotoRepository
    {

        private readonly IConfiguration _config;

        public ProfilephotoRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<int> DeleteAsync(int photoId)
        {
            int affectedRows = 0;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                affectedRows = await connection.ExecuteAsync(
                    "Profilephoto_Delete",
                    new { PhotoId = photoId },
                    commandType: CommandType.StoredProcedure);
            }

            return affectedRows;
        }

        public async Task<List<Profilephoto>> GetAllByIdAsync(int userId)
        {
            IEnumerable<Profilephoto> photos;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                photos = await connection.QueryAsync<Profilephoto>(
                    "Profilephoto_GetById",
                    new { UserId = userId },
                    commandType: CommandType.StoredProcedure);
            }

            return photos.ToList();
        }

        public async Task<Profilephoto> GetAsync(int photoId)
        {
            Profilephoto Profilephoto;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                Profilephoto = await connection.QueryFirstOrDefaultAsync<Profilephoto>(
                    "Profilephoto_Get",
                    new { PhotoId = photoId },
                    commandType: CommandType.StoredProcedure);
            }

            return Profilephoto;
        }

        public async Task<Profilephoto> InsertAsync(ProfilephotoCreate profilephotoCreate, int userId)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("PublicId", typeof(string));
            dataTable.Columns.Add("ImageUrl", typeof(string));

            dataTable.Rows.Add(profilephotoCreate.PublicId, profilephotoCreate.ImageUrl);

            int newProfilephotoId;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                newProfilephotoId = await connection.ExecuteScalarAsync<int>(
                    "Profilephoto_Insert",
                    new
                    {
                        Profilephoto = dataTable.AsTableValuedParameter("dbo.ProfilephotoType"),
                        UserId = userId,
                    },
                    commandType: CommandType.StoredProcedure);
            }

            Profilephoto photo = await GetAsync(newProfilephotoId);

            return photo;
        }
    }
}
