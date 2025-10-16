using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace EQUIBORROW
{
    public partial class RegisterForm : Form
    {
        string constr = "Data Source=.;Initial Catalog=EquiBorrowDB;Integrated Security=True;TrustServerCertificate=True";

        public RegisterForm()
        {
            InitializeComponent();
        }
        private void RegisterForm_Load(object sender, EventArgs e)
        {
            // Set Segoe UI font for all controls
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Font = new Font("Segoe UI", 10);
            }

            // Pre-fill role dropdown
            roleCombo.Items.Clear();
            roleCombo.Items.Add("Admin");
            roleCombo.Items.Add("User");
            roleCombo.SelectedIndex = 1; // Default to "User"
        }


        private void createBtn_Click(object sender, EventArgs e)
        {
            
        }

        private void backLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Form1 login = new Form1();
            login.Show();
        }

        private void createBtn_Click_1(object sender, EventArgs e)
        {
            string email = emailBox.Text.Trim();
            string password = pwdBox.Text.Trim();
            string role = roleCombo.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            SqlConnection con = new SqlConnection(constr);
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            // Check if email already exists
            string checkQuery = "SELECT COUNT(*) FROM Users WHERE Email = @email";
            SqlCommand checkCmd = new SqlCommand(checkQuery, con);
            checkCmd.Parameters.AddWithValue("@email", email);
            int exists = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (exists > 0)
            {
                MessageBox.Show("Email already registered.");
                con.Close();
                return;
            }

            // Insert new user
            string insertQuery = "INSERT INTO Users (Email, Password, Role) VALUES (@email, @pwd, @role)";
            SqlCommand cmd = new SqlCommand(insertQuery, con);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@pwd", password);
            cmd.Parameters.AddWithValue("@role", role);
            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Account created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Hide();
            Form1 login = new Form1();
            login.Show();

        }
    }
}
