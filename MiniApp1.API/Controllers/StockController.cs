using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace MiniApp1.API.Controllers
{
    [Authorize]//kullanıcıların yetkili olması gerektiği için authorize attribute ekliyorum
    [Route("api/[controller]")]
    [ApiController]
    
    public class StockController : ControllerBase
    {
        //Mini apı lerimde veri tabanına gitmeyen basit controllerlar yazazağım amacım sadece bu mini apı'lerimin token bazlı kimlik doğrulama ile korunup korunmadığını test etmek

        //Bu kontroller stock bilgisi için istek yapan kullanıcının name ve Id bilgilerini döneceğim
        [HttpGet]
        public IActionResult GetStock()
        {
            var UserName = HttpContext.User.Identity.Name;
            var UserIdClaimKey = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            return Ok($"Stock İşlemleri UserName:{ UserName} -- UserId:{UserIdClaimKey.Value}");
        }
    }
}
