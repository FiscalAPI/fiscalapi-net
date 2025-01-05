using FiscalApi.Common;

namespace FiscalApi.Models
{
    public class Product : BaseDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
    }
}