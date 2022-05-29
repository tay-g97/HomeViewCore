﻿using System;
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

            if (userId != property.UserId)
            {
                return Unauthorized("You do not own this property listing");
            }

            var uploadResult = await _photoService.AddPhotoAsync(file);

            if (uploadResult.Error != null) return BadRequest(uploadResult.Error.Message);

            var photoCreate = new PhotoCreate
            {
                PublicId = uploadResult.PublicId,
                ImageUrl = uploadResult.SecureUrl.AbsoluteUri,
            };

            var photo = await _photoRepository.InsertAsync(photoCreate, userId, propertyId);

            return Ok(photo);
        }

        [HttpGet("{photoId}")]
        public async Task<ActionResult<Photo>> Get(int photoId)
        {
            var photo = await _photoRepository.GetAsync(photoId);

            return Ok(photo);
        }



    }
}
