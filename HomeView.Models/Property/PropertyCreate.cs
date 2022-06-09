using System;
using System.ComponentModel.DataAnnotations;

namespace HomeView.Models.Property
{
    public class PropertyCreate
    {
        public int PropertyId { get; set; }
        [Required(ErrorMessage = "Property name is required")]
        [MaxLength(50, ErrorMessage = "Must be 50 characters or less")]
        public string Propertyname { get; set; }
        [Required(ErrorMessage = "Guide price is required")]
        public double Guideprice { get; set; }
        [Required(ErrorMessage = "Property type is required")]
        public string Propertytype { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Number of bedrooms is required")]
        public int Bedrooms { get; set; }
        [Required(ErrorMessage = "Number of bathrooms is required")]
        public int Bathrooms { get; set; }
        public string Icons { get; set; }
        [Required(ErrorMessage = "Address line 1 is required")]
        public string Addressline1 { get; set; }
        public string Addressline2 { get; set; }
        public string Addressline3 { get; set; }
        public string Town { get; set; }
        public string City { get; set; }
        [Required(ErrorMessage = "Postcode is required")]
        public string Postcode { get; set; }
    }
}