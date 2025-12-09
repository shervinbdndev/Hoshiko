namespace Hoshiko.Domain.Entities
{
    public class User
    {
        public int Id {get;set;}
        public string UserName {get;set;} = null!;
        public string Email {get;set;} = null!;
    }
}