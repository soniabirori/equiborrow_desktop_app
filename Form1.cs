using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EQUIBORROW
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

       


        private void registerLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RegisterForm rf = new RegisterForm();
            rf.Show();
            this.Hide(); // Optional: hide login while registering

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void LoginBtn_Click(object sender, EventArgs e)
      
        {
            string email = emailBox.Text.Trim();
            string password = pwdBox.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Boss, Email and Password can't be empty");
                return;
            }

            string constr = "Data Source=.;Initial Catalog=EquiBorrowDB;Integrated Security=True;TrustServerCertificate=True";
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();

                string query = "SELECT Role FROM Users WHERE Email = @email AND Password = @pwd";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@pwd", password);

                object roleObj = cmd.ExecuteScalar();

                if (roleObj != null)
                {
                    string role = roleObj.ToString();
                    MessageBox.Show("Welcome " + role + "!", "Login Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Equipment eqt = new Equipment(role);
                    eqt.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Boss, incorrect Email or Password", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}


