using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace etteremdejo
{
    internal class User
    {
        string username,password,email,role;

        public User(string username, string email, string password, string role)
        {
            this.Username = username;
            this.Email = email;
            this.Password = password;
            this.Role = role;
        }

        public string Username { get => username; set => username = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public string Role { get => role; set => role = value; }
    }
}
