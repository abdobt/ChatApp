using Npgsql;
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
    public partial class authentification : Form
    {
        srv.WebService1SoapClient chatsrv = new srv.WebService1SoapClient();
        public authentification()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            srv.User a = chatsrv.Participer(this.textBox1.Text);
            if(a==null)
                MessageBox.Show("Nom d'utilisateur déjà utilisé !", "Erreur",
    MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                    {
                        Discussion f = new Discussion(a.Id, a.Username);
                        f.Visible = true;
                        this.Hide();
                    }
          
        }

    }
}
