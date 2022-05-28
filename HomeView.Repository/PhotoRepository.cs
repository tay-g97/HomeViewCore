using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using HomeView.Models.Photo;
using Microsoft.Extensions.Configuration;

namespace HomeView.Repository
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly IConfiguration _config;

        public PhotoRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<Photo> GetAsync(int photoId)
        {
            Photo photo;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                photo = await connection.QueryFirstOrDefaultAsync<Photo>(
                    "Photo_Get",
                    new {PhotoId = photoId},
                    commandType: CommandType.StoredProcedure);
            }

            return photo;
        }

        public async Task<Photo> InsertAsync(PhotoCreate photoCreate, int userId)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("PublicId", typeof(string));
            dataTable.Columns.Add("ImageUrl", typeof(string));

            dataTable.Rows.Add(photoCreate.PublicId, photoCreate.ImageUrl);

            int newPhotoId;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                newPhotoId = await connection.ExecuteScalarAsync<int>(
                    "Photo_Insert",
                    new
                    {
                        Photo = dataTable.AsTableValuedParameter("dbo.PhotoType"),
                        UserId = userId
                    },
                    commandType: CommandType.StoredProcedure);
            }

            Photo photo = await GetAsync(newPhotoId);

            return photo;
        }
    }
}
