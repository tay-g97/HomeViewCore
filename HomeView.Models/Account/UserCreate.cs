using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HomeView.Models.Account
{
    public class UserCreate : UserLogin
    {
        [Required(ErrorMessage = "First name is required")]
        [MinLength(1, ErrorMessage = "Must be 1-15 characters")]
        [MaxLength(15, ErrorMessage = "Must be 1-15 characters")]
        public string Firstname { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        [MinLength(1, ErrorMessage = "Must be 1-15 characters")]
        [MaxLength(15, ErrorMessage = "Must be 1-15 characters")]
        public string Lastname { get; set; }
        [Required(ErrorMessage = "Date of Birth is required")]
        [MinLength(10, ErrorMessage = "Must be 10 characters")]
        [MaxLength(10, ErrorMessage = "Must be 10 characters")]
        public string Dateofbirth { get; set; }
        [Required(ErrorMessage = "Address line 1 is required")]
        [MinLength(1, ErrorMessage = "Must be 1-30 characters")]
        [MaxLength(30, ErrorMessage = "Must be 1-30 characters")]
        public string Addressline1 { get; set; }
        [MinLength(1, ErrorMessage = "Must be 1-30 characters")]
        [MaxLength(30, ErrorMessage = "Must be 1-30 characters")]
        public string Addressline2 { get; set; }
        [MinLength(1, ErrorMessage = "Must be 1-30 characters")]
        [MaxLength(30, ErrorMessage = "Must be 1-30 characters")]
        public string Addressline3 { get; set; }
        [MinLength(1, ErrorMessage = "Must be 1-20 characters")]
        [MaxLength(20, ErrorMessage = "Must be 1-20 characters")]
        public string Town { get; set; }
        [MinLength(1, ErrorMessage = "Must be 1-20 characters")]
        [MaxLength(20, ErrorMessage = "Must be 1-20 characters")]
        public string City { get; set; }
        [Required(ErrorMessage = "Post Code is required")]
        [MinLength(1, ErrorMessage = "Must be 1-10 characters")]
        [MaxLength(10, ErrorMessage = "Must be 1-10 characters")]
        public string Postcode { get; set; }
        [Required(ErrorMessage = "Account Type is required")]
        [MinLength(5, ErrorMessage = "Must be 5-6 characters")]
        [MaxLength(6, ErrorMessage = "Must be 5-6 characters")]
        public string Accounttype { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [MinLength(5, ErrorMessage = "Must be 5-30 characters")]
        [MaxLength(30, ErrorMessage = "Must be 5-30 characters")]
        public string Email { get; set; }
        [MinLength(6, ErrorMessage = "Must be 6-15 characters")]
        [MaxLength(15, ErrorMessage = "Must be 6-15 characters")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Email preferences are required")]
        public bool MarketingEmail { get; set; }
        [Required(ErrorMessage = "Phone preferences are required")]
        public bool MarketingPhone { get; set; }
        public string ProfilepictureUrl { get; set; }
    }
}
