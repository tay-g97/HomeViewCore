﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HomeView.Models.Message;

namespace HomeView.Repository
{
    public interface IMessageRepository
    {
        public Task<Message> GetAsync(int messageId);
        public Task<Message> InsertAsync(MessageCreate messageCreate, int userId, int receiverId);
    }
}
