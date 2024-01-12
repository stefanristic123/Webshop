using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using API.Helpers;
using AutoMapper.QueryableExtensions;
using API.DTOs;

namespace API.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(DataContext context,IMapper mapper){
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedList<ProductDto>> GetProductsAsync(ProductParams productParams){
            var query = _context.Products.AsQueryable();

            var minDob = productParams.MinPrice;
            var maxDob = productParams.MaxPrice;

            query = query.Where(u => u.Price >= minDob && u.Price <= maxDob);

            if (!string.IsNullOrEmpty(productParams.SearchTerm)){
                query = query.Where(p => p.Name.Contains(productParams.SearchTerm));
            }

            return await PagedList<ProductDto>.CreateAsync(query.ProjectTo<ProductDto>(_mapper.ConfigurationProvider).AsTracking(), 
                productParams.PageNumber, productParams.PageSize);
        }

        public async Task<Product> GetProductByNameAsync(string name)
        {
            var lowerCaseName = name.ToLower();
            return await _context.Products
                .Include(p => p.ProductPhotos) 
                .SingleOrDefaultAsync(x => x.Name == lowerCaseName);
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.ProductPhotos) 
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
        }
    }
}