using P2PLearningAPI.Data;
using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace P2PLearningAPI.Repository
{
    public class RequestRepository : IRequestInterface
    {
        private readonly P2PLearningDbContext _context;
        private readonly ITokenService _tokenService;

        public RequestRepository(P2PLearningDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public ICollection<Request> GetRequests(string token)
        {
            return _context.Requests.Include(r => r.User).OrderBy(r => r.Date_of_request).ToList();
        }

        public Request? GetRequest(long id, string token)
        {
            if(CheckRequestExist(id) == false)
                return null;
            var (UserId, _) = _tokenService.DecodeToken(token);
            var request = _context.Requests.Include(r => r.User).FirstOrDefault(r => r.Id == id)!;
            
            if ( UserId != request.UserId)
                throw new UnauthorizedAccessException("User is not an Adminstrator");
            return request;

        }

        public bool CheckRequestExist(long id)
        {
            return _context.Requests.Any(r => r.Id == id);
        }

        public ICollection<Request> GetRequestsByUser(string userId, string token)
        {
            var (UserId, _) = _tokenService.DecodeToken(token);
            if ( userId != UserId)
                throw new UnauthorizedAccessException("User is not an Authorized to view this request");
            return _context.Requests.Include(r => r.User).Where(r => r.UserId == userId).ToList();
        }

        public Request CreateRequest(Request request, string token)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            User user = _context.Users.Where(u => u.Id == request.UserId).First();
            if (user == null)
                throw new InvalidOperationException("User does not exist");
            var (UserId, _) = _tokenService.DecodeToken(token);
            if (UserId != user.Id)
                throw new UnauthorizedAccessException("User is not Authorized to create this request");
            if (user.AddRequest(request))
            {
                _context.Requests.Add(request);
                if(Save())
                    return request;
                throw new InvalidOperationException("Failed to save the Request to the database");
            }
            throw new InvalidOperationException("User unable to Create Requests");
        }

        public Request UpdateRequest(Request request, string token)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            var (UserId, _) = _tokenService.DecodeToken(token);
            if (UserId != request.UserId)
                throw new UnauthorizedAccessException("User is not Authorized to update this request");
            _context.Requests.Update(request);
            Save();
            return request;
        }

        public bool ApproveRequest(long id, string token)
        {
            var request = GetRequest(id, token);
            if (request == null)
                return false;
            request.ApproveRequest();
            _context.Requests.Update(request);
            return Save();
        }

        public bool CloseRequest(long id, string token)
        {
            var request = GetRequest(id, token);
            if (request == null)
                return false;
            request.CloseRequest();
            _context.Requests.Update(request);
            return Save();
        }

        public bool DeleteRequest(long id, string token)
        {
            var request = GetRequest(id, token);
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
