using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class UserControl2 : UserControl
    {
        private Boolean connected;
        private string uname;
        public UserControl2(String name,Boolean active)
        {
            this.Uname = name;
            this.Connected = active;
            InitializeComponent();
            this.label1.Text = name;
        }

        public bool Connected { get => connected; set => connected = value; }
        public string Uname { get => uname; set => uname = value; }
        /*public void change_status(Boolean t)
        {
            if (t)
                this.pictureBox2.Image = global::WindowsFormsApp2.Properties.Resources._checked;
            else
                this.pictureBox2.Image = global::WindowsFormsApp2.Properties.Resources.remove;
        }*/
    }
}
