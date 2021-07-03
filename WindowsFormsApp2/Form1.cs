using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private  int a=0;
        private int id_c;
        private string username ;
        private DateTime dt;
        private DateTime d2;
        Boolean b = false;
        List<UserControl2> ul;
        int u = 0;
        public Form1(int id, string username)
        {
            this.id_c = id;
            this.username = username;
            InitializeComponent();
            var cs = "Host=localhost;Username=postgres;Password=;Database=chatapp";

             var con = new NpgsqlConnection(cs);
            con.Open();

            string sql7 = "SELECT * FROM users where id!=@id";
            var cmd7 = new NpgsqlCommand(sql7, con);
            cmd7.Parameters.AddWithValue("id", this.id_c);
            NpgsqlDataReader rdr7 = cmd7.ExecuteReader();
            ul = new List<UserControl2>();
            while (rdr7.Read())
            {
                UserControl2 uc = new UserControl2(rdr7.GetString(1), rdr7.GetBoolean(2));
                ul.Add(uc);
            }
            rdr7.Close();
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
            foreach (UserControl2 l in ul)
            {
                this.tableLayoutPanel1.Controls.Add(l, 1, this.tableLayoutPanel1.RowCount);
                this.tableLayoutPanel1.RowCount = this.tableLayoutPanel1.RowCount + 1;
                this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            }
            this.tableLayoutPanel1.AutoScroll = true;
            //this.tableLayoutPanel1.HorizontalScroll.Enabled = false;
          //  this.tableLayoutPanel1.HorizontalScroll.Visible = false;
           // Message m0 = new Message("abdo1", new DateTime(2021, 6, 25, 16, 35, 0), "hello message 0");
            dt = lm[0].Time;
            Console.WriteLine(lm.Count);
            foreach(Message  m in lm)
            {
                string sql2 = "SELECT * FROM users where id=@id";
                var cmd2 = new NpgsqlCommand(sql2, con);
                cmd2.Parameters.AddWithValue("id", m.Id_user);

                NpgsqlDataReader rdr2 = cmd2.ExecuteReader();
                while (rdr2.Read())
                {
                    m.Username = rdr2.GetString(1);
                }
                rdr2.Close();
                if (dt.Date<=m.Time.Date)
                {
                    Label l = new Label();
                    if (m.Time.Date == DateTime.Now.Date)
                        l.Text = "Ajourd'hui";
                    else if(m.Time.Date == DateTime.Now.AddDays(-1).Date)
                        l.Text = "Hier";
                    else
                    l.Text = m.Time.ToString("yyyy-MM-dd");
                    l.ForeColor = Color.Gray;
                    l.Top = a + 40;
                    l.Left = 300;
                    //  this.a += l.Height + 10;
                    // this.panel1.Controls.Add(l);
                    this.tableLayoutPanel2.Controls.Add(l, 1, this.tableLayoutPanel2.RowCount);
                    this.tableLayoutPanel2.RowCount = this.tableLayoutPanel2.RowCount + 1;
                    this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
                    dt =m.Time.AddDays(1);
                }
                if (!m.Username.Equals(this.username))
                   addmessage(m.Msg, m.Time, m.Username);
                else
                   adduseroldmessage(m.Msg, m.Time);
            }
            d2 = DateTime.Now;
            Timer timer1 = new Timer
            {
                Interval = 2000
            };
            timer1.Enabled = true;
            timer1.Tick += new System.EventHandler(OnTimerEvent);
            this.tableLayoutPanel2.VerticalScroll.Value = this.tableLayoutPanel2.VerticalScroll.Maximum;
        }
        private void OnTimerEvent(object sender, EventArgs e)
        {
            var cs = "Host=localhost;Username=postgres;Password=;Database=chatapp";

            var con = new NpgsqlConnection(cs);
            con.Open();
            string sql7 = "SELECT * FROM users where id!=@id";
            var cmd7 = new NpgsqlCommand(sql7, con);
            cmd7.Parameters.AddWithValue("id", this.id_c);
            NpgsqlDataReader rdr7 = cmd7.ExecuteReader();
            List<UserControl2>  uls = new List<UserControl2>();
            while (rdr7.Read())
            {
                UserControl2 uc = new UserControl2(rdr7.GetString(1), rdr7.GetBoolean(2));
                uls.Add(uc);
            }
            rdr7.Close();
            Boolean changed = false;
                foreach (UserControl2 l in uls)
            {
                int s = 0;
                foreach (UserControl2 d in ul)
                {
                    if (d.Connected != l.Connected && d.Uname==l.Uname)
                    {
                        if (d.Uname.Equals(l.Uname))
                            Console.WriteLine("equals");
                        Console.WriteLine("different");
                        // this.tableLayoutPanel1.Controls.Clear();
                        //  changed = true;
                       this.tableLayoutPanel1.Controls.Add(l, 1, s +1);
                        Console.WriteLine("index : " + s);
                        this.tableLayoutPanel1.Controls.RemoveAt(s);
                        if (l.Connected != true)
                        {
                            Label le = new Label();
                            le.Text = l.Uname + " a quitté";
                            this.tableLayoutPanel2.Controls.Add(le, 1, this.tableLayoutPanel2.RowCount);
                            this.tableLayoutPanel2.RowCount = this.tableLayoutPanel2.RowCount + 1;
                            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
                            this.tableLayoutPanel2.VerticalScroll.Value = this.tableLayoutPanel2.VerticalScroll.Maximum;
                        }
                    }
                    s++;
                }
                
            }
            this.ul = uls;
           /* if (changed)
            {
                foreach (UserControl2 l in ul)
                {
                    this.tableLayoutPanel1.Controls.Add(l, 1, this.tableLayoutPanel1.RowCount);
                    this.tableLayoutPanel1.RowCount = this.tableLayoutPanel1.RowCount + 1;
                    this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
                }
            }             */  
            string sql2 = "SELECT * FROM messages where id_user!=@id and tm between @d1 and @d2 ";
            var cmd2 = new NpgsqlCommand(sql2, con);
            cmd2.Parameters.AddWithValue("id", this.id_c);
            cmd2.Parameters.AddWithValue("d2",DateTime.Now);
            cmd2.Parameters.AddWithValue("d1", this.d2);
            NpgsqlDataReader rdr2 = cmd2.ExecuteReader();
            List<Message> lm = new List<Message>();
            while (rdr2.Read())
            {
                Message m0 = new Message(rdr2.GetInt32(3), rdr2.GetDateTime(2), rdr2.GetString(1));
                lm.Add(m0);
            }
            this.d2 = DateTime.Now;
            rdr2.Close();
            foreach (Message m in lm)
            {
                string sql3 = "SELECT * FROM users where id=@id";
                var cmd3 = new NpgsqlCommand(sql3, con);
                cmd3.Parameters.AddWithValue("id", m.Id_user);
                NpgsqlDataReader rdr3 = cmd3.ExecuteReader();
                while (rdr2.Read())
                {
                    m.Username = rdr2.GetString(1);
                }
                rdr2.Close();
                addmessage(m.Msg, m.Time, m.Username);
            }
            con.Close();
            }
        private void button1_Click(object sender, EventArgs e)
        {
            addusermessages();
            this.textBox1.Text = "";
        }
        private void panel1_ControlAdded(object sender, ControlEventArgs e)
        {
            this.panel1.ScrollControlIntoView(e.Control);
        }
        private void addmessage(String ms,DateTime dt,String uname)
        {
            UserControl1 uc1 = new UserControl1(ms, dt.ToString("HH:mm"), uname, Color.White);
            this.panel1.Controls.Add(uc1);
            /* if (this.a > 500)
             {
                uc1.Top = a + 25;
               this.a += uc1.Height;
             }
             else
             {
                 uc1.Top = 20 + a;
                 this.a += uc1.Height;
             }
             uc1.Left =0;*/
            this.tableLayoutPanel2.Controls.Add(uc1, 0, this.tableLayoutPanel2.RowCount);
            this.tableLayoutPanel2.RowCount = this.tableLayoutPanel2.RowCount + 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.VerticalScroll.Value = this.tableLayoutPanel2.VerticalScroll.Maximum;
        }
        private  void addusermessages()
        {
            var cs = "Host=localhost;Username=postgres;Password=;Database=chatapp";

            var con = new NpgsqlConnection(cs);
            con.Open();

            var sql = "INSERT INTO messages(message,tm,id_user) VALUES(@msg, @time,@id)";
            var cmd = new NpgsqlCommand(sql, con);
            DateTime d = DateTime.Now;
            cmd.Parameters.AddWithValue("msg", this.textBox1.Text);
            cmd.Parameters.AddWithValue("time", d);
            cmd.Parameters.AddWithValue("id", this.id_c);
            cmd.Prepare();

            cmd.ExecuteNonQuery();
            con.Close();
            Console.WriteLine("row inserted");
            UserControl1 uc1 = new UserControl1(this.textBox1.Text,d.ToString("HH:mm"), this.username, Color.LightBlue);
            this.panel1.Controls.Add(uc1);
            /* Console.WriteLine("a : " + a);
             if (this.a > 500)
             {
                 uc1.Top = a + 25;
                // this.a += uc1.Height;
             }
             else
             {
                 uc1.Top = 15 + a;
                 this.a += uc1.Height;
             }
             uc1.Left = 300;*/
            this.tableLayoutPanel2.Controls.Add(uc1, 2, this.tableLayoutPanel2.RowCount);
            this.tableLayoutPanel2.RowCount = this.tableLayoutPanel2.RowCount + 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.VerticalScroll.Value = this.tableLayoutPanel2.VerticalScroll.Maximum;
        }
        private void adduseroldmessage(String ms, DateTime dt)
        {
            UserControl1 uc1 = new UserControl1(ms, dt.ToString("HH:mm"), this.username, Color.LightBlue);
            /*  this.panel1.Controls.Add(uc1);
              if (a > 500)
              {
                  uc1.Top = a + 25;
                  this.a += uc1.Height;
              }
              else
              {
                  uc1.Top = 20 + a;
                  this.a += uc1.Height;
              }
              uc1.Left = 300;*/
            this.tableLayoutPanel2.Controls.Add(uc1,2, this.tableLayoutPanel2.RowCount);
            this.tableLayoutPanel2.RowCount = this.tableLayoutPanel2.RowCount + 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.VerticalScroll.Value = this.tableLayoutPanel2.VerticalScroll.Maximum;
        }

        private void splitter1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosed(Object sender, FormClosedEventArgs e)
        {

           Console.WriteLine("closed");
            var cs = "Host=localhost;Username=postgres;Password=;Database=chatapp";
            var con = new NpgsqlConnection(cs);
            con.Open();
            string sql2 = "Update users set connected=false where id=@id";
            var cmd2 = new NpgsqlCommand(sql2, con);
            cmd2.Parameters.AddWithValue("id", id_c);
            cmd2.ExecuteScalar();
            con.Close();
            Environment.Exit(0);
        }
    }
}
