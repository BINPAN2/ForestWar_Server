using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model
{
    class User
    {
        public User(int id,string username,string password)
        {
            this.ID = id;
            this.Username = username;
            this.Password = password;
        }

        public int ID { set; get; }
        public string Username { set; get; }
        public string Password { set; get; }
    }
}
