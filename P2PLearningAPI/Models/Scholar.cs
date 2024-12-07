using P2PLearningAPI.Models;

namespace P2PLearningAPI.Models
{
    public class Scholar: User
    {
        public int Number_of_request { get; set; } = 5;
        public ICollection<Request> Requests { get; } = new List<Request>();

        public Scholar(string first_name, string lastname, string email, string? profile_pic=null, string? bio=null): 
            base(first_name, lastname, email, profile_pic, bio)
        {}
        public bool IsAllowedToRequest()
        {
            return Requests.Count > 0;
        }
        public bool AddRequest(Request Request)
        {
            if(IsAllowedToRequest()) { 
                Requests.Add(Request); 
                Number_of_request--; 
                return true;
            }
            return false;
            
        }
    }
}
