using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public ProductsController(IProductRepository productRepository, IMapper mapper,IPhotoService  photoService ){
            _mapper = mapper;
            _photoService = photoService;
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts([FromQuery]ProductParams productParams){
            var products = await _productRepository.GetProductsAsync(productParams);
            Response.AddPaginationHeader(new PaginationHeader(products.CurrentPage, products.PageSize, products.TotalCount, products.TotalPages));
            return Ok(products);         
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<ProductDto>> GetProduct(string name){ 
            var product = await _productRepository.GetProductByNameAsync(name);
            return _mapper.Map<ProductDto>(product);
        }
        

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetUserId(int id){
            var product = await _productRepository.GetProductByIdAsync(id);
            return _mapper.Map<ProductDto>(product);
        }


        [HttpPost("add-photo/{id}")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file, int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            var result = await _photoService.AddPhotoAsync(file);
            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new ProductPhoto{
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                IsMain = false
            };

             var currentMainPhoto = product.ProductPhotos.FirstOrDefault(p => p.IsMain);
            if (currentMainPhoto != null){
                currentMainPhoto.IsMain = false;
            } else {
                photo.IsMain = true;
            }

            product.ProductPhotos.Add(photo);

            if (await _productRepository.SaveAllAsync())
            {
                return CreatedAtAction(nameof(GetProduct), new { name = product.Name }, _mapper.Map<PhotoDto>(photo));
            }

            return BadRequest("Problem adding photo");
        }


        [HttpPut("set-main-photo/{photoId}/{id}")]
        public async Task<ActionResult> SetMainPhoto(int photoId, int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);

            var photo = product.ProductPhotos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("This is already your main photo");

            var currentMain = product.ProductPhotos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if (await _productRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Problem setting main photo");
        }

        [HttpDelete("delete-photo/{photoId}/{id}")]

        public async Task<ActionResult> DeletePhoto(int photoId, int id){
            var product = await _productRepository.GetProductByIdAsync(id);

            var photo = product.ProductPhotos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("You cannot delete your main photo");

            if (photo.PublicId != null) {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId); 
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            product.ProductPhotos.Remove(photo);
            if(await _productRepository.SaveAllAsync()) return Ok();

            return BadRequest("Faild to delete photo");


        }
    }
}