using System;
using System.Collections.Generic;
using System.Text;

namespace HomeView.Models.Message
{
    public class MessageCreate
    {
        public int MessageId { get; set; }
        public string Message { get; set; }
        public string Messagetype { get; set; }
        public bool Reply { get; set; }
        public int RepliedId { get; set; }
        public int PropertyId { get; set; }

    }
}
