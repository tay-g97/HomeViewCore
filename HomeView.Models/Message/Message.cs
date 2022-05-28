using System;
using System.Collections.Generic;
using System.Text;

namespace HomeView.Models.Message
{
    public class Message : MessageCreate
    {
        public DateTime Datetimesent { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
    }
}
