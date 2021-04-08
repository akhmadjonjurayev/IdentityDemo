using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityDemo.Shared
{
    public class UserManagerResponce
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public DateTime? ExireDate { get; set; }
    }
    public static class Role
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }
}
