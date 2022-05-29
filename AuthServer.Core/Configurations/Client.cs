using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Configurations
{
    public class Client
    {
        //Bu modelim ne dto ne de entity model olmadığı için ayrı bir klasörde oluşturdum
        public int Id { get; set; }
        public string Secret { get; set; }//Client ın passwordu nedir
        public List<string> Audiences { get; set; }//Bu client hangi apı lere erişebilecek
    }
}
