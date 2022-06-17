using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace MiniApp2.API.Controllers
{
    [Authorize] //kullanıcıların yetkili olması gerektiği için authorize attribute ekliyorum
     [Route("api/[controller]")]
    [ApiController]
   
    public class InvoiceController : ControllerBase
    {
        //Burda da test amaçlı olarak veri tabanına gitmeyen ancak fatura bilgileri için istek yapan kullanıcıların name ve ıd bilgilerini dönmek istiyorum
        [HttpGet]
        public IActionResult GetInvoices()
        {
             var UserName=HttpContext.User.Identity.Name;
            var UserIdClaimKey = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            return Ok($"Fatura Bilgileri UserName:{ UserName} -- UserId:{UserIdClaimKey.Value}");
        }
    }
}
