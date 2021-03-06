﻿using System;

namespace RestAPIConcepts.ViewModels
{
    public class ProductGuidViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public Guid SupplierId { get; set; }

    }
}
