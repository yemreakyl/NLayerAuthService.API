using System;

namespace AuthServer.Core.Models
{
    public class UserRefreshToken
    {
        //Refresh token verdiğim kullanıcıların bilgilerini veri tabanında tutmak amacıyla oluşturduğum model
        public string UserId { get; set; }
        public DateTime Expiration { get; set; }//Kullanım süresi
        public string Code { get; set; }//refresh kodu
    }
}
