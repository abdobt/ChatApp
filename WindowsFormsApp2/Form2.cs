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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var cs = "Host=localhost;Username=postgres;Password=;Database=chatapp";

            var con = new NpgsqlConnection(cs);
            con.Open();

            string sql = "SELECT * FROM users where username=@un";
            var cmd = new NpgsqlCommand(sql, con);
            cmd.Parameters.AddWithValue("un", this.textBox1.Text);
            NpgsqlDataReader rdr = cmd.ExecuteReader();
            String username = "";
            int id = 0;
            if(rdr.HasRows)
            {
                while (rdr.Read())
                {
                    id = rdr.GetInt32(0);
                    username = rdr.GetString(1);
                }
                rdr.Close();
                string sql2 = "Update users set connected=true where id=@id";
                var cmd2 = new NpgsqlCommand(sql2, con);
                cmd2.Parameters.AddWithValue("id", id);
                cmd2.ExecuteScalar();
                Form1 f = new Form1(id, username);
                f.Visible = true;
                this.Hide();
            }
            else
            {
                MessageBox.Show("Utilisateur n'existe pas", "Erreur",
    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
