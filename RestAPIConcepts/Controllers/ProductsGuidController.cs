using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestAPIConcepts.Models;
using RestAPIConcepts.Services;
using RestAPIConcepts.ViewModels;

namespace RestAPIConcepts.Controllers
{
    [Route("api/suppliersguid/{supplierId}/productsguid")]
    [ApiController]
    public class ProductsGuidController : Controller
    {
        private readonly SuppliersGuidService supplierService;
        private readonly ProductsGuidService productService;

        /*
            If we have validation on constructos, it's better to write it in a constrcutor with a body {} 
         */
        public ProductsGuidController(SuppliersGuidService supplierService, ProductsGuidService productService) =>
            (this.supplierService, this.productService) = (supplierService ?? throw new ArgumentNullException(nameof(supplierService)),
                productService ?? throw new ArgumentNullException(nameof(productService)));
        //{
        //    this.supplierService = supplierService ?? throw new ArgumentNullException(nameof(supplierService));
        //    this.productService = productService ?? throw new ArgumentNullException(nameof(productService));
        //}

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<ProductGuidViewModel>>> GetSupplierProducts(Guid supplierId)
        {
            if (!await this.supplierService.IsExistingAsync(supplierId))
            {
                return NotFound();
            }
            else
            {
                return Ok(await this.productService.GetAllBySupplierIdAsync(supplierId));
            }
        }

        //[HttpGet("{productId}", Name = "[controller]." + nameof(ProductsGuidController.GetSupplierProductsById))]
        [HttpGet("{productId}", Name = nameof(ProductsGuidController.GetSupplierProductsById))]
        public async Task<ActionResult<ProductGuidViewModel>> GetSupplierProductsById(Guid supplierId, Guid productId)
        {
            if (!await this.supplierService.IsExistingAsync(supplierId))
            {
                return NotFound();
            }
            else
            {
                if (!await this.productService.IsExistingAsync(productId))
                {
                    return NotFound();
                }
                else
                {
                    return Ok(await this.productService.GetAsync(productId));
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProductGuidViewModel>> CreateSupplierProduct(Guid supplierId, CreateProductGuidViewModel model)
        {
            var newProduct = new ProductGuid()
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                SupplierId = supplierId
            };

            await this.productService.AddAsync(newProduct);

            return CreatedAtRoute(nameof(ProductsGuidController.GetSupplierProductsById), new { supplierId = supplierId, productId = newProduct.Id },
                new ProductGuidViewModel()
                {
                    Id = newProduct.Id,
                    Name = newProduct.Name,
                    Description = newProduct.Description,
                    Price = newProduct.Price
                });
        }

        [HttpPut("{productId}")]
        public async Task<ActionResult<ProductGuidViewModel>> UpdateOrInsertSupplierProduct(Guid supplierId, Guid productId,
            UpdateProductGuidViewModel model)
        {
            if (!await this.supplierService.IsExistingAsync(supplierId))
            {
                return NotFound();
            }
            else
            {
                if (!await this.productService.IsExistingAsync(productId))
                {
                    var newProduct = new ProductGuid()
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Price = model.Price,
                        SupplierId = supplierId
                    };

                    await this.productService.AddAsync(newProduct);

                    return CreatedAtRoute(nameof(ProductsGuidController.GetSupplierProductsById), new { supplierId = supplierId, productId = newProduct.Id },
                        new ProductGuidViewModel()
                        {
                            Id = newProduct.Id,
                            Name = newProduct.Name,
                            Description = newProduct.Description,
                            Price = newProduct.Price
                        });
                }
                else
                {
                    var updatedProduct = new ProductGuid()
                    {
                        Id = productId,
                        Name = model.Name,
                        Description = model.Description,
                        Price = model.Price,
                        SupplierId = supplierId
                    };

                    await this.productService.UpdateAsync(updatedProduct);
                    return NoContent();
                }
            }
        }

        [HttpPatch("{productId}")]
        public async Task<ActionResult<ProductGuidViewModel>> PatchOrInsertSupplierProduct(Guid supplierId, Guid productId,
            JsonPatchDocument<UpdateProductGuidViewModel> patchDocument)
        {
            // If we do not want to Upsert functionality then return NotFound
            //if (!await this.service.IsExistingAsync(supplierId))
            //{
            //    return NotFound();
            //}

            if (!await this.supplierService.IsExistingAsync(supplierId))
            {
                return NotFound();
            }
            else
            {
                if (!await this.productService.IsExistingAsync(productId))
                {
                    // insert
                    var updateViewModel = new UpdateProductGuidViewModel();
                    patchDocument.ApplyTo(updateViewModel, ModelState);

                    if (!TryValidateModel(updateViewModel))
                    {
                        return ValidationProblem(ModelState);
                    }

                    var newProduct = new ProductGuid()
                    {
                        Id = productId,
                        Name = updateViewModel.Name,
                        Description = updateViewModel.Description,
                        Price = updateViewModel.Price,
                        SupplierId = supplierId
                    };
                    await this.productService.AddAsync(newProduct);

                    var viewModel = new ProductGuidViewModel()
                    {
                        Id = newProduct.Id,
                        Name = newProduct.Name,
                        Description = newProduct.Description,
                        Price = newProduct.Price,
                        SupplierId = supplierId
                    };

                    return CreatedAtRoute(nameof(ProductsGuidController.GetSupplierProductsById),
                        new { supplierId = viewModel.SupplierId, productId = viewModel.Id }, viewModel);
                }
                else
                {
                    var product = await this.productService.GetAsync(productId);
                    var updatedViewModel = new UpdateProductGuidViewModel()
                    {
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                    };

                    patchDocument.ApplyTo(updatedViewModel, ModelState);

                    if (!TryValidateModel(updatedViewModel))
                    {
                        return ValidationProblem(ModelState);
                    }

                    var updatedProduct = new ProductGuid()
                    {
                        Id = productId,
                        Name = updatedViewModel.Name,
                        Description = updatedViewModel.Description,
                        Price = updatedViewModel.Price,
                        SupplierId = supplierId

                    };

                    await this.productService.UpdateAsync(updatedProduct);

                    return NoContent();
                }
            }
        }

        [HttpDelete("{productId}")] 
        public async Task<IActionResult> DeleteProduct(Guid supplierId, Guid productId)
        {
            if (!await this.supplierService.IsExistingAsync(supplierId))
            {
                return NotFound();
            }
            if(!await this.productService.IsExistingAsync(productId))
            {
                return NotFound();
            }

            await this.productService.DeleteAsync(productId);
            return NoContent();
        }
    }
}
