using System;
using System.Collections.Generic;
using System.Text;

namespace HomeView.Models.Property
{
    public class PropertySearch
    {
        public string Location { get; set; }
        public string PropertyType { get; set; }
        public string Keywords { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public int MinBeds { get; set; }
        public int MaxBeds { get; set; }
    }
}
