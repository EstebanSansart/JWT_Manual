using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiJWTManual.Dtos.Custom;
using ApiJWTManual.Services;

namespace ApiJWTManual.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;

        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("Autenticar")]
        
        public async Task<IActionResult> Autenticar([FromBody] AuthRequest auth){
            var AuthResult = await _authService.ReturnToken(auth);

            if(AuthResult == null)
                return Unauthorized();

            return Ok(AuthResult);
        }
    }
}