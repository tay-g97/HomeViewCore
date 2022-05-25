using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HomeView.Models.Property
{
    public class PropertySearch
    {
        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; }
        [Required(ErrorMessage = "PropertyType is required")]
        public string PropertyType { get; set; }
        [Required(ErrorMessage = "Keywords are required")]
        public string Keywords { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public int MinBeds { get; set; }
        public int MaxBeds { get; set; }
    }
}
