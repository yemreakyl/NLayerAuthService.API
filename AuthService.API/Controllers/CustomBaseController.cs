using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Dtos;

namespace AuthService.API.Controllers
{
    public class CustomBaseController : ControllerBase
    {
        //Bu controllerın amacı gelen status code a göre otomatik olarak objectResul olarak cevap dönerken kullanmak
        public IActionResult ActionResultİnstance<T>(Response<T> response) where T:class
        {
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode,
            };
        }
    }
}
