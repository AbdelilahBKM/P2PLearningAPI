using P2PLearningAPI.Data;
using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace P2PLearningAPI.Repository
{
    public class QuestionRepository : IQuestionInterface
    {
        private readonly P2PLearningDbContext _context;

        public QuestionRepository(P2PLearningDbContext context)
        {
            _context = context;
        }

        // Get all questions
        public ICollection<Question> GetQuestions()
        {
            return _context.Questions.Include(q => q.Answers).ToList();
        }

        // Get a question by ID
        public Question GetQuestion(long id)
        {
            return _context.Questions.Include(q => q.Answers).FirstOrDefault(q => q.Id == id);
        }

        // Check if a question exists by ID
        public bool CheckQuestionExist(long id)
        {
            return _context.Questions.Any(q => q.Id == id);
        }

        // Get questions by discussion ID
        public ICollection<Question> GetQuestionsByDiscussion(long discussionId)
        {
            return _context.Questions.Where(q => q.DiscussionId == discussionId).ToList();
        }

        // Create a new question
        public Question CreateQuestion(Question question)
        {
            _context.Questions.Add(question);
            Save();
            return question;
        }

        // Update an existing question
        public Question UpdateQuestion(Question question)
        {
            _context.Questions.Update(question);
            Save();
            return question;
        }

        // Delete a question by ID
        public bool DeleteQuestion(long id)
        {
            var question = GetQuestion(id);
            if (question == null)
                return false;

            _context.Questions.Remove(question);
            return Save();
        }

        // Get answers for a specific question
        public ICollection<Answer> GetAnswersForQuestion(long questionId)
        {
            var question = GetQuestion(questionId);
            return question?.Answers ?? new List<Answer>();
        }

        // Save changes to the database
        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
