using AuthServer.Core.DTOs.GetDtos;
using AuthServer.Core.Models;
using AuthServer.Core.Services;
using AuthServer.Service;
using Microsoft.AspNetCore.Identity;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserApp> _userManager;//Kullanıcı verileri ile alakalı işlemler yapacağım için ihtiyacım var

        public UserService(UserManager<UserApp> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Response<UserDto>> CreateUserAsync(CreateUserDto createUserDto)
        {
            var User=new UserApp() { Email=createUserDto.Email,  UserName=createUserDto.UserName};//Yeni kullanıcı eklerken ilk aşamada sadece email, user nama ve password almanın yeterli olacağını düşünmüştüm email ve username verileri ile userapp oluşturuyorum 
            var Result=await _userManager.CreateAsync(User,createUserDto.Password);//Burda usermanager aracılığı ile bir user oluşturmak istedim 
            if (!Result.Succeeded)// Kullanıcıyı oluşturamazsa
            {
                var Errors = Result.Errors.Select(x => x.Description).ToList();//Hata listesinin açıklama kısımlarını aldım
                return Response<UserDto>.Fail(400,new ErrorDto(Errors,true));

            }
            return Response<UserDto>.Success(200,ObjectMapper.Mapper.Map<UserDto>(User));//KKaydetmiş olduğum kullanıcıyı automapper kullanarak userdto nesnesine dönüştürdü

        }


        public async Task<Response<UserDto>> GetUserByNameAsync(string username)
        {
            var User=await _userManager.FindByNameAsync(username);
            if (User == null)
            {
                return Response<UserDto>.Fail(404, "User name not found",true);
            }
            return Response<UserDto>.Success(200, ObjectMapper.Mapper.Map<UserDto>(User));
        }
    }
}
