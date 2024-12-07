namespace P2PLearningAPI.Models
{
    public class Request
    {
        public long Id { get; set; }
        public string Topic { get; set; }
        public string Description { get; set; }
        public long  UserId { get; set; }
        public Scholar User { get; set; } = null!;
        public DateTime Date_of_request { get; set; }
        public bool IsApproved { get; set; } = false;
        public bool IsClosed { get; set; } = false;
        public Request() { }
        public Request(string Topic, string Description, User User)
        {
            this.Topic = Topic;
            this.Description = Description;
            this.User = (Scholar)User;
            UserId = User.Id;
            Date_of_request = DateTime.Now;
        }

        public void ApproveRequest()
        {
            IsApproved = true;
            IsClosed = false;
        }
        public void CloseRequest()
        {
            IsApproved = false;
            IsClosed = true;
        }
        
    }
}
