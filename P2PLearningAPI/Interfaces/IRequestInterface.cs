using P2PLearningAPI.Models;

namespace P2PLearningAPI.Interfaces
{
    public interface IRequestInterface
    {
        ICollection<Request> GetRequests(string token);
        Request? GetRequest(long id, string token);
        bool CheckRequestExist(long id);
        ICollection<Request> GetRequestsByUser(string userId, string token);
        Request CreateRequest(Request request, string token);
        Request UpdateRequest(Request request, string token);
        bool ApproveRequest(long id, string token);
        bool CloseRequest(long id, string token);
        bool DeleteRequest(long id, string token);
        bool Save();
    }
}
