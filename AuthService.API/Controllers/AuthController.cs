using AuthServer.Core.DTOs;
using AuthServer.Core.Services;
using AuthService.Core.DTOs.GetDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : CustomBaseController
    {
        //Öncelikle bu authentication controller olduğu için benim service katmanında yer alan authentication service classından bir nesne örneğine ihtiyacım var
        private readonly IAuthenticationService _authenticationService;
        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        [HttpPost]  
        public async Task<IActionResult> CreateToken(LoginDto loginDto)
        {
            var Result=await _authenticationService.CreateTokenAsync(loginDto);
            return ActionResultİnstance(Result);
        }
        [HttpPost]
        public IActionResult CreateTokenForClient(ClientLoginDto clientLoginDto )
        {
            var Result = _authenticationService.CreateClientToken(clientLoginDto);
            return ActionResultİnstance(Result);
        }
        [HttpPost]
        public async Task<IActionResult> RevokeRefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var Result=await _authenticationService.RevokeRefreshToken(refreshTokenDto.Token);
            return ActionResultİnstance(Result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTokenByRefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var Result = await _authenticationService.CreateTokenByRefreshAsync(refreshTokenDto.Token);
            return ActionResultİnstance(Result);
        }
    }
}
