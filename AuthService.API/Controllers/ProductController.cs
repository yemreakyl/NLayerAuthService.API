using AuthServer.Core.DTOs.ReturnDtos;
using AuthServer.Core.Models;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthService.API.Controllers
{
    //[Authorize]//Product ile crud işlemleri yapacak olan kullanıcıların yetkili olması gerektiği için authorize attribute ekliyorum
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : CustomBaseController
    {
        private readonly IGenericService<Product, ProductDto> _genericService;

        public ProductController(IGenericService<Product, ProductDto> genericService)
        {
            _genericService = genericService;
        }
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            return ActionResultİnstance(await _genericService.GetAllAsync());
        }
        [HttpPost]
        public async Task<IActionResult> SaveProduct(ProductDto productDto )
        {
            return ActionResultİnstance(await _genericService.AddAsync(productDto));
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(ProductDto productDto)
        {
            return ActionResultİnstance(await _genericService.Update(productDto, productDto.Id));
        }
        //İd parametresini querry string tarafından almak istemiyorum bu şekilde api/product/2 şeklinde çalışacak
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            return ActionResultİnstance(await _genericService.Remove(id));
        }
    }
}
