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
using System;
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
            //Dependecy injection i�lemleri i�in
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));//Generic interfaceler i�in bu �ekilde(typeof) yaz�yorum
            services.AddScoped(typeof(IGenericService<,>), typeof(GenericServices<,>));

            //DbContext
            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("SqlServer"), sqlopt =>
                {
                    sqlopt.MigrationsAssembly("AuthService.Data");
                });

            });

            //Burda Identity mi ve identity i�erisinde ki optionslar�m� tan�ml�yorum
            services.AddIdentity<UserApp, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();//�ifre s�f�rlama gibi i�lemler i�in
                                                                                   //identity e hangi orm arac�n� kulland���m� belirttim    




            //Bu configurasyonla birlikte appsettingjson i�erisinde yer alan options lar�m� al�p olu�turmu� oldu�um  classlar�ma ge�tim
            services.Configure<CustomTokenOption>(Configuration.GetSection("TokenOption"));
            services.Configure<List<Client>>(Configuration.GetSection("Clients"));

            //Bu k�s�mda authentication ile alakal� i�lemlerimi ger�ekle�tiriyorum
            services.AddAuthentication(options =>
            {
                //Benden bir �ema istiyor ve Jwt default �emay� veriyorum
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //Burda ise altta jwt den gelen �ema ile �stte authentication dan gelen �emay� birbirine ba�l�yorum
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                //jwt do�rulama yapaca��n� belirttim(AddJwtBearer ile) bu �ekilde endpointlere request geldi�inde direk header k�sm�nda token arayacak.
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
            {
                //Token bilgilerini kullanmak i�in app.setting.json dan al�p bir nesne �rne�i olu�turdum
                var tokenOptions = Configuration.GetSection("TokenOption").Get<CustomTokenOption>();
                //Gelen token �zerinde yapaca��m validation i�lemleri
                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    //Burda ise do�rulama i�in gerekli de�erleri g�nderdim
                    ValidIssuer = tokenOptions.Issuer,
                    ValidAudience = tokenOptions.Audience[0],
                    IssuerSigningKey = SignService.GetSecurityKey(tokenOptions.SecurityKey),
                    //burda hangi alanlar�n do�rualama i�lemine tutulaca��n� g�sterdim
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero

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
            app.UseAuthentication();//Authorization �ncesine authentication ekliyorum.Burda s�ralama �nemli

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
