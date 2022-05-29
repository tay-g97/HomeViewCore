using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using HomeView.Models.Message;
using Microsoft.Extensions.Configuration;

namespace HomeView.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IConfiguration _config;

        public MessageRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<int> DeleteAsync(int messageId)
        {
            int affectedRows = 0;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                affectedRows = await connection.ExecuteAsync(
                    "Message_Delete",
                    new { MessageId = messageId },
                    commandType: CommandType.StoredProcedure);
            }

            return affectedRows;
        }

        public async Task<Message> GetAsync(int messageId)
        {
            Message message;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                message = await connection.QueryFirstOrDefaultAsync<Message>(
                    "Message_Get",
                    new { MessageId = messageId },
                    commandType: CommandType.StoredProcedure);
            }

            return message;
        }

        public async Task<List<Message>> GetByUserIdAsync(int userId)
        {
            IEnumerable<Message> messages;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                messages = await connection.QueryAsync<Message>(
                    "Message_GetAllById",
                    new { UserId = userId },
                    commandType: CommandType.StoredProcedure);
            }

            return messages.ToList();
        }

        public async Task<Message> InsertAsync(MessageCreate messageCreate, int userId, int receiverId)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("MessageId", typeof(int));
            dataTable.Columns.Add("Message", typeof(string));
            dataTable.Columns.Add("Messagetype", typeof(string));
            dataTable.Columns.Add("Reply", typeof(bool));
            dataTable.Columns.Add("RepliedId", typeof(int));
            dataTable.Columns.Add("PropertyId", typeof(int));

            dataTable.Rows.Add(
                messageCreate.MessageId,
                messageCreate.Message,
                messageCreate.Messagetype,
                messageCreate.Reply,
                messageCreate.RepliedId,
                messageCreate.PropertyId
            );

            int newMessageId;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                newMessageId = await connection.ExecuteScalarAsync<int>(
                    "Message_Insert",
                    new { Message = dataTable.AsTableValuedParameter("dbo.MessageType"), UserId = userId, ReceiverId = receiverId },
                    commandType: CommandType.StoredProcedure
                );
            }

            Message message = await GetAsync(newMessageId);

            return message;
        }
    }
}
