using P2PLearningAPI.Models;

namespace P2PLearningAPI.Interfaces
{
    public interface IRequestInterface
    {
        ICollection<Request> GetRequests();
        Request GetRequest(long id);
        bool CheckRequestExist(long id);
        ICollection<Request> GetRequestsByUser(long userId);
        Request CreateRequest(Request request);
        Request UpdateRequest(Request request);
        bool ApproveRequest(long id);
        bool CloseRequest(long id);
        bool DeleteRequest(long id);
        bool Save();
    }
}
