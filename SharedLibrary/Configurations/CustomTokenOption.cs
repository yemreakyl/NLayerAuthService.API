﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Configurations
{
    public class CustomTokenOption
    {
        //Bu classımın amacı ana apı içindeki appsteetinjson da yer alan token bilgilerine karşılık olarak oluşturdum ve o bilgilerle bu classımı haberdar ediyorum
        public List<string> Audience { get; set; } // Token la istek yapılabilecek olan apı lar
        public string Issuer { get; set; }//Token ı yayınlayan apı
        public int AccessTokenExpiration { get; set; }//Geçerlilik süresi
        public int RefreshTokenExpiration { get; set; }//Geçerlilik süresi
        public string SecurityKey { get; set; }
    }
}
