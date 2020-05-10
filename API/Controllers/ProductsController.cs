using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Helper;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepo<Product> _productsRepo;
        private readonly IGenericRepo<ProductBrand> _productBrandRepo;
        private readonly IGenericRepo<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepo<Product> productsRepo, IGenericRepo<ProductBrand> productBrandRepo,
            IGenericRepo<ProductType> productTypeRepo, IMapper mapper)
        {
            _mapper = mapper;
            _productTypeRepo = productTypeRepo;
            _productBrandRepo = productBrandRepo;
            _productsRepo = productsRepo;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
            [FromQuery]ProductSpecParams productParams)
        {
            var spec = new ProductsWithTypesAndBrandsSpec(productParams);
            var products = await _productsRepo.ListAsync(spec);
            
            var countSpec = new ProductWithFiltersForCountSpec(productParams);
            var totalItems = await _productsRepo.CountAsync(countSpec);

            var data = _mapper.Map<IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex,
                productParams.PageSize, totalItems, data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpec(id);
            var product = await _productsRepo.GetEntityWithSpec(spec);

            if (product == null)
                return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<ProductToReturnDto>(product));
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<List<ProductBrand>>>> GetProductBrands()
        {
            return Ok(await _productBrandRepo.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<List<ProductType>>>> GetProductTypes()
        {
            return Ok(await _productTypeRepo.ListAllAsync());
        }
    }
}