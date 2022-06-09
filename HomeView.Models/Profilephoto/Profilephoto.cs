using System;
using System.Collections.Generic;
using System.Text;

namespace HomeView.Models.Profilephoto
{
    public class Profilephoto : ProfilephotoCreate
    {
        public int PhotoId { get; set; }
        public int UserId { get; set; }

    }
}
