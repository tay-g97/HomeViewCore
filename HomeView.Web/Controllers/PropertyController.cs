using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HomeView.Models.Property;
using HomeView.Repository;
using HomeView.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.IdentityModel.Tokens;

namespace HomeView.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IPhotoService _photoService;

        public PropertyController(IPropertyRepository propertyRepository, IPhotoRepository photoRepository, IAccountRepository accountRepository, IPhotoService photoService)
        {
            _propertyRepository = propertyRepository;
            _photoRepository = photoRepository;
            _accountRepository = accountRepository;
            _photoService = photoService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Property>> Create(PropertyCreate propertyCreate)
        {
            int userId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);
            string accountType = (User.Claims.First(i => i.Type == JwtRegisteredClaimNames.Typ).Value);

            if (accountType != "Seller")
            {
                return Unauthorized("You do not have a seller account");
            }

            var property = await _propertyRepository.InsertAsync(propertyCreate, userId);

            return Ok(property);
        }

        [HttpGet("{propertyId}")]
        public async Task<ActionResult<Property>> Get(int propertyId)
        {
            var property = await _propertyRepository.GetAsync((propertyId));

            if (property == null)
                return NotFound("Property not found");

            return Ok(property);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<Property>>> GetByUserId(int userId)
        {
            var username = await _accountRepository.GetUsernameByIdAsync(userId);

            if (username == null)
                return NotFound("User does not exist");

            var properties = await _propertyRepository.GetAllByIdAsync((userId));

            return Ok(properties);
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
                    var foundPhoto = await _photoRepository.GetAsync(item.PhotoId);

                    if (foundPhoto != null)
                    {
                        if (foundPhoto.UserId == userId)
                        {
                            var deleteResult = await _photoService.DeletePhotoAsync(foundPhoto.PublicId);

                            if (deleteResult.Error != null) return BadRequest(deleteResult.Error.Message);

                            var affectedRows = await _photoRepository.DeleteAsync(foundPhoto.PhotoId);
                        }
                        else
                        {
                            return Unauthorized("Photo was not uploaded by the current user.");
                        }


                    }
                }

                return Ok("Deleted property and all property images");
            }
            
            
            return Unauthorized("You did not create this property");
            
        }

        [Authorize]
        [HttpPut("update/{propertyId}")]
        public async Task<ActionResult<Property>> Update(PropertyCreate propertyCreate, int propertyId)
        {
            int userId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);
            var foundProperty = await _propertyRepository.GetAsync(propertyId);

            if (foundProperty.ToString().IsNullOrEmpty())
            {
                return NotFound("Property does not exist");
            }


            if (foundProperty.UserId == userId)
            {
                var property = await _propertyRepository.UpdateAsync(propertyCreate, propertyId, userId);

                return Ok(property);
            }

            return Unauthorized("You did not create this property");


        }

        [HttpGet("search")]
        public async Task<ActionResult<List<Property>>> PropertySearch(PropertySearch propertySearch)
        {
            var propertyList = await _propertyRepository.SearchProperties(propertySearch);
            return Ok(propertyList);
        }
    }
}
