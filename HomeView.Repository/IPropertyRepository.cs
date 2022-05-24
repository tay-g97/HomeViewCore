using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HomeView.Models.Property;

namespace HomeView.Repository
{
    public interface IPropertyRepository
    {
        public Task<List<PropertySearch>> SearchProperties(string location, string propertyType, string keywords, 
            int minPrice, int maxPrice, int minBeds, int maxBeds,
            CancellationToken cancellationToken);
    }
}
