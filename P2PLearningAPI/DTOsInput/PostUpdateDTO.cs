namespace P2PLearningAPI.DTOsInput
{
    public class PostUpdateDTO
    {
        public long Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
    }
}
