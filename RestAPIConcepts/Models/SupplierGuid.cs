using System;
using System.Collections.Generic;

namespace RestAPIConcepts.Models
{
    public class SupplierGuid
    {
        public SupplierGuid()
        {
            this.Products = new List<ProductGuid>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public List<ProductGuid> Products { get; private set; }
    }
}
