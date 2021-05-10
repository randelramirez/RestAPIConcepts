namespace RestAPIConcepts.ViewModels
{
    public class CreateProductGuidViewModel
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        // SupplierId is taken from the route
        //public Guid SupplierId { get; set; }
    }
}
