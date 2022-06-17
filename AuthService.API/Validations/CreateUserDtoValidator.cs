using AuthServer.Core.DTOs.GetDtos;
using FluentValidation;

namespace AuthService.API.Validations
{
    public class CreateUserDtoValidator:AbstractValidator<CreateUserDto>
    {
        //Bu class'ın amacı kullanıcı tarafı yeni bir kullancı oluşturmak istediği zaman eksik bilgi gönderirse geriye bir cevap dönmek istiyorum. Bunu Create user dto modei üzerinde "required" ile işaretleyerek de yapabilirdim ancak best pratice açısından fluent validations kütüphanesi aracılığıyla yapıyorum
        //constructor içerisinde işlemlerimi yapıyorum
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email can not be empty").EmailAddress().WithMessage("İnvalid email");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Name can not be empty");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password can not be empty");
            //Bu validationsları yapıcı methot içerisinde belirledikten sonra startup tarafına apı'ye gösteriyorum
        }
    }
}
