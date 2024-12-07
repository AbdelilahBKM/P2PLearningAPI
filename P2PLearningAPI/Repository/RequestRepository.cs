using P2PLearningAPI.Data;
using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace P2PLearningAPI.Repository
{
    public class RequestRepository : IRequestInterface
    {
        private readonly P2PLearningDbContext _context;

        public RequestRepository(P2PLearningDbContext context)
        {
            _context = context;
        }

        public ICollection<Request> GetRequests()
        {
            return _context.Requests.Include(r => r.User).OrderBy(r => r.Date_of_request).ToList();
        }

        public Request GetRequest(long id)
        {
            return _context.Requests.Include(r => r.User).FirstOrDefault(r => r.Id == id)!;
        }

        public bool CheckRequestExist(long id)
        {
            return _context.Requests.Any(r => r.Id == id);
        }

        public ICollection<Request> GetRequestsByUser(long userId)
        {
            return _context.Requests.Include(r => r.User).Where(r => r.UserId == userId).ToList();
        }

        public Request CreateRequest(Request request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            Scholar user = _context.Scholars.Where(u => u.Id == request.Id).First();
            if (user == null)
                throw new InvalidOperationException("User does not exist");
            if (user.AddRequest(request))
            {
                _context.Requests.Add(request);
                if(Save())
                    return request;
                throw new InvalidOperationException("Failed to save the Request to the database");
            }
            throw new InvalidOperationException("User unable to Create Requests");
        }

        public Request UpdateRequest(Request request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            _context.Requests.Update(request);
            Save();
            return request;
        }

        public bool ApproveRequest(long id)
        {
            var request = GetRequest(id);
            if (request == null)
                return false;

            request.ApproveRequest();
            _context.Requests.Update(request);
            return Save();
        }

        public bool CloseRequest(long id)
        {
            var request = GetRequest(id);
            if (request == null)
                return false;

            request.CloseRequest();
            _context.Requests.Update(request);
            return Save();
        }

        public bool DeleteRequest(long id)
        {
            var request = GetRequest(id);
            if (request == null)
                return false;

            _context.Requests.Remove(request);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
