using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace HomeView.Models.Account
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Dateofbirth { get; set; }
        public string Addressline1 { get; set; }
        public string Addressline2 { get; set; }
        public string Addressline3 { get; set; }
        public string Town { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string Accounttype { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool MarketingEmail { get; set; }
        public bool MarketingPhone { get; set; }
        public string ProfilepictureUrl { get; set; }
        public string Token { get; set; }
    }
}
