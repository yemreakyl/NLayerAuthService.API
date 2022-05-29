using AuthServer.Core.Configurations;
using AuthServer.Core.DTOs;
using AuthServer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface ITokenService
    {
        //Bu yapıyı kendi iç yapımda kullanacağım yani apı ile haberleşmeyecek bu sebeple dönüş tipi response şeklinde değil.IAuthenticationservice interfacesi apı ile haberleşecek bu interface ise IAuthenticationservice in doğruladığı kullanıcılara token üretecek. İki çeşit token üretiyoruz
        TokenDto CreateToken(UserApp userApp);
        ClientTokenDto CreateClientToken(Client client);//Kullanıcı adı şifre gerektirmeyen clientlar için
    }
}
