using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace Distributed.Cache.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        readonly IDistributedCache _distributedCache;

        public TestController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [HttpGet("Set")]
        public IActionResult Set(string name, string surname)
        {
            //string value qebul edir
            _distributedCache.SetString("name",name, options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(1),
                //SlidingExpiration = TimeSpan.FromSeconds(10)
            });

            //byte value qebul edir
            _distributedCache.Set("surname", Encoding.UTF8.GetBytes(surname), options: new()
            {
                //AbsoluteExpiration = DateTime.Now.AddMinutes(1),
                SlidingExpiration = TimeSpan.FromSeconds(10)
            });

            return Ok();
        }


        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var surname = "";
            var name = await _distributedCache.GetStringAsync("name");
            var surnameByte = await _distributedCache.GetAsync("surname");
            
            if (surnameByte != null)
                surname = Encoding.UTF8.GetString(surnameByte);


            return Ok(new
            {
                name,
                surname
            });
        }
    }
}
