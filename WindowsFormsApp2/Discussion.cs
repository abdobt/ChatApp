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
    public partial class Discussion : Form
    {
        private  int a=0;
        private int id_c;
        private string username ;
        private DateTime dt;
        private DateTime d2;
        Boolean b = false;
        List<UserControl2> ul;
        int u = 0;
        srv.WebService1SoapClient chatsrv = new srv.WebService1SoapClient();
        public Discussion(int id, string username)
        {
            this.id_c = id;
            this.username = username;
            InitializeComponent();
            ul = new List<UserControl2>();
            //Ajout de la liste des utilisateurs 
            srv.User[] list = chatsrv.getparticipants(this.id_c);
            foreach(srv.User u in list)
            {
                UserControl2 uc = new UserControl2(u.Username, u.Connected);
                ul.Add(uc);
                this.tableLayoutPanel1.Controls.Add(uc, 1, this.tableLayoutPanel1.RowCount);
                this.tableLayoutPanel1.RowCount = this.tableLayoutPanel1.RowCount + 1;
                this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            }
         
            srv.Message[] lm = chatsrv.getmessages();
            this.tableLayoutPanel1.AutoScroll = true;
            dt = lm[0].Time;
            foreach(srv.Message m in lm)
            {
                m.Username = chatsrv.getuserbyid(m.Id_user).Username;
                if (dt.Date<=m.Time.Date)
                {
                    Label l = new Label();
                    if (m.Time.Date == DateTime.Now.Date)
                        l.Text = "Ajourd'hui";
                    else if(m.Time.Date == DateTime.Now.AddDays(-1).Date)
                        l.Text = "Hier";
                    else
                    l.Text = m.Time.ToString("yyyy-MM-dd");
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
            List<UserControl2> uls = new List<UserControl2>();
            srv.User[] list = chatsrv.getparticipants(this.id_c);
            foreach (srv.User u in list)
            {
                UserControl2 uc = new UserControl2(u.Username, u.Connected);
                uls.Add(uc);
            }
            if (ul.Count!=uls.Count)
            {
                if (ul.Count > uls.Count)
                {
                    foreach (UserControl2 uc2 in ul)
                    {
                        Boolean exist = false;
                        foreach (UserControl2 uc in uls)
                        {
                            if (uc2.Uname.Equals(uc.Uname))
                            {
                                exist = true;
                                break;
                            }
                        }
                        if (!exist)
                        {
                            Label le = new Label();
                            le.Text = uc2.Uname + " a quitté";
                            this.tableLayoutPanel2.Controls.Add(le, 1, this.tableLayoutPanel2.RowCount);
                            this.tableLayoutPanel2.RowCount = this.tableLayoutPanel2.RowCount + 1;
                            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
                            this.tableLayoutPanel2.VerticalScroll.Value = this.tableLayoutPanel2.VerticalScroll.Maximum;
                        }
                    }
                }
                    if (ul.Count < uls.Count)
                    {
                    foreach (UserControl2 uc2 in uls)
                    {
                        Boolean exist = false;
                        foreach (UserControl2 uc in ul)
                        {
                            if (uc2.Uname.Equals(uc.Uname))
                            {
                                exist = true;
                                break;
                            }
                        }
                        if (!exist)
                        {
                            Label le = new Label();
                            le.Text = uc2.Uname + " participe";
                            this.tableLayoutPanel2.Controls.Add(le, 1, this.tableLayoutPanel2.RowCount);
                            this.tableLayoutPanel2.RowCount = this.tableLayoutPanel2.RowCount + 1;
                            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
                            this.tableLayoutPanel2.VerticalScroll.Value = this.tableLayoutPanel2.VerticalScroll.Maximum;
                        }
                    }
                    }
                this.tableLayoutPanel1.Controls.Clear();
                 foreach (UserControl2 l in uls)
                 {
                     this.tableLayoutPanel1.Controls.Add(l, 1, this.tableLayoutPanel1.RowCount);
                     this.tableLayoutPanel1.RowCount = this.tableLayoutPanel1.RowCount + 1;
                     this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
                 }
            }             
            this.ul = uls;
            srv.Message[] lm = chatsrv.messagesbetweentwodates(this.id_c, this.d2);
            this.d2 = DateTime.Now;
            foreach (srv.Message m in lm)
            {
                m.Username = chatsrv.getuserbyid(m.Id_user).Username;
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
            this.tableLayoutPanel2.Controls.Add(uc1, 0, this.tableLayoutPanel2.RowCount);
            this.tableLayoutPanel2.RowCount = this.tableLayoutPanel2.RowCount + 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.VerticalScroll.Value = this.tableLayoutPanel2.VerticalScroll.Maximum;
        }
        private  void addusermessages()
        {
            DateTime d = DateTime.Now;
            chatsrv.addmessage(this.textBox1.Text, this.id_c, d);
            UserControl1 uc1 = new UserControl1(this.textBox1.Text,d.ToString("HH:mm"), this.username, Color.LightBlue);
            this.panel1.Controls.Add(uc1);
            this.tableLayoutPanel2.Controls.Add(uc1, 2, this.tableLayoutPanel2.RowCount);
            this.tableLayoutPanel2.RowCount = this.tableLayoutPanel2.RowCount + 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.VerticalScroll.Value = this.tableLayoutPanel2.VerticalScroll.Maximum;
        }
        private void adduseroldmessage(String ms, DateTime dt)
        {
            UserControl1 uc1 = new UserControl1(ms, dt.ToString("HH:mm"), this.username, Color.LightBlue);
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
            chatsrv.quitter(this.id_c);
            Environment.Exit(0);
        }
    }
}
