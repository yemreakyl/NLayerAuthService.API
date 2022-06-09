using AuthServer.Core.Configurations;
using AuthServer.Core.DTOs;
using AuthServer.Core.IUnitOfWork;
using AuthServer.Core.Models;
using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<UserApp> _userManager;//Kullanıcı verilerine ihtyiacım olduğu için Login Dto içerisinde
        private readonly List<Client> _clients;//Client token içerisinde clients lere ihtyiacım olduğu için 
        private readonly ITokenService _tokenService;//tokenı üretecek olan yapı olduğu için
        private readonly IUnitOfWork _unitOfWork;//veri tabanına gideceğim için 
        private readonly IGenericRepository<UserRefreshToken> _userRefreshTokenRepo;// Refresh tokenları veri tabanına kaydedeceğim için

        //Aşağıda Client ları option olarak almam şu manaya geliyor . Option dediğim anda program Apı içerisindeki startup.cs ye gidiyor ve orada ConfigureServices altındaki vereceğim nesneye bakıyor ve ona işaret ettiğim yerden veriyi alıp getiriyor. Burda clients larımı app.setting json içerisinde tuttuğum için ordan alıp listeye dönüştürüp bana geri gatirecek
        public AuthenticationService(UserManager<UserApp> userManager, IOptions<List<Client>> optionsClient, ITokenService tokenService, IUnitOfWork unitOfWork, IGenericRepository<UserRefreshToken> genericRepository)
        {
            _userManager = userManager;
            _clients = optionsClient.Value;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _userRefreshTokenRepo = genericRepository;
        }

        public Response<ClientTokenDto>CreateClientToken(ClientLoginDto clientLoginDto)
        {
            if (clientLoginDto == null) throw new ArgumentNullException(nameof(clientLoginDto)); 
            var Client=_clients.SingleOrDefault(x=>x.Id==clientLoginDto.ClientId&&x.Secret==clientLoginDto.ClientSecret);
            if (Client == null)
            {
                return Response<ClientTokenDto>.Fail(404, "Client Id or Secret is wrong", true);
            }
            var Token= _tokenService.CreateClientToken(Client);
            return Response<ClientTokenDto>.Success(200,Token);

        }

        public async Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto)
        {
            if (loginDto == null) throw new ArgumentNullException(nameof(loginDto));
            var User = await _userManager.FindByEmailAsync(loginDto.Email);
            if (User == null) return Response<TokenDto>.Fail(400, "Email or password is wrong", true);
            if (!await _userManager.CheckPasswordAsync(User, loginDto.Password))
                {
                return Response<TokenDto>.Fail(400, "Email or password is wrong", true);
                };
            var Token=_tokenService.CreateToken(User);
            var userRefreshToken = await _userRefreshTokenRepo.Where(x => x.UserId == User.Id).SingleOrDefaultAsync();
            if (userRefreshToken != null)
            {
                await _userRefreshTokenRepo.AddAsync(new UserRefreshToken() { UserId = User.Id, Code = Token.RefreshToken, Expiration = Token.RefreshTokenExpiration });
            }
            else
            {
                //Demekki veri tabanında varmış o zaman güncelleme yapıyorum
                    userRefreshToken.Code = Token.RefreshToken;
                    userRefreshToken.Expiration = Token.RefreshTokenExpiration;
            }
            _unitOfWork.SaveChanges();
            return Response<TokenDto>.Success(200, Token);
        }

        public async Task<Response<TokenDto>> CreateTokenByRefreshAsync(string refreshToken)
        {
            var ExistRefreshToken = await _userRefreshTokenRepo.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();
            if (ExistRefreshToken == null)
            {
                return Response<TokenDto>.Fail(404, "Refresh token not found", true);
            }
            var User=await _userManager.FindByIdAsync(ExistRefreshToken.UserId);
            if (User == null)
            {
                return Response<TokenDto>.Fail(404, "User not found", true);
            }
            var Token = _tokenService.CreateToken(User);//Yeni bir token oluşturduk bu yüzden yeni bir refresh token da içinde geldi bu yüzden veri tabanında var olan eski refreh token üzerinde güncelleme yapmam gerekiyor
            ExistRefreshToken.Code = Token.RefreshToken;
            ExistRefreshToken.Expiration = Token.RefreshTokenExpiration;
            _unitOfWork.SaveChangesAsync();
            return Response<TokenDto>.Success(200,Token);
        }

        public async Task<Response<NoContentDto>> RevokeRefreshToken(string refreshToken)
        {
            var ExistRefreshToken=await _userRefreshTokenRepo.Where(x=>x.Code == refreshToken).SingleOrDefaultAsync();
            if (ExistRefreshToken==null)
            {
                return Response<NoContentDto>.Fail(404, "Refresh token not found", true);
            }
            _userRefreshTokenRepo.Remove(ExistRefreshToken);
            await _unitOfWork.SaveChangesAsync();
            return Response<NoContentDto>.Success(200);
        }
    }
}
