using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AuthServer.Service
{
    public static class ObjectMapper
    {
        //en önce outo mapper kütüphanesini ekliyorum
        //öncelikle mapleme işlemini yapmak için Imapper nesnesine ihtiyacım var bu ımapper nesnesini lazy içerisinde veriyorum ki uygulama ayağa kalkınca değilde ihtiyaç olduğunda aktif olsun performans açısından.
        //Daha sonra bu ımapperı aktif edebilmek için bir mapperconfiguration nesnesine ihtiyacım var 
        //ben burda ı mapperı tanımlarken yapıcı methodunda direk mapperconfiguration da vermiş oldum
        //ama istersem mapper configuration ı ayrı bir yer oluşturup daha sonra da verebilirdim
        //bu mapper configuration classı içerisinde ilk kez create map yapabilirim yada benim yaptığım gibi create map işlerini dtomapper içerisinde profile den miras alan bir class içerisinde yapıp mapperconfiguration içerisinde add profile dan hazır profile ekleyerek verebilirim
        //Daha sonra da return ile createmapper diyerek mapleyici mi yaratıyorum
        //lazy bir durum olduğu için lambda ile içine girerken ()=> şeklinde girmem gerekiyor
        
        private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(()=>
        {
            var config = new MapperConfiguration(cfg =>
              {
                  cfg.AddProfile<DtoMapper>();
              });
            return config.CreateMapper();
        });
      
        public static IMapper Mapper => lazy.Value;//Bu ifade(value) lazy başlatılan durumlarda  objectMapper.Mapper dediğim anda lazy başlatılacak kodları çalıştırır

    }

}
