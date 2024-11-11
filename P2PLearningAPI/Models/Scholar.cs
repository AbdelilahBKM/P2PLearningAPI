using P2PLearningAPI.Models;

namespace P2PLearningAPI.Models
{
    public class Scholar: User
    {
        public int Number_of_request { get; set; } = 10;
        public ICollection<Request> Requests { get; } = new List<Request>();

        public Scholar(string first_name, string lastname, string email): base(first_name, lastname, email)
        {}
    }
}
