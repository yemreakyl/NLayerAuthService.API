using Microsoft.AspNetCore.Identity;

namespace AuthServer.Core.Models
{
    public class UserApp:IdentityUser
    {
        //İstek yapan kullanıcıların verilerinin kontrolü için oluşturduğum model
        public string City { get; set; }

    }
}
