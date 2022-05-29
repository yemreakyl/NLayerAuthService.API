using AuthServer.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Data
{
    public class AppDbContext:IdentityDbContext<UserApp,IdentityRole,string>
    {
        //Data katmanında öncelikle pmc den 4 tane paket yükledim.solution explorerdan bakablirsin
        //Burda benim 3 tane modelim var normalde ve bu modellerden userapp identity kütüphanesini kullanarak veritabanında oluşuyor ben bu userapp ile diğer iki modelimi de aynı veri tabanında tutmak ve iki tane insance oluturmamak için DbContext yerine IdentityDbContext sınıfından miras alıyorum.IdentityDbContext te aslında dbcontext te bağlandığı için aynı instance de bağlanmış oluyorum
        //AppDbContext sınıfı DbContext için gerekli opsiyonları ifade eder ve bizde yapıcı metot içerisinde alıp Identitytarafında arka planda dbsetler oluşması için gönderiyoruz
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }

        DbSet<Product> products;
        DbSet<UserRefreshToken> refreshTokens;
    }
}
