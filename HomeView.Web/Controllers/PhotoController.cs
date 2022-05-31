using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using HomeView.Models.Photo;
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
    public class PhotoController : ControllerBase
    {
        private readonly IPhotoRepository _photoRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IPhotoService _photoService;
        private readonly IPropertyRepository _propertyRepository;

        public PhotoController(
            IPhotoRepository photoRepository,
            IAccountRepository accountRepository,
            IPropertyRepository propertyRepository,
            IPhotoService photoService)
        {
            _photoRepository = photoRepository;
            _accountRepository = accountRepository;
            _propertyRepository = propertyRepository;
            _photoService = photoService;
        }

        [Authorize]
        [HttpPost("{propertyId}")]
        public async Task<ActionResult<Photo>> UploadPhoto(IFormFile file, int propertyId)
        {
            int userId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);
            var property = await _propertyRepository.GetAsync(propertyId);
            var photoList = await _photoRepository.GetAllByPropertyIdAsync(propertyId);
            var thumbnail = HttpContext.Request.Query["thumbnail"];

            foreach (var item in photoList)
            {
                if (thumbnail == true && thumbnail == item.Thumbnail)
                {
                    return BadRequest("This property already has a thumbnail image");
                }
            }

            if (userId != property.UserId)
            {
                return Unauthorized("You do not own this property listing");
            }

            if (thumbnail.IsNullOrEmpty())
            {
                thumbnail = "false";
            }

            var uploadResult = await _photoService.AddPhotoAsync(file);

            if (uploadResult.Error != null) return BadRequest(uploadResult.Error.Message);

            var photoCreate = new PhotoCreate
            {
                PublicId = uploadResult.PublicId,
                ImageUrl = uploadResult.SecureUrl.AbsoluteUri,
            };

            var photo = await _photoRepository.InsertAsync(photoCreate, userId, propertyId, Convert.ToBoolean(thumbnail));

            return Ok(photo);
        }

        [HttpGet("{photoId}")]
        public async Task<ActionResult<Photo>> Get(int photoId)
        {
            var photo = await _photoRepository.GetAsync(photoId);

            return Ok(photo);
        }

        [HttpGet("property/{propertyId}")]
        public async Task<ActionResult<List<Photo>>> GetByPropertyId(int propertyId)
        {
            var photos = await _photoRepository.GetAllByPropertyIdAsync((propertyId));

            return photos;
        }

        [Authorize]
        [HttpDelete("delete/{photoId}")]

        public async Task<ActionResult<int>> Delete(int photoId)
        {
            int userId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);

            var foundPhoto = await _photoRepository.GetAsync(photoId);

            if (foundPhoto != null)
            {
                if (foundPhoto.UserId == userId)
                {
                    var deleteResult = await _photoService.DeletePhotoAsync(foundPhoto.PublicId);

                    if (deleteResult.Error != null) return BadRequest(deleteResult.Error.Message);

                    var affectedRows = await _photoRepository.DeleteAsync(foundPhoto.PhotoId);

                    return Ok("Deleted "+affectedRows+" photos");
                }
                else
                {
                    return Unauthorized("Photo was not uploaded by the current user.");
                }


            }

            return BadRequest("Photo does not exist");
        }



    }
}
