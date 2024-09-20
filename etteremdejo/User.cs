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

        public User(string username, string password, string email, string role)
        {
            this.Username = username;
            this.Password = password;
            this.Email = email;
            this.Role = role;
        }

        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string Email { get => email; set => email = value; }
        public string Role { get => role; set => role = value; }
    }
}
