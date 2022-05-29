using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.Dtos
{
    public class Response<T> where T : class
    {
        //Client tarafına her şekilde bir status code dönüyorum ayrıca istek başarılı ise data eğer ki istek başarısız ise ErrorDto nesnesi dömek istiyorum
        public int StatusCode { get;private set; }
        public T Data { get;private set; }
        public ErrorDto Error{ get;private set; }
        [JsonIgnore]
        public bool IsSuccesfull { get;private set; }//Bu değişkeni sadece kendi iç yapımda kullanacağım işlemlerin başarılı yada başarısız olduğunu anlamak için status code vs bakmamak için lazım olabilir diye tanımlıyorum.Serileaze esnasında gözardı edilmesi için [JsonIgnore] attribute ile tanımladım

        //Başarılı istek sonucunda kullanıcıya sadece statusCode hemde datdönmek istediğim zaman
        public static Response<T> Success(int statusCode)
        {
            return new Response<T> { StatusCode = statusCode,Data=default,IsSuccesfull=true};
        }
        //Başarılı istek sonucunda kullanıcıya hem statusCode hemde data dönmek istediğim zaman 
        public static Response<T> Success(int statusCode,T data)
        {
            return new Response<T> { StatusCode = statusCode, Data = data,IsSuccesfull=true };
        }
        //Başarısız istek sonucunda kullanıcıya hem statusCode hemde errordto nesnesi dönmek istediğim zaman
        public static Response<T> Fail(int statusCode,ErrorDto errordto)
        {
            return new Response<T> { StatusCode = statusCode, Error = errordto ,IsSuccesfull=false};
        }
        //Başarısız istek sonucunda  tek bir hata olması durumunda statusCode ,tek hata mesajı içeren errordto nesnesi dönmek istediğim zaman
        public static Response<T> Fail(int statusCode, string message,bool ısshow)
        {
           ErrorDto errordto=new ErrorDto(message,ısshow);
            return new Response<T> { StatusCode = statusCode, Error = errordto,IsSuccesfull=false };
        }
    }
}
