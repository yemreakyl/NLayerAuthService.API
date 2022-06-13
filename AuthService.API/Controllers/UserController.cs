using AuthServer.Core.DTOs.GetDtos;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : CustomBaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            var Result=await _userService.CreateUserAsync(createUserDto);
            return ActionResultİnstance(Result);
        }
        [Authorize]//Bu endpoint'i authorize attribute ile işaretliyorum bu da bu endpoint'in mutlaka bir token istediği anlamına geliyor
        [HttpGet]
        public async Task<IActionResult> GetUserByNameAsync()
        {
            //Kullanıcı adını istemek yerine istek yapan kullanıcının tokenı içerisinden kendim alıyorum
           var Result= await _userService.GetUserByNameAsync(HttpContext.User.Identity.Name);
            return ActionResultİnstance(Result);
        }
    }
}
