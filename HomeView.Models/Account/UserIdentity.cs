using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace HomeView.Models.Account
{
    public class UserIdentity
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string NormalizedUsername { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Dateofbirth { get; set; }
        public string PasswordHash { get; set; }
        public string Addressline1 { get; set; }
        public string Addressline2 { get; set; }
        public string Addressline3 { get; set; }
        public string Town { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string Accounttype { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public string Phone { get; set; }
        public bool MarketingEmail { get; set; }
        public bool MarketingPhone { get; set; }
        public int? ProfilepictureId { get; set; }
    }
}
