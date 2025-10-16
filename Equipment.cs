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
using System.Xml.Linq;

namespace EQUIBORROW
{
    public partial class Equipment : Form
    {
        string userRole;
        public Equipment(string role)
        {
            InitializeComponent();
            userRole = role;

            searchBox.Text = "Search by ID, Name or Type";
            searchBox.ForeColor = Color.Gray;

            // Wire up events
            searchBox.Enter += searchBox_Enter;
            searchBox.Leave += searchBox_Leave;
            searchBox.TextChanged += searchBox_TextChanged;

        }
        string constr = "Data Source=.;Initial Catalog=EquiBorrowDB;Integrated Security=True;TrustServerCertificate=True";
        private void RegisterBtn_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(constr);
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            string query = "INSERT INTO Equipment VALUES(@idf,@name,@descr,@typ,@dat)";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("@idf", identBox.Text.Trim());
            cmd.Parameters.AddWithValue("@name",NameBox.Text.Trim());
            cmd.Parameters.AddWithValue("@descr", DescriptionBox.Text.Trim());
            cmd.Parameters.AddWithValue("@typ", TypeCombo.Text);
            cmd.Parameters.AddWithValue("@dat", regDate.Value);
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show(identBox.Text.Trim()+" is saved successfully");
            DisplayData();

        }

        private void Equipment_Load(object sender, EventArgs e)
        {
            DisplayData();
            if (userRole == "User")
            {
                // Hide admin-only features
                DeleteBtn.Visible = false;
                UpdateBtn.Visible = false;
                ReportBtn.Visible = false;
                RegisterBtn.Visible = false;
            }

            // Optional: show role somewhere
            this.Text = "Equipment - Logged in as " + userRole;



        }

        private void DisplayData()
        {
            SqlConnection con = new SqlConnection(constr);
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            string query = "SELECT * FROM Equipment";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataSet ds = new DataSet();
            sda.Fill(ds, "Equipment");
            EquipmentView.DataSource = ds.Tables["Equipment"];
            con.Close();
        }

        private void UpdateBtn_Click(object sender, EventArgs e)
        {
           
            string id = EquipmentView.SelectedRows[0].Cells[0].Value.ToString();
            SqlConnection con = new SqlConnection(constr);
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            string query = "UPDATE Equipment SET identification = @idf,name = @name, description = @descr, type = @type, regDate = @date WHERE identification = @id";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@idf", identBox.Text.Trim());
            cmd.Parameters.AddWithValue("@name", NameBox.Text.Trim());
            cmd.Parameters.AddWithValue("@descr", DescriptionBox.Text.Trim());
            cmd.Parameters.AddWithValue("@type", TypeCombo.Text.Trim());
            cmd.Parameters.AddWithValue("@date", regDate.Text.Trim());
            cmd.Parameters.AddWithValue("@id", id);

            int rowsAffected = cmd.ExecuteNonQuery();
            con.Close();

            if (rowsAffected > 0)
            {
                MessageBox.Show("Record updated successfully!");
                DisplayData();
            }
            else
            {
                MessageBox.Show("No matching record found to update.");
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
           
            if (EquipmentView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a record to delete.");
                return;
            }

            string id = EquipmentView.SelectedRows[0].Cells[0].Value.ToString();

            DialogResult result = MessageBox.Show("Are you sure you want to delete this record?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                SqlConnection con = new SqlConnection(constr);
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                string query = "DELETE FROM Equipment WHERE identification = @id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);

                int rowsAffected = cmd.ExecuteNonQuery();
                con.Close();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Record deleted successfully!");
                    DisplayData();
                }
                else
                {
                    MessageBox.Show("No matching record found to delete.");
                }
            }
        }

        private void SearchBtn_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(constr);
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            string keyword = searchBox.Text.Trim();
            string query = "SELECT * FROM Equipment WHERE identification LIKE @kw OR name LIKE @kw OR type LIKE @kw";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds, "Equipment");
            EquipmentView.DataSource = ds.Tables["Equipment"];
            con.Close();


        }
        private void searchBox_Enter(object sender, EventArgs e)
        {
            if (searchBox.Text == "Search by ID, Name or Type")
            {
                searchBox.Text = "";
                searchBox.ForeColor = Color.Black;
            }
        }

        private void searchBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchBox.Text))
            {
                searchBox.Text = "Search by ID, Name or Type";
                searchBox.ForeColor = Color.Gray;
            }
        }


        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            
            if (searchBox.Text == "Search by ID, Name or Type")
                return;

            SqlConnection con = new SqlConnection(constr);
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            string keyword = searchBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                DisplayData(); // Show all records if search is empty
                return;
            }

            string query = "SELECT * FROM Equipment WHERE identification LIKE @kw OR name LIKE @kw OR type LIKE @kw";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds, "Equipment");
            EquipmentView.DataSource = ds.Tables["Equipment"];
            con.Close();
        }

        private void ReportBtn_Click_1(object sender, EventArgs e)
        {
            ReportForm rf = new ReportForm();
            rf.Show();
        }
    }
}


