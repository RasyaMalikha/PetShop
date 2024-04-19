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

namespace PetShop
{
    public partial class HoldForm : Form
    {
        public HoldForm()
        {
            InitializeComponent();
            DisplayCategory();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=HP-EC0XXX;Initial Catalog=PetShop;Integrated Security=True;TrustServerCertificate=True");

        private void DisplayCategory()
        {
            Con.Open();
            string Query = "Select * from HoldTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            HoldDGV.DataSource = ds.Tables[0]; //pstikan menggunakan datagridview supaya kebaca menggunakan datasource
            Con.Close();

        }

        private void HoldDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
