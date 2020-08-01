﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult<IEnumerable<SupplierGuidViewModel>>> GetSuppliers([FromQuery] bool includeProducts = true)
        {
            return Ok(await this.service.GetAllAsync(includeProducts));
        }

        //[HttpGet("{productId}", Name = "[controller]." + nameof(SuppliersGuidController.GetSupplierById))]
        [HttpGet("{supplierId}", Name = nameof(SuppliersGuidController.GetSupplierById))]
        public async Task<ActionResult<SupplierGuidViewModel>> GetSupplierById([FromRoute] Guid supplierId, bool includeProducts = true)
        {
            return Ok(await this.service.GetAsync(supplierId, includeProducts));
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
    }
}