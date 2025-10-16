using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace EQUIBORROW
{
    public partial class ReportForm : Form
    {
        string constr = "Data Source=.;Initial Catalog=EquiBorrowDB;Integrated Security=True;TrustServerCertificate=True";

        public ReportForm()
        {
            InitializeComponent();
        }

        private void ReportForm_Load(object sender, EventArgs e)
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
            reportGrid.DataSource = ds.Tables["Equipment"];
            con.Close();
        }

        private void PrintBtn_Click(object sender, EventArgs e)
        {
            
            PrintDialog printDialog = new PrintDialog();
            PrintDocument printDocument = new PrintDocument();

            printDialog.Document = printDocument;
            printDocument.PrintPage += PrintDocument_PrintPage;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument.Print();
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Bitmap bmp = new Bitmap(reportGrid.Width, reportGrid.Height);
            reportGrid.DrawToBitmap(bmp, new Rectangle(0, 0, reportGrid.Width, reportGrid.Height));
            e.Graphics.DrawImage(bmp, 0, 0);
        }

    }
}

