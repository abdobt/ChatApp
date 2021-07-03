using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    class Message
    {
        private String username;
        private DateTime time;
        private int id_user;
        private string msg;
        public Message (string username,DateTime dt, string m)
        {
            this.Username = username;
            this.time = dt;
            this.msg = m;
        }
        public Message(int id_s, DateTime dt, string m)
        {
            this.id_user = id_s;
            this.time = dt;
            this.msg = m;
        }
        public DateTime Time { get => time; set => time = value; }
        public string Msg { get => msg; set => msg = value; }
        public string Username { get => username; set => username = value; }
        public int Id_user { get => id_user; set => id_user = value; }
    }
}
