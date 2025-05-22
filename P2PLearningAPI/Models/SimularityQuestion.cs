using Microsoft.EntityFrameworkCore;

namespace P2PLearningAPI.Models
{
    public class SimularityQuestion
    {
        public long SimularityId { get; set; }
        public Simularity Simularity { get; set; } = null!;
        public long QuestionId { get; set; }
        public Question Question { get; set; } = null!;
        public double Score { get; set; }
    }
}
