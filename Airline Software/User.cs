using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airline_Software
{
    internal class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int Id { get; }

        public void ChangePassword(string newPassword)
        {
            Password = newPassword;
        }
    }
}
