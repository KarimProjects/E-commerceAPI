using AutoMapper;
using Store.Data.Entities;
using Store.Repository.Interfaces;
using Store.Repository.Specification.Product;
using Store.Service.Helper;
using Store.Service.Services.ProductService.Dtos;

namespace Store.Service.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IReadOnlyList<BrandTypeDetailsDto>> GetAllBrandsAsync()
        {
            var brands = await _unitOfWork.Repository<ProductBrand, int>().GetAllAsync();
            var mapperBrands = _mapper.Map<IReadOnlyList<BrandTypeDetailsDto>>(brands);
            return mapperBrands;
        }

        public async Task<PaginatedResultDto<ProductDetailsDto>> GetAllProductsAsync(ProductSpecification input)
        {
            var specs = new ProductsWithSpecifications(input);
            var products = await _unitOfWork.Repository<Product, int>().GetAllWithSpecificationAsync(specs);
            var countSpecs = new ProductsWithFilterAndCountSpecifications(input);
            var count = await _unitOfWork.Repository<Product, int>().CountSpecificationAsync(countSpecs);
            var mapperProducts = _mapper.Map<IReadOnlyList<ProductDetailsDto>>(products);
            return new PaginatedResultDto<ProductDetailsDto>(input.PageIndex, input.PageSize, count, mapperProducts);
        }

        public async Task<IReadOnlyList<BrandTypeDetailsDto>> GetAllTypesAsync()
        {
            var types = await _unitOfWork.Repository<ProductType, int>().GetAllAsync();
            var mapperTypes = _mapper.Map<IReadOnlyList<BrandTypeDetailsDto>>(types);
            return mapperTypes;
        }

        public async Task<ProductDetailsDto> GetProductByIdAsync(int? id)
        {
            if(id is null)
                throw new Exception("Id is Null");
            var specs = new ProductsWithSpecifications(id);
            var product = await _unitOfWork.Repository<Product, int>().GetWithSpecificationByIdAsync(specs);
            if (product is null)
                throw new Exception("Product Not Found");
            var mapperProduct = _mapper.Map<ProductDetailsDto>(product);
            return mapperProduct;
        }
    }
}
