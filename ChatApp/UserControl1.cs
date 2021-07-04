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
    public partial class UserControl1 : UserControl
    {
        public UserControl1(String message,String time,String username,Color c)
        {
            InitializeComponent(message);
            this.label1.Text = time;
            this.label3.Text = username;
            this.panel1.BackColor = c;
        }
    }
}
