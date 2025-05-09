﻿using Microsoft.EntityFrameworkCore;
using P2PLearningAPI.Data;
using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace P2PLearningAPI.Repository
{
    public class JoiningRepository : IJoiningInterface
    {
        private readonly P2PLearningDbContext _context;
        private readonly ITokenService _tokenService;

        // Constructor to inject the DbContext
        public JoiningRepository(P2PLearningDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // Get all joinings
        public ICollection<Joining> GetJoinings()
        {
            return _context.Joinings.OrderBy(j => j.Id).ToList();
        }

        // Get a single joining by id
        public Joining? GetJoining(long id)
        {
            return _context.Joinings.FirstOrDefault(j => j.Id == id);
        }

        public Joining? GetJoining(string userId, long discussionId)
        {
            return _context.Joinings.FirstOrDefault(j => j.UserId == userId && j.DiscussionId == discussionId);
        }

        // Get joinings by userId
        public ICollection<Joining> GetJoiningsByUser(string userId)
        {
            return _context.Joinings
                .Where(j => j.UserId == userId)
                .Include(j => j.Discussion)
                .ToList();
        }

        // Get joinings by discussionId
        public ICollection<Joining> GetJoiningsByDiscussion(long discussionId)
        {
            return _context.Joinings.Where(j => j.DiscussionId == discussionId).ToList();
        }

        // Create a new joining
        public Joining CreateJoining(Joining joining, string token)
        {
            if (joining == null)
                throw new ArgumentNullException(nameof(joining));
            (string userId, var _) = _tokenService.DecodeToken(token);
            if (joining.UserId != userId)
                throw new UnauthorizedAccessException("Unauthorized to create this joining.");
            Joining? test = _context.Joinings.Where(j => 
            (j.UserId == joining.UserId && j.DiscussionId == joining.DiscussionId)
                ).FirstOrDefault();
            if (test != null)
                throw new InvalidOperationException("Joining already exist");
            _context.Joinings.Add(joining);
            Discussion? discussion = _context.Discussions.FirstOrDefault(d => d.Id == joining.DiscussionId);
            if (discussion == null)
                throw new InvalidOperationException("Discussion not found.");
            discussion.Number_of_members++;
            if (Save())
                return joining;
            throw new InvalidOperationException("Failed to save the joining to the database.");
        }

        // Delete a joining by id
        public bool DeleteJoining(long id, string Token)
        {
            (string userId,var _) = _tokenService.DecodeToken(Token);
            var joining = GetJoining(id);
            if (joining == null)
                throw new InvalidOperationException("Joining not found.");
            if(joining.UserId != userId)
                throw new UnauthorizedAccessException("Unauthorized to delete this joining.");
            Discussion? discussion = _context.Discussions.FirstOrDefault(d => d.Id == joining.DiscussionId);
            if (discussion == null)
                throw new InvalidOperationException("Discussion not found.");
            _context.Joinings.Remove(joining);
            discussion.Number_of_members--;
            return Save();
        }

        // Save changes to the database
        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
