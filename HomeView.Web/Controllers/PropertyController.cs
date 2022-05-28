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

            if (propertyCreate.Photo1Id.HasValue)
            {
                var photo = await _photoRepository.GetAsync(propertyCreate.Photo1Id.Value);

                if (photo.UserId != userId)
                {
                    return Unauthorized("You do not own this photo");
                }
            }
            if (propertyCreate.Photo2Id.HasValue)
            {
                var photo = await _photoRepository.GetAsync(propertyCreate.Photo2Id.Value);

                if (photo.UserId != userId)
                {
                    return Unauthorized("You do not own this photo");
                }
            }
            if (propertyCreate.Photo3Id.HasValue)
            {
                var photo = await _photoRepository.GetAsync(propertyCreate.Photo3Id.Value);

                if (photo.UserId != userId)
                {
                    return Unauthorized("You do not own this photo");
                }
            }
            if (propertyCreate.Photo4Id.HasValue)
            {
                var photo = await _photoRepository.GetAsync(propertyCreate.Photo4Id.Value);

                if (photo.UserId != userId)
                {
                    return Unauthorized("You do not own this photo");
                }
            }
            if (propertyCreate.Photo5Id.HasValue)
            {
                var photo = await _photoRepository.GetAsync(propertyCreate.Photo5Id.Value);

                if (photo.UserId != userId)
                {
                    return Unauthorized("You do not own this photo");
                }
            }


            var property = await _propertyRepository.InsertAsync(propertyCreate, userId);

            return Ok(property);
        }

        [HttpGet("{propertyId}")]
        public async Task<ActionResult<Property>> Get(int propertyId)
        {
            var property = await _propertyRepository.GetAsync((propertyId));

            return property;
        }
    }
}
