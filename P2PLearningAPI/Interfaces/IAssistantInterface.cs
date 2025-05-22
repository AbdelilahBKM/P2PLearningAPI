using P2PLearningAPI.DTOsOutput;

namespace P2PLearningAPI.Interfaces
{
    public interface IAssistantInterface
    {
        ICollection<SimularityDTO> GetSimularities();
        SimularityDTO? GetSimularityById(long id);
        SimularityDTO? GetSimularity(long QuestionId);
        SuggestedAnswerDTO? GetSuggestedAnswer(long QuestionId);

    }
}
