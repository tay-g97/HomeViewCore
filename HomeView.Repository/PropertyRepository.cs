using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using HomeView.Models.Property;
using Microsoft.Extensions.Configuration;

namespace HomeView.Repository
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly IConfiguration configuration;

        public PropertyRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<List<Property>> SearchProperties(PropertySearch propertySearch)
        {

            var dataTable = new DataTable();
            dataTable.Columns.Add("Location", typeof(string));
            dataTable.Columns.Add("PropertyType", typeof(string));
            dataTable.Columns.Add("Keywords", typeof(string));
            dataTable.Columns.Add("MinPrice", typeof(int));
            dataTable.Columns.Add("MaxPrice", typeof(int));
            dataTable.Columns.Add("MinBeds", typeof(int));
            dataTable.Columns.Add("MaxBeds", typeof(int));

            dataTable.Rows.Add(
                propertySearch.Location,
                propertySearch.PropertyType,
                propertySearch.Keywords,
                propertySearch.MinPrice,
                propertySearch.MaxPrice,
                propertySearch.MinBeds,
                propertySearch.MaxBeds
            );
            IEnumerable<Property> properties;
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

              properties = await connection.QueryAsync<Property>("Property_Search",
                    new { PropertySearch = dataTable.AsTableValuedParameter("dbo.PropertySearchType") },
                    commandType: CommandType.StoredProcedure);
            }

            return properties.ToList();
        }
    }
}