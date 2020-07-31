using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPIConcepts.ViewModels
{
    public class SupplierGuidViewModel
    {
        public SupplierGuidViewModel()
        {
            this.Products = new List<ProductGuidViewModel>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public ICollection<ProductGuidViewModel> Products { get; set; }
    }
}
