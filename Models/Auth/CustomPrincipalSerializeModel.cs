using livemenu.Kernel.Authentication;

namespace livemenu.Models.Auth
{
    public class CustomPrincipalSerializeModel
    {
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ID { get; set; }

        public long? RightSubjectID { get; set; }
        public AuthenticationMethod Method { get; set; }
    }
}