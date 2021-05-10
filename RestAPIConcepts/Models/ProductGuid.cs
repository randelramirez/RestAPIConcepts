using System;

namespace RestAPIConcepts.Models
{
    public class ProductGuid
    {

        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public Guid SupplierId { get; set; }

        public SupplierGuid Supplier { get; set; }
    }
}
