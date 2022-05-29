using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using HomeView.Models.Property;
using HomeView.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HomeView.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPhotoRepository _photoRepository;

        public PropertyController(IPropertyRepository propertyRepository, IPhotoRepository photoRepository)
        {
            _propertyRepository = propertyRepository;
            _photoRepository = photoRepository;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Property>> Create(PropertyCreate propertyCreate)
        {
            int userId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);

            var property = await _propertyRepository.InsertAsync(propertyCreate, userId);

            return Ok(property);
        }

        [HttpGet("{propertyId}")]
        public async Task<ActionResult<Property>> Get(int propertyId)
        {
            var property = await _propertyRepository.GetAsync((propertyId));

            return property;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<Property>>> GetByUserId(int userId)
        {
            var properties = await _propertyRepository.GetAllByIdAsync((userId));

            return properties;
        }

        [Authorize]
        [HttpDelete("delete/{propertyId}")]
        public async Task<ActionResult<int>> Delete(int propertyId)
        {
            int userId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);

            var foundProperty = await _propertyRepository.GetAsync(propertyId);
            var foundImages = await _photoRepository.GetAllByPropertyIdAsync(propertyId);

            if (foundProperty == null)
            {
                return NotFound("Property doesn't exist");
            }

            if (foundProperty.UserId == userId)
            {
                foreach (var item in foundImages)
                {
                    var affectedPhotoRows = await _photoRepository.DeleteAsync(item.PhotoId);
                }

                var affectedRows = await _propertyRepository.DeleteAsync(propertyId);

                return Ok("Deleted "+affectedRows+" property rows and all property images");
            }
            
            
            return Unauthorized("You did not create this property");
            
        }
    }
}
