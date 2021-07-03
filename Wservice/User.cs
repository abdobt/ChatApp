using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wservice
{
    public class User
    {
        private int id;
        private string username;
        private Boolean connected;
        public User(int id,string un,Boolean cn)
        {
            this.Id = id;
            this.Username = un;
            this.Connected = cn;
        }
        public User()
        {

        }
        public int Id { get => id; set => id = value; }
        public string Username { get => username; set => username = value; }
        public bool Connected { get => connected; set => connected = value; }
    }
}
