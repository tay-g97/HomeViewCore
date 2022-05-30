using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HomeView.Models.Property;

namespace HomeView.Repository
{
    public interface IPropertyRepository
    {
        public Task<List<Property>> SearchProperties(PropertySearch propertySearch);

        public Task<int> DeleteAsync(int propertyId);

        public Task<Property> InsertAsync(PropertyCreate propertyCreate, int userId);

        public Task<Property> GetAsync(int propertyId);

        public Task<List<Property>> GetAllByIdAsync(int userId);

        public Task<Property> UpdateAsync(PropertyCreate propertyCreate, int propertyId, int userId);
    }
}
