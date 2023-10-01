using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiJWTManual.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CountryController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista(){
            var countryList = await Task.FromResult(new List<string>{"Francia","Argentina","Croacia","Marruecos"});
            return Ok(countryList); 
        }
    }
}