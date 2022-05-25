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
            IEnumerable<Property> properties;
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

              properties = await connection.QueryAsync<Property>("Property_Search",
                    new
                    {
                        Location = propertySearch.Location,
                        PropertyType = propertySearch.PropertyType,
                        Keywords = propertySearch.Keywords,
                        MinPrice = propertySearch.MinPrice,
                        MaxPrice = propertySearch.MaxPrice,
                        MinBeds = propertySearch.MinBeds,
                        MaxBeds = propertySearch.MaxBeds
                    },
                    commandType: CommandType.StoredProcedure);
            }

            return properties.ToList();
        }
    }
}