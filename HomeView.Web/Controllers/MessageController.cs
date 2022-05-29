using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using HomeView.Models.Message;
using HomeView.Models.Property;
using HomeView.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HomeView.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IPhotoRepository _photoRepository;

        public MessageController(IMessageRepository messageRepository, IPhotoRepository photoRepository)
        {
            _messageRepository = messageRepository;
            _photoRepository = photoRepository;
        }

        [Authorize]
        [HttpPost("send/{receiverId}")]
        public async Task<ActionResult<Message>> Create(MessageCreate messageCreate, int receiverId)
        {
            int userId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);

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

        [Authorize]
        [HttpGet("view/all/{userId}")]
        public async Task<ActionResult> GetAllByIdAsync(int userId)
        {
            int currentUserId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);

            if (userId != currentUserId)
            {
                return Unauthorized("You are not authorized to view these messages");
            }

            var messages = await _messageRepository.GetByUserIdAsync(userId);

            if (messages.IsNullOrEmpty())
            {
                return NotFound("You have no messages");
            }

            return Ok(messages);
        }

        [Authorize]
        [HttpDelete("delete/{messageId}")]
        public async Task<ActionResult<int>> Delete(int messageId)
        {
            int userId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);

            var foundMessage = await _messageRepository.GetAsync(messageId);

            if (foundMessage == null)
            {
                return NotFound("Message doesn't exist");
            }

            if (foundMessage.ReceiverId == userId)
            {
                var affectedRows = await _messageRepository.DeleteAsync(messageId);

                return Ok("Deleted " + affectedRows + " Rows");
            }


            return Unauthorized("You did not receive this message");

        }
    }
}
