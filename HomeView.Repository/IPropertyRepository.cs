using System.Collections.Generic;
using System.Threading.Tasks;
using HomeView.Models.Property;

namespace HomeView.Repository
{
    public interface IPropertyRepository
    {
        public Task<List<Property>> SearchProperties(PropertySearch propertySearch);
    }
}
