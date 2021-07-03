using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Wservice
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
        String cs = "Host=localhost;Username=postgres;Password=;Database=chatapp";
        NpgsqlConnection con;
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        [WebMethod]
        public User Participer(String usname)
        {
            con = new NpgsqlConnection(cs);
            con.Open();

            string sql = "SELECT * FROM users where username=@un";
            var cmd = new NpgsqlCommand(sql, con);
            cmd.Parameters.AddWithValue("un", usname);
            NpgsqlDataReader rdr = cmd.ExecuteReader();
            String username = "";
            int id = 0;
            Boolean connected = false;
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    id = rdr.GetInt32(0);
                    username = rdr.GetString(1);
                    connected = rdr.GetBoolean(2);
                }
                rdr.Close();
                if(connected)
                {
                    return null;
                }
                else
                {
                    string sql2 = "Update users set connected=true where id=@id";
                    var cmd2 = new NpgsqlCommand(sql2, con);
                    cmd2.Parameters.AddWithValue("id", id);
                    cmd2.ExecuteScalar();
                }
                
            }
            else
            {
                rdr.Close();
                string sql2 = "Insert into users(id,username,connected) values(@un,true)";
                var cmd2 = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("un", usname);
                string sql3 = "Select max(id) from users";
                var cmd3 = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("un", usname);
                cmd.ExecuteNonQuery();
                id = (int)cmd.ExecuteScalar();
                username = usname;
                con.Close();
            }
            return new User(id, username,true);
        }
        [WebMethod]
        public List<User> getparticipants(int id)
        {
            con = new NpgsqlConnection(cs);
            con.Open();

            string sql7 = "SELECT * FROM users where id!=@id and connected=true";
            var cmd7 = new NpgsqlCommand(sql7, con);
            cmd7.Parameters.AddWithValue("id", id);
            NpgsqlDataReader rdr7 = cmd7.ExecuteReader();
            List<User> lu = new List<User>();
            while (rdr7.Read())
            {
                User u = new User(rdr7.GetInt32(0),rdr7.GetString(1), rdr7.GetBoolean(2));
                lu.Add(u);
            }
            rdr7.Close();
            con.Close();
            return lu;
        }
        [WebMethod]
        public List<Message> getmessages()
        {
            con = new NpgsqlConnection(cs);
            con.Open();
            string sql = "SELECT * FROM messages";
            var cmd = new NpgsqlCommand(sql, con);

            NpgsqlDataReader rdr = cmd.ExecuteReader();
            List<Message> lm = new List<Message>();
            while (rdr.Read())
            {
                Message m0 = new Message(rdr.GetInt32(3), rdr.GetDateTime(2), rdr.GetString(1));
                lm.Add(m0);
            }
            rdr.Close();
            con.Close();
            return lm;
        }
        [WebMethod]
        public  User getuserbyid(int id)
        {
             con = new NpgsqlConnection(cs);
            con.Open();
            string sql2 = "SELECT * FROM users where id=@id";
            var cmd2 = new NpgsqlCommand(sql2, con);
            cmd2.Parameters.AddWithValue("id", id);

            NpgsqlDataReader rdr2 = cmd2.ExecuteReader();
            User u = null;
            while (rdr2.Read())
            {
                u = new User(id,rdr2.GetString(1),rdr2.GetBoolean(2));
            }
            rdr2.Close();
            con.Close();
            return u;
        }
        [WebMethod]
        public void quitter(int id)
        {
            con = new NpgsqlConnection(cs);
            con.Open();
            string sql2 = "Update users set connected=false where id=@id";
            var cmd2 = new NpgsqlCommand(sql2, con);
            cmd2.Parameters.AddWithValue("id", id);
            cmd2.ExecuteScalar();
            con.Close();
        }
        [WebMethod]
        public List<Message> messagesbetweentwodates(int id , DateTime d)
        {
           con = new NpgsqlConnection(cs);
            con.Open();
            string sql2 = "SELECT * FROM messages where id_user!=@id and tm between @d1 and @d2 ";
            var cmd2 = new NpgsqlCommand(sql2, con);
            cmd2.Parameters.AddWithValue("id", id);
            cmd2.Parameters.AddWithValue("d2", DateTime.Now);
            cmd2.Parameters.AddWithValue("d1",d);
            NpgsqlDataReader rdr2 = cmd2.ExecuteReader();
            List<Message> lm = new List<Message>();
            while (rdr2.Read())
            {
                Message m0 = new Message(rdr2.GetInt32(3), rdr2.GetDateTime(2), rdr2.GetString(1));
                lm.Add(m0);
            }
            rdr2.Close();
            con.Close();
            return lm;
        }
        [WebMethod]
        public void addmessage(String text,int id,DateTime d)
        {
            var cs = "Host=localhost;Username=postgres;Password=;Database=chatapp";

             con = new NpgsqlConnection(cs);
            con.Open();

            var sql = "INSERT INTO messages(message,tm,id_user) VALUES(@msg, @time,@id)";
            var cmd = new NpgsqlCommand(sql, con);
            cmd.Parameters.AddWithValue("msg", text);
            cmd.Parameters.AddWithValue("time", d);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}
