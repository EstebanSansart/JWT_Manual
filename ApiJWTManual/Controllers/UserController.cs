using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiJWTManual.Dtos.Custom;
using ApiJWTManual.Services;
using System.IdentityModel.Tokens.Jwt;

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

        [HttpPost]
        [Route("ObtenerRefreshToken")]
        public async Task<IActionResult> ObtenerRefreshToken([FromBody] RefreshTokenRequest request){
            var tokenHandler = new JwtSecurityTokenHandler();
            var supposedlyExpiredToken = tokenHandler.ReadJwtToken(request.ExpiredToken);

            if(supposedlyExpiredToken.ValidTo > DateTime.UtcNow)
                return BadRequest(new AuthResponse{Result = false, Msg = "The token has not expired"});

            string userId = supposedlyExpiredToken.Claims.First(x =>
                x.Type == JwtRegisteredClaimNames.NameId).Value.ToString();

            var authResponse = await _authService.ReturnRefreshToken(request, int.Parse(userId));

            if(authResponse.Result)
                return Ok(authResponse);
            else
                return BadRequest(authResponse);
        }
    }
}