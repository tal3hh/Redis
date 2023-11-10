using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryCache.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private IMemoryCache _memeroyCache;

        public ValuesController(IMemoryCache memeroyCache)
        {
            _memeroyCache = memeroyCache;
        }

        [HttpGet("SetName/{name}")]
        public void Set(string name)
        {
            _memeroyCache.Set("name", name);
        }


        [HttpGet("GetName")]
        public IActionResult Get()
        {
            //Ramda 'name' keyine qarsiliq, bir valuenin olub/olmadigini yoxluyur. out'la yuxaridaki deyeri alir.
            if (_memeroyCache.TryGetValue<string>("name", out string name))
            {
                return Ok(_memeroyCache.Get<string>("name"));
            }

            return NotFound();
        }


        [HttpGet("SetDate/{animal}")]
        public void SetDate(string animal)
        {
            _memeroyCache.Set<string>("animal", animal, options: new()
            {
                // 1'dk sonra ramdan silinecek.
                AbsoluteExpiration = DateTime.Now.AddMinutes(1),

                // 10 saniyede bir GET olunmasa silinecek.(yeni isledilmese)
                SlidingExpiration = TimeSpan.FromSeconds(10)
            });
        }

        [HttpGet("GetDate")]
        public IActionResult GetDate()
        {
            if (_memeroyCache.TryGetValue<string>("animal", out string animal))
            {
                return Ok(_memeroyCache.Get<string>("animal"));
            }

            return NotFound();
        }
    }
}
