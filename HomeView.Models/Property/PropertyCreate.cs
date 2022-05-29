using System;

namespace HomeView.Models.Property
{
    public class PropertyCreate
    {
        public int PropertyId { get; set; }
        public string Propertyname { get; set; }
        public double Guideprice { get; set; }
        public string Propertytype { get; set; }
        public string Description { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public string Icons { get; set; }
        public string Addressline1 { get; set; }
        public string Addressline2 { get; set; }
        public string Addressline3 { get; set; }
        public string Town { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
    }
}