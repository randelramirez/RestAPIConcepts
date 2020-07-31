using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestAPIConcepts.Models;
using RestAPIConcepts.Services;
using RestAPIConcepts.ViewModels;

namespace RestAPIConcepts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierGuidController : ControllerBase
    {
        private readonly SupplierGuidService service;

        public SupplierGuidController(SupplierGuidService service)
        {
            this.service = service;
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<SupplierGuidViewModel>>> Get([FromQuery] bool includeProducts = true)
        {
            return Ok(await this.service.GetAllAsync(includeProducts));
        }

        [HttpGet("{supplierId}", Name = nameof(SupplierGuidController.GetById))]
        public async Task<ActionResult<SupplierGuidViewModel>> GetById([FromRoute] Guid supplierId, bool includeProducts = true)
        {
            return Ok(await this.service.GetAsync(supplierId,includeProducts));
        }

        [HttpPost()]
        public async Task<ActionResult<SupplierGuidViewModel>> Post(CreateSupplierGuidViewModel model)
        {
            var supplier = new SupplierGuid() { Name = model.Name, Address = model.Address };
            await this.service.AddAsync(supplier);
            return CreatedAtRoute(nameof(SupplierGuidController.GetById), new { supplierId = supplier.Id }, model);
        }

        [HttpPut("{supplierId}")]
        public async Task<ActionResult<SupplierGuidViewModel>> Put([FromRoute] Guid supplierId, UpdateSupplierGuidViewModel model)
        {
            var supplier = new SupplierGuid() {Id = supplierId, Name = model.Name, Address = model.Address };
            await this.service.UpdateAsync(supplier);
            return CreatedAtRoute(nameof(SupplierGuidController.GetById), new { supplierId = supplier.Id }, model);
        }
    }
}
