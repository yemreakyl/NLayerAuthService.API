using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.DTOs.GetDtos
{
    public class CreateUserDto
    {
        //Bu dto nun amacı veri tabanına yeni bir kullanıcı kaydı yapmak istediğim zaman ilk aşamada benim için gerekli olan verileri almak maksatlı ve bu modelimi Iuserservice içerisindeki yeni kullanıcı kaydı yapan methodumda kullanacağım
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
