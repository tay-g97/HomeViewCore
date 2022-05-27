﻿using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using HomeView.Models.Account;
using HomeView.Models.Message;
using HomeView.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeView.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly UserManager<UserIdentity> _userManager;

        public MessageController(IMessageRepository messageRepository, IPhotoRepository photoRepository, UserManager<UserIdentity> userManager)
        {
            _messageRepository = messageRepository;
            _photoRepository = photoRepository;
            _userManager = userManager;
        }

        [Authorize]
        [HttpPost("{receiverId}")]
        public async Task<ActionResult<Message>> Create(MessageCreate messageCreate, int receiverId)
        {
            int userId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);
            var userIdentity = await _userManager.FindByIdAsync(userId.ToString());
            var message = await _messageRepository.InsertAsync(messageCreate, userId, receiverId);

            return Ok(message);
        }

        [Authorize]
        [HttpGet("view/{messageId}")]
        public async Task<ActionResult<Message>> Get(int messageId)
        {
            int userId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);
            var message = await _messageRepository.GetAsync(messageId);

            if (message.ReceiverId == userId | message.SenderId == userId)
            {
                return Ok(message);
            }

            return Unauthorized("You are not authorized to view this message");
        }
    }
}
