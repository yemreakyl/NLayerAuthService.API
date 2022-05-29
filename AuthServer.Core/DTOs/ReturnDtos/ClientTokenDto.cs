using System;

namespace AuthServer.Core.DTOs
{
    public class ClientTokenDto
    {
        //Bu dto nun amacı bazı apı lerimin kimlik doğrulama ile işi olmayabilir örneğin hava durumu yada borsa bilgilerini veren bir apı olabilir.Ancak ben yinede bilmediğim client ların apı' mi kullanmasını istemediğim için onlara da kimlik doğrulaması olmayan bir token veriyorum ve bu tokenın refresh token ı olmayacağı için ayrı bir model oluşturuyorum.Toekn dto dönüşü yapıp refresh token property lerini boş olarak da gönderebilirdim ancak bu şekilde ayrı bir tip yaratmayı tercih ettim
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpiration { get; set; }
    }
}
