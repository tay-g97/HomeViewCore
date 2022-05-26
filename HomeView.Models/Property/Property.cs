using System;
using System.Collections.Generic;
using System.Text;

namespace HomeView.Models.Property
{
    public class Property : PropertyCreate
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
