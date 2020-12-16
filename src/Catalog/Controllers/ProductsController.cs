using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Application.Products.Commands;
using Catalog.Application.Products.Queries;
using Catalog.Controllers.dto;
using Catalog.Domain.Products;
using Catalog.Domain.Products.PriceInfo;
using MediatR;

namespace Catalog.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="mapper"></param>
        public ProductsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Get the list of products
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult>  GetProducts()
        {
            var request = new GetProductsHandler.GetProducts();
            var response = await _mediator.Send(request, CancellationToken.None);
            var result =  _mapper.Map<List<ProductDto>>(response);
            return new OkObjectResult(result);
        }
        /// <summary>
        /// Returns a single product by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var request = new GetProductByIdHandler.GetProductByIdQuery(id);
            var response = await _mediator.Send(request, CancellationToken.None);
            if (response == null) return NotFound();
            var result = _mapper.Map<ProductDto>(response);
            return new OkObjectResult(result);
        }
        /// <summary>
        /// Adds a new product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductDto product)
        {
            var request = new AddProduct.AddProductRequest { Product = _mapper.Map<Product>(product) };
            var result = await _mediator.Send(request, CancellationToken.None);
            return CreatedAtAction(nameof(GetProductById), "Products", new { id = result}, result);
        }
        /// <summary>
        /// Updates product details except external information like PriceInfo and Stockinfo
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateProduct(Guid id, ProductDto product)
        {
            var request = new UpdateProduct.UpdateProductCommand(id) { Product = _mapper.Map<Product>(product)};
            await _mediator.Send(request, CancellationToken.None);
            return Accepted();
        }

        /// <summary>
        /// Although this method directly overrides the stock in the catalog, this should be updated from the update from the warehouse
        /// </summary>
        /// <param name="id"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        [HttpPost("{id:Guid}/stock")]
        public async Task<IActionResult> UpdateStock([FromRoute] Guid id, [FromBody] StockInfoDto amount)
        {
            var request = new UpdateProductAvailability.UpdateProductAvailabilityCommand(id, amount.CurrentAmountAvailable);
            await _mediator.Send(request, CancellationToken.None);
            return Accepted();
        }

        /// <summary>
        ///  Although this method directly overrides the price in the catalog, this should be updated from the update from the pricing
        /// </summary>
        /// <param name="id"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        [HttpPost("{id:Guid}/price")]
        public async Task<IActionResult> UpdatePrice([FromRoute]Guid id, [FromBody] PriceDto price)
        {
            var request = new UpdateProductPrice.UpdateProductPriceCommand(id, _mapper.Map<Price>(price));
            await _mediator.Send(request, CancellationToken.None);
            return Accepted();
        }

        /// <summary>
        /// HardDeletes a product (this should be replaced by a soft-delete later)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var request = new DeleteProduct.DeleteProductRequest(id);
            await _mediator.Send(request, CancellationToken.None);
            return Accepted();
        }
    }
}
