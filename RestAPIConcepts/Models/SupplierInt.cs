using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
