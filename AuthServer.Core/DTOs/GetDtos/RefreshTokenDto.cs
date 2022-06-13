using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Core.DTOs.GetDtos
{
    public class RefreshTokenDto
    {
        //Bu class ın amacı apı controller tarafında revoke refresh token controller ını yazarken kullanıcıdan direk string tipte refresh token  almak yerine bir class almak istiyorum
        public string Token { get; set; }
    }
}
