using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Dtos
{
    public class ErrorDto
    {
        //Bu class ın amacı hata durumunda aynı tip döneceğim nesne dönmek istiyorum.Bu classımı tek tip response dönmek için tasarladığım customresponse classı içerisinde kullanacağım.

        public List<string> Errors { get;private set; }//Birden fazla hata olabilir
        public bool IsShow { get;private set; }//Hata kullanıcıya gösterilecek mi?

        public ErrorDto()
        {
            Errors = new List<string>();
        }
        public ErrorDto(string Error,bool Isshow)
        {
            this.Errors.Add(Error);
            this.IsShow = Isshow;
        }
        public ErrorDto(List<string> Errors,bool Isshow)
        {
            this.Errors=Errors;
            this.IsShow = Isshow;
        }



    }
}
