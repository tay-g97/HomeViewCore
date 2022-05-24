using System.Collections.Generic;
using System.Threading.Tasks;
using HomeView.Models.Property;
using HomeView.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HomeView.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyRepository propertyRepository;

        public PropertyController(IPropertyRepository propertyRepository)
        {
            this.propertyRepository = propertyRepository;
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<Property>>> PropertySearch(PropertySearch propertySearch)
        {
            var propertyList = await propertyRepository.SearchProperties(propertySearch);
            return Ok(propertyList);
        }
    }
}
