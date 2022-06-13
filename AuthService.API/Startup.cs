using AuthServer.Core.Configurations;
using AuthServer.Core.IUnitOfWork;
using AuthServer.Core.Models;
using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Data;
using AuthServer.Data.Repositories;
using AuthServer.Data.UnitOfWork;
using AuthServer.Service.Services;
using AuthService.Service;
using AuthService.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SharedLibrary.Configurations;
using System.Collections.Generic;

namespace AuthService.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            //Dependecy injection iþlemleri için
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));//Generic interfaceler için bu þekilde(typeof) yazýyorum
            services.AddScoped(typeof(IGenericService<,>), typeof(GenericServices<,>));

            //DbContext
            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("SqlServer"));

            });

            //Burda Identity mi ve identity içerisinde ki optionslarýmý tanýmlýyorum
            services.AddIdentity<UserApp, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequireNonAlphanumeric = true;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();//þifre sýfýrlama gibi iþlemler için
            //identity e hangi orm aracýný kullandýðýmý belirttim    




            //Bu configurasyonla birlikte appsettingjson içerisinde yer alan options larýmý alýp oluþturmuþ olduðum  classlarýma geçtim
            services.Configure<CustomTokenOption>(Configuration.GetSection("TokenOptions"));
            services.Configure<List<Client>>(Configuration.GetSection("Client"));

            //Bu kýsýmda authentication ile alakalý iþlemlerimi gerçekleþtiriyorum
            services.AddAuthentication(options =>
            {
                //Benden bir þema istiyor ve Jwt default þemayý veriyorum
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //Burda ise altta jwt den gelen þema ile üstte authentication dan gelen þemayý birbirine baðlýyorum
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;


                //jwt doðrulama yapacaðýný belirttim(AddJwtBearer ile) bu þekilde endpointlere request geldiðinde direk header kýsmýnda token arayacak.
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>

            {
                //Token bilgilerini kullanmak için app.setting.json dan alýp bir nesne örneði oluþturdum
                var tokenOption = Configuration.GetSection("TokenOption").Get<CustomTokenOption>();
                //Gelen token üzerinde yapacaðým validation iþlemleri
                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    //burda hangi alanlarýn doðrualama iþlemine tutulacaðýný gösterdim
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,

                    //Burda ise doðrulama için gerekli deðerleri gönderdim
                    ValidIssuer = tokenOption.Issuer,
                    ValidAudience = tokenOption.Audiences[0],
                    IssuerSigningKey = SignService.GetSecurityKey(tokenOption.SecurityKey),



                };

            });







            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthService.API", Version = "v1" });
            });
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthService.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();//Authorization öncesine authentication ekliyorum.Burda sýralama önemli

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
