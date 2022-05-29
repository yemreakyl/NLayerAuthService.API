using AuthServer.Core.DTOs;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface IAuthenticationService
    {
        //Apı ile haberleşip token dönecek olan interface.Itokenservice interfacesi token ı üreten interface, bu interface ise gelen isteğin doğrulamasını yapıp kontrol ettikten sonra Itokenservice aracılığıyla üretip apı ye dönüş yapaacak
        Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto);
        Task<Response<TokenDto>> CreateTokenByRefreshAsync(string refreshToken);//Refresh token göndererek yeni token isteği
        Task<Response<ClientTokenDto>> CreateClientTokenAsync(ClientLoginDto clientLoginDto);

        Task<Response<NoContentDto>> RevokeRefreshToken(string refreshToken);//Refresh tokenı veritabanında null olarak değiştirecek methot aynı zamanda başkasının eline geçen bir refresh tokenı geçersiz kılmak amacıyla da kullanılabilir



    }
}
