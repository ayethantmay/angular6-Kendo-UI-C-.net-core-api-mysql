using System;

namespace dotnet2_1WebAPI
{
     [System.ComponentModel.DataAnnotations.Schema.Table("inventory")]
    public class Inventory
    {
       public int inventory_id { get; set; }
        public string inventory_category { get; set; }
        public string inventory_name { get; set; }
        public string inventory_description { get; set; }
        public decimal inventory_price { get; set; }
        public string inventory_brand { get; set; }
    }
}
