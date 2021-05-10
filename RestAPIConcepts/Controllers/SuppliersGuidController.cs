using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestAPIConcepts.Models;
using RestAPIConcepts.Services;
using RestAPIConcepts.ViewModels;

namespace RestAPIConcepts.Controllers
{
    [Route("api/suppliersguid")]
    [ApiController]
    public class SuppliersGuidController : ControllerBase
    {
        private readonly SuppliersGuidService service;

        public SuppliersGuidController(SuppliersGuidService service)
        {
            this.service = service;
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<SupplierGuidViewModel>>> GetSuppliers(/*Lets use name as the query key instead of nameFilter*/[FromQuery(Name ="name")] string nameFilter,[FromQuery] bool includeProducts = true)
        {
            return Ok(await this.service.GetAllAsync(nameFilter, includeProducts));
        }

        //[HttpGet("{productId}", Name = "[controller]." + nameof(SuppliersGuidController.GetSupplierById))]
        [HttpGet("{supplierId}", Name = nameof(SuppliersGuidController.GetSupplierById))]
        public async Task<ActionResult<SupplierGuidViewModel>> GetSupplierById([FromRoute] Guid supplierId, bool includeProducts = true)
        {
            if(!await this.service.IsExistingAsync(supplierId))
            {
                return NotFound();
            }

            return Ok(await this.service.GetAsync(supplierId, includeProducts));
        }

        [HttpGet("({supplierIds})", Name = nameof(SuppliersGuidController.GetSupplierByIds))]
        public async Task<ActionResult<SupplierGuidViewModel>> GetSupplierByIds(
            [FromRoute][ModelBinder(BinderType = typeof(ModelBinders.ArrayModelBinder))] IEnumerable<Guid> supplierIds, 
            [FromQuery] bool includeProducts = true)
        {
            
            return Ok(await this.service.GetAsync(supplierIds,includeProducts));
        }

        [HttpPost()]
        public async Task<ActionResult<SupplierGuidViewModel>> CreateSupplier(CreateSupplierGuidViewModel model)
        {
            var newSupplier = new SupplierGuid() { Name = model.Name, Address = model.Address };
            await this.service.AddAsync(newSupplier);
            return CreatedAtRoute(nameof(SuppliersGuidController.GetSupplierById), new { supplierId = newSupplier.Id }, 
                new SupplierGuidViewModel() 
                { 
                    Id = newSupplier.Id, 
                    Name = newSupplier.Name, 
                    Address = newSupplier.Address 
                });
        }

        [HttpPut("{supplierId}")]
        public async Task<ActionResult<SupplierGuidViewModel>> UpdateOrInsertSupplier([FromRoute] Guid supplierId, 
            UpdateSupplierGuidViewModel model)
        {
            // If we do not want to Upsert functionality then return NotFound
            //if (!await this.service.IsExistingAsync(supplierId))
            //{
            //    return NotFound();
            //}

            if (!await this.service.IsExistingAsync(supplierId))
            {
                // Create
                var newSupplier = new SupplierGuid()
                {
                    Id = supplierId,
                    Address = model.Address,
                    Name = model.Name
                };
                
                await this.service.AddAsync(newSupplier);
                return CreatedAtRoute(nameof(SuppliersGuidController.GetSupplierById), new { supplierId = newSupplier.Id }, 
                    new SupplierGuidViewModel()
                    {
                        Id = newSupplier.Id,
                        Name = newSupplier.Name,
                        Address = newSupplier.Address
                    });
            }
            else
            {
                // Update
                var supplier = new SupplierGuid() { Id = supplierId, Name = model.Name, Address = model.Address };
                await this.service.UpdateAsync(supplier);
                return NoContent();
            }
        }

        [HttpPatch("{supplierId}")]
        public async Task<ActionResult<SupplierGuidViewModel>> PatchOrInsertSupplier([FromRoute] Guid supplierId, 
            JsonPatchDocument<UpdateSupplierGuidViewModel> patchDocument)
        {
            // If we do not want to Upsert functionality then return NotFound
            //if (!await this.service.IsExistingAsync(supplierId))
            //{
            //    return NotFound();
            //}

            if (!await this.service.IsExistingAsync(supplierId))
            {
                // insert
                var updateViewModel = new UpdateSupplierGuidViewModel();
                patchDocument.ApplyTo(updateViewModel, ModelState);

                if (!TryValidateModel(updateViewModel))
                {
                    return ValidationProblem(ModelState);
                }

                var newSupplier = new SupplierGuid() 
                { 
                    Id = supplierId, 
                    Name = updateViewModel.Name, 
                    Address = updateViewModel.Address 
                };
                await this.service.AddAsync(newSupplier);

                var viewModel = new SupplierGuidViewModel() 
                { 
                    Id = newSupplier.Id, 
                    Name = newSupplier.Name, 
                    Address = newSupplier.Address 
                };

                return CreatedAtRoute(nameof(SuppliersGuidController.GetSupplierById), 
                    new { supplierId = viewModel.Id }, viewModel);
            }
            else
            {
                var supplier = await this.service.GetAsync(supplierId);
                var updateViewModel = new UpdateSupplierGuidViewModel()
                {
                    Name = supplier.Name,
                    Address = supplier.Address
                };

                patchDocument.ApplyTo(updateViewModel, ModelState);

                if (!TryValidateModel(updateViewModel))
                {
                    return ValidationProblem(ModelState);
                }

                var updatedSupplier = new SupplierGuid()
                {
                    Id = supplierId,
                    Name = updateViewModel.Name,
                    Address = updateViewModel.Address
                };

                await this.service.UpdateAsync(updatedSupplier);

                return NoContent();
            }

        }

        [HttpDelete("{supplierId}", Name = nameof(SuppliersGuidController.DeleteSupplier))]
        public async Task<IActionResult> DeleteSupplier(Guid supplierId)
        {
            //check if supplier exists
            if(!await this.service.IsExistingAsync(supplierId))
            {
                return NotFound();
            }

            await this.service.DeleteAsync(supplierId);
            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetSupplierOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST,DELETE,PUT,PATCH,DELETE");
            return Ok();
        }
    }
}
