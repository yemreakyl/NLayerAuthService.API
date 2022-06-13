namespace AuthServer.Core.DTOs.GetDtos
{
    public class UserDto
    {
        //Bu dto nun amacı client tarafı bir kullanıcının bilgilerini istediği zaman onlara tüm verileri dönmek yerine sadece istediğim verileri dönmek amaçlı.Bu modelimi Iuserservice içerisinde isme göre kullanıcıları getiren methodumda kullanacağım
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
    }
}
