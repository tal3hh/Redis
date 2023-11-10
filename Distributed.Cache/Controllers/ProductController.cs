using Distributed.Cache.Entities;
using Distributed.Cache.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Distributed.Cache.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        readonly IRepository<Product> _repo;

        public ProductController(IRepository<Product> repo)
        {
            _repo = repo;
        }


        [HttpGet("{key}")]
        public async Task<IActionResult> Get(string key)
        {
            var data = await _repo.GetAsync(key);
            return Ok(data);
        }

        [HttpPost("{key}/{minute}")]
        public async Task<IActionResult> Create(string key, int minute, Product product)
        {
            await _repo.CreateAsync(key, product, minute);
            return Ok(product);
        }

        [HttpDelete("{key}")]
        public async Task<IActionResult> Delete(string key)
        {
            await _repo.DeleteAsync(key);
            return Ok();
        }


    }
}
