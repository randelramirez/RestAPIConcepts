using System.Collections.Generic;

namespace RestAPIConcepts.Models
{
    public class SupplierInt
    {
        public SupplierInt()
        {
            this.Products = new List<ProductInt>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }
        public List<ProductInt> Products { get; private set; }
    }
}
