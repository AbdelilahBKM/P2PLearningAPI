﻿namespace P2PLearningAPI.Models
{
    public class Request
    {
        public long Id { get; set; }
        public string Topic { get; set; }
        public string Description { get; set; }
        public string  UserId { get; set; }
        public User User { get; set; } = null!;
        public DateTime Date_of_request { get; set; }
        public bool IsApproved { get; set; } = false;
        public bool IsClosed { get; set; } = false;
        public DateTime Created_at { get; set; } = DateTime.Now;
        public DateTime Updated_at { get; set; } = DateTime.Now;
        public Request() { }
        public Request(string Topic, string Description, User User)
        {
            this.Topic = Topic;
            this.Description = Description;
            this.User = User;
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
