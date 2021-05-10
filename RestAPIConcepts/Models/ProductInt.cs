namespace RestAPIConcepts.Models
{
    public class ProductInt
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public int SupplierId { get; set; }

        public SupplierInt Supplier { get; set; }
    }
}
