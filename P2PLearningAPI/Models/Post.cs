using Microsoft.Identity.Client;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace P2PLearningAPI.Models
{
    public class Post
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsUpdated { get; set; } = false;
        public long Reputation { get; set; } = 0;
        public DateTime PostedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }
        public long UserID { get; set; }
        public User PostedBy { get; set; } = null!;
        public bool IsClosed { get; set; } = false;
        public ICollection<Vote> Votes { get; } = new HashSet<Vote>();
        public Post() { }
        public Post(string Title, string Content, User PostedBy) {
            this.Title = Title;
            this.Content = Content;
            this.PostedBy = PostedBy;
            this.UserID = PostedBy.Id;
        }
        public void AddVote(Vote vote)
        {
            Votes.Add(vote);
            switch (vote.VoteType)
            {
                case VoteType.Positive:
                    Reputation += 1;
                    break;
                case VoteType.Negative:
                    Reputation -= 1;
                    break;
                default:
                    break;
            }

        }
        public void RemoveVote(Vote vote)
        {
            Votes.Remove(vote);
            switch (vote.VoteType)
            {
                case VoteType.Positive:
                    Reputation -= 1;
                    break;
                case VoteType.Negative:
                    Reputation += 1;
                    break;
            }
        }
    }
}
