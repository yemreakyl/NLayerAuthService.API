using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Configurations;
using SharedLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Extensions
{
    public static class CustomTokenAuth
    {
        //Bu class ın amacı apı lerimizin startup tarafında tanımladığım authenticatin configurasyonlarını kod tekrarı yapmamk için bu class üzerinden yöneteceğim
        //Startup tarafında configure services içerisinde işlem yaptığım için methodumda Iservicecollection için bir extension methot yazıyorum bu sebeple de this keyword ile belirtiyorum
        public static void AddCustomtokenAuth(this IServiceCollection services,CustomTokenOption tokenOptions)
        {
            services.AddAuthentication(options =>
            {
                //Benden bir şema istiyor ve Jwt default şemayı veriyorum
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //Burda ise altta jwt den gelen şema ile üstte authentication dan gelen şemayı birbirine bağlıyorum
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                //jwt doğrulama yapacağını belirttim(AddJwtBearer ile) bu şekilde endpointlere request geldiğinde direk header kısmında token arayacak.
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
            {
                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    //Burda ise doğrulama için gerekli değerleri gönderdim
                    ValidIssuer = tokenOptions.Issuer,
                    ValidAudience = tokenOptions.Audience[0],
                    IssuerSigningKey = SignService.GetSecurityKey(tokenOptions.SecurityKey),
                    //burda hangi alanların doğrualama işlemine tutulacağını gösterdim
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero

                };

            });
        }
    }
}
