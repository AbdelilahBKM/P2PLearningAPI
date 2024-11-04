namespace P2PLearningAPI.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DatePosted { get; set; }

        // Relationship with the User class
        public int UserId { get; set; }
        public User User { get; set; }

        // List of answers for the question
        public List<Answer> Answers { get; set; } = new List<Answer>();

        // Upvote and downvote counts
        public int Upvotes { get; set; } = 0;
        public int Downvotes { get; set; } = 0;

        public void Upvote()
        {
            Upvotes++;
        }

        public void Downvote()
        {
            Downvotes++;
        }
    }
}