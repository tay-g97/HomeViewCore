using System;
using System.Collections.Generic;
using System.Text;

namespace HomeView.Models.Photo
{
    public class Photo : PhotoCreate
    {
        public int PhotoId { get; set; }
        public int UserId { get; set; }
        public bool Thumbnail { get; set; }
        public int PropertyId { get; set; }
    }
}
