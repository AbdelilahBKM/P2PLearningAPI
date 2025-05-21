using P2PLearningAPI.DTOsOutput;

namespace P2PLearningAPI.Interfaces
{
    public interface ISimularityInterface
    {
        ICollection<SimularityDTO> GetSimularities();
        SimularityDTO? GetSimularityById(long id);
        SimularityDTO? GetSimularity(long QuestionId);
    }
}
