using AuthServer.Core.Configurations;
using AuthServer.Core.DTOs;
using AuthServer.Core.Models;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Configurations;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Service.Services
{
    public class TokenService : ITokenService
    {
        //Bu classımın amacı kullanıcıya token ları oluşturmak ve bunun için 2 tane property e ihtiyacım olcak bunlar user manager tipinde kulllanıcıları yöneteceğim nesne ve token bilgilerini aldığım shared içindeki customtokenoption classı nesnesi.
        //User manager classı identity içerisinde bulunur ve kullanıcıları yönetmek için kullanılır.(kayıt ekleme,silme doğrulama vs)

        private readonly UserManager<UserApp> _usermanager;
        private readonly CustomTokenOption _tokenoption;

        
        public TokenService(UserManager<UserApp> usermanager, IOptions<CustomTokenOption> options)        {
            _usermanager = usermanager;
            _tokenoption = options.Value;
        }
        //ürettiiğim token içerisinde göndermek için bir refresh token üretiyorum
        private string CreateRefreshToken()
        {
            var Bytes = new byte[32];
            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(Bytes);
            return Convert.ToBase64String(Bytes);
        }
        //ürettiğim tokenın payload bölümünde yer alan key value çiftlerini oluşturacağım .Bu kısımda kullanıcı ile alakalı key value çiftlerini ve audiences leri tutmak istedim
        //Bu methodum kullanıcı için oluşturacağım tokenın claim nesnelerini oluşturuyorum
        //*claim* sınıfı payload içerisinde yer alan her bir key value çiftine verilen addır.Yani payload bölümü claim lerden oluşur bu sebeple methodum da claim enumerable dönüyorum
        private IEnumerable<Claim> GetClaimsforusers(UserApp userApp, List<string> audiences)
        {
            var claims = new List<Claim>()
            {
                //Burda Claimtypes yada JwtRegisteredClaimNames sabit yapıları içerisinde en çok kullanılan ve sistemin tanıdığı key tipleri var, kendim "name" yazmak yerine ClaimTypes.Name gibi sabit yapıları kullanıyorum ki sistem direk name kısmı olduğunu otomatik algılıyor
                new Claim(ClaimTypes.NameIdentifier,userApp.Id),
                new Claim(ClaimTypes.Name,userApp.UserName),
                new Claim(JwtRegisteredClaimNames.Email,userApp.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())//rastgele bir token id si oluşturdum
            };
            claims.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));// audiences listesi içerisinde dönerek her bir audiences için bir aud,x claim i oluştur
            return claims;

        }
        //Bu methodum ise client lar için oluşturacağım tokenlarda kullanacağım claim leri üretecek
        private IEnumerable<Claim> GetClaimsforclients(Client client)
        {
            var Claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub,client.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),//Token a özel bir uniq değer
            };
            Claims.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            return Claims;
        }
        
        public ClientTokenDto CreateClientToken(Client client)
        {
            var accessTokenİnspiration = DateTime.Now.AddMinutes(_tokenoption.AccessTokenİnspiration);//Token options tan adlığım geçerlilik süresini oluştuğu ana ekleyerek son geçerlilik süresini belirtiyorum
            var securityKey = SignService.GetSecurityKey(_tokenoption.SecurityKey);//sign service classımda oluşturduğum security keyi aldım
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.Aes128KW);//İmzayla şifrelemeyi yapan class

            JwtSecurityToken JwtSecurityToken = new JwtSecurityToken(
                issuer: _tokenoption.Issuer,
                expires: accessTokenİnspiration,
                notBefore: DateTime.Now,
                claims: GetClaimsforclients(client),
                signingCredentials: signingCredentials
                  );

            var TokenHandler = new JwtSecurityTokenHandler();//Asıl tokenı yazan class bu yukarıdaki JwtSecurityTokenı parametre olarak alıyor ve token ı yazıyor
            var Token = TokenHandler.WriteToken(JwtSecurityToken);
            var TokenDto = new ClientTokenDto()
            {
                AccessToken = Token,
                AccessTokenExpiration = accessTokenİnspiration,
            };
            return TokenDto;
        }

        public TokenDto CreateToken(UserApp userApp)
        {
            var accessTokenİnspiration = DateTime.Now.AddMinutes(_tokenoption.AccessTokenİnspiration);//Token options tan adlığım geçerlilik süresini oluştuğu ana ekleyerek son geçerlilik süresini belirtiyorum
            var refreshTokenİnspiration = DateTime.Now.AddMinutes(_tokenoption.RefresTokenİnspiration);
            var securityKey = SignService.GetSecurityKey(_tokenoption.SecurityKey);//sign service classımda oluşturduğum security keyi aldım
            SigningCredentials signingCredentials=new SigningCredentials(securityKey,SecurityAlgorithms.Aes128KW);//İmzayla şifrelemeyi yapan class

            JwtSecurityToken JwtSecurityToken = new JwtSecurityToken(
                issuer:_tokenoption.Issuer,
                expires: accessTokenİnspiration, 
                notBefore: DateTime.Now,
                claims: GetClaimsforusers(userApp,_tokenoption.Audiences),
                signingCredentials:signingCredentials 
                  );

            var TokenHandler = new JwtSecurityTokenHandler();//Asıl tokenı yazan class bu yukarıdaki JwtSecurityTokenı parametre olarak alıyor ve token ı yazıyor
            var Token=TokenHandler.WriteToken(JwtSecurityToken);
            var TokenDto = new TokenDto()
            {
                AccessToken = Token,
                RefreshToken = CreateRefreshToken(),
                AccessTokenExpiration = accessTokenİnspiration,
                RefreshTokenExpiration = refreshTokenİnspiration
            };
            return TokenDto;
        }
    }
}
