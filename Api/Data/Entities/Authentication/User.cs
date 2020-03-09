using System.ComponentModel;

namespace Api.Data.Entities.Authentication
{
    public class User
    {
        public int ID { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public int RelatedAccountID { get; set; }
        public bool IsEmailVerified { get; set; }
    }
}
