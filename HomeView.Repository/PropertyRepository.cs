using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using HomeView.Models.Property;
using Microsoft.Extensions.Configuration;

namespace HomeView.Repository
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly IConfiguration _config;

        public PropertyRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<Property> GetAsync(int propertyId)
        {
            Property property;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                property = await connection.QueryFirstOrDefaultAsync<Property>(
                    "Property_Get",
                    new {PropertyId = propertyId},
                    commandType: CommandType.StoredProcedure);
            }

            return property;
        }

        public async Task<Property> InsertAsync(PropertyCreate propertyCreate, int userId)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("PropertyId", typeof(int));
            dataTable.Columns.Add("Propertyname", typeof(string));
            dataTable.Columns.Add("GuidePrice", typeof(decimal));
            dataTable.Columns.Add("Propertytype", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("Bedrooms", typeof(int));
            dataTable.Columns.Add("Bathrooms", typeof(int));
            dataTable.Columns.Add("Icons", typeof(string));
            dataTable.Columns.Add("Addressline1", typeof(string));
            dataTable.Columns.Add("Addressline2", typeof(string));
            dataTable.Columns.Add("Addressline3", typeof(string));
            dataTable.Columns.Add("Town", typeof(string));
            dataTable.Columns.Add("City", typeof(string));
            dataTable.Columns.Add("Postcode", typeof(string));
            dataTable.Columns.Add("Photo1Id", typeof(int));
            dataTable.Columns.Add("Photo2Id", typeof(int));
            dataTable.Columns.Add("Photo3Id", typeof(int));
            dataTable.Columns.Add("Photo4Id", typeof(int));
            dataTable.Columns.Add("Photo5Id", typeof(int));

            dataTable.Rows.Add(
                propertyCreate.PropertyId,
                propertyCreate.Propertyname,
                propertyCreate.Guideprice,
                propertyCreate.Propertytype,
                propertyCreate.Description,
                propertyCreate.Bedrooms,
                propertyCreate.Bathrooms,
                propertyCreate.Icons,
                propertyCreate.Addressline1,
                propertyCreate.Addressline2,
                propertyCreate.Addressline3,
                propertyCreate.Town,
                propertyCreate.City,
                propertyCreate.Postcode,
                propertyCreate.Photo1Id,
                propertyCreate.Photo2Id,
                propertyCreate.Photo3Id,
                propertyCreate.Photo4Id,
                propertyCreate.Photo5Id
            );

            int newPropertyId;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                newPropertyId = await connection.ExecuteScalarAsync<int>(
                    "Property_Insert",
                    new {Property = dataTable.AsTableValuedParameter("dbo.PropertyType"), UserId = userId},
                    commandType: CommandType.StoredProcedure
                );
            }

            Property property = await GetAsync(newPropertyId);

            return property;
        }
    }
}
