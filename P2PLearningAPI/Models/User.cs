﻿using Microsoft.AspNetCore.Identity;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;

namespace P2PLearningAPI.Models
{
    public enum UserType { Scholar, Administrator }
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ProfilePicture { get; set; } 
        public string? Bio { get; set; }
        public bool IsActive { get; set; }
        public UserType UserType { get; set; }
        public int? numberOfRequests { get; set; }
        public ICollection<Joining> Joinings { get; } = new HashSet<Joining>();
        public ICollection<Post> Posts { get; } = new HashSet<Post>();
        public ICollection<Vote> Votes { get; } = new HashSet<Vote>();
        public ICollection<Request> Requests { get; } = new HashSet<Request>();
        public ICollection<Notification> Notifications { get; } = new HashSet<Notification>();
        public ICollection<UserBadge> userBadges { get; } = new HashSet<UserBadge>();
        public DateTime Last_Login { get; set; }
        public bool AccountDeleted { get; set; } = false;
        public DateTime Created_at { get; set; } = DateTime.Now;
        public DateTime Updated_at { get; set; } = DateTime.Now; 

        public bool AddRequest(Request request)
        {
            if(numberOfRequests < 15)
            {
                Requests.Add(request);
                numberOfRequests++;
                return true;
            }
            return false;
        }


    }
}
