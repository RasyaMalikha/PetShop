using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PetShop
{
    public partial class Reports : Form
    {
        SqlConnection Con = new SqlConnection(@"Data Source=HP-EC0XXX;Initial Catalog=PetShop;Integrated Security=True;TrustServerCertificate=True");

        public Reports()
        {
            InitializeComponent();
            dtPicker1.Format = DateTimePickerFormat.Custom;
            dtPicker1.CustomFormat = "MMMM";
            dtPicker1.ShowUpDown = true;

            dtPicker2.Format = DateTimePickerFormat.Custom;
            dtPicker2.CustomFormat = "MMMM";
            dtPicker2.ShowUpDown = true;
        }

        private void generateDG()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Rows.Clear();
                dg.Refresh();
                dg.DataSource = dt;

                SqlCommand cmd = new SqlCommand("SELECT MONTH(PaymentDate) AS Bulan, SUM(PaymentPrice) AS Total, COUNT(*) AS NumberOfPayments FROM PaymentsTbl WHERE PaymentType IS NOT NULL AND MONTH(PaymentDate) BETWEEN @StartDate AND @EndDate GROUP BY MONTH(PaymentDate)", Con);
                cmd.Parameters.AddWithValue("@StartDate", dtPicker1.Value.Month);
                cmd.Parameters.AddWithValue("@EndDate", dtPicker2.Value.Month);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);

                dg.DataSource = dt;

                int i = 0;
                List<string> month = new List<string>();
                List<int> price = new List<int>();

                foreach (DataRow dtr in dt.Rows)
                {
                    month.Add(dtr[0].ToString());
                    price.Add(int.Parse(dtr[1].ToString()));
                    i += 1;
                }

                var resMonth = month.ToArray();
                var resPrice = price.ToArray();

                chart1.Series.Clear();
                chart1.Titles.Clear();

                chart1.Series.Add("Income");
                chart1.Titles.Add("Income in Million");

                for (int j = 0; j < resMonth.Length; j++)
                {
                    string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(int.Parse(resMonth[j]));
                    chart1.Series["Income"].Points.AddXY(monthName, (float)resPrice[j] / 1000000); // dibagi 1 juta, karena ditampilinnya dalam satuan juta kalo disoal
                }
            }
            catch (SqlException ex)
            {
                // Handle SQL exception
                MessageBox.Show("Maaf, data tidak ketemu" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenBtn_Click(object sender, EventArgs e)
        {
            if (int.Parse(dtPicker1.Value.ToString("MM")) <= int.Parse(dtPicker2.Value.ToString("MM")))
            {
                generateDG();
            }
            else
            {
                MessageBox.Show("Tidak ada data yang anda input di bulan tersebut");
            }
        }
    }
}
