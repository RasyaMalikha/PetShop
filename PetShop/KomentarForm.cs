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
    public partial class KomentarForm : Form
    {

        private DataGridView BillDGV; // Variabel untuk menyimpan referensi BillDGV
        SqlConnection Con = new SqlConnection(@"Data Source=HP-EC0XXX;Initial Catalog=PetShop;Integrated Security=True;TrustServerCertificate=True");

        public KomentarForm(DataGridView BillDGV)
        {
            InitializeComponent();
            this.BillDGV = BillDGV; // Simpan referensi BillDGV yang diteruskan
        }

        public string GetKomentar()
        {
            return KomenTb.Text;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void BatalButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Ambil nilai dari textbox komentar
            string komentar = KomenTb.Text;

            // Looping melalui semua data di DataGridView
            foreach (DataGridViewRow row in BillDGV.Rows)
            {
                string productName = row.Cells["PrName"].Value.ToString();
                int quantity = Convert.ToInt32(row.Cells["PrQty"].Value);

                // Salin data ke tabel Hold
                using (SqlConnection connection = new SqlConnection("Data Source=HP-EC0XXX;Initial Catalog=PetShop;Integrated Security=True;TrustServerCertificate=True"))
                {
                    connection.Open();

                    string sqlInsert = "INSERT INTO HoldTbl (PrName, PrQty, Komentar) VALUES (@ProductName, @Quantity, @Komentar);";

                    using (SqlCommand command = new SqlCommand(sqlInsert, connection))
                    {
                        command.Parameters.AddWithValue("@ProductName", productName);
                        command.Parameters.AddWithValue("@Quantity", quantity);
                        command.Parameters.AddWithValue("@Komentar", komentar);

                        command.ExecuteNonQuery();
                    }
                }

                // Hapus item dari ProductBillTbl
                using (SqlConnection connection = new SqlConnection("Data Source=HP-EC0XXX;Initial Catalog=PetShop;Integrated Security=True;TrustServerCertificate=True"))
                {
                    connection.Open();

                    string sqlDelete = "DELETE FROM ProductBillTbl2 WHERE PrName = @ProductName AND PrQty = @Quantity;";

                    using (SqlCommand command = new SqlCommand(sqlDelete, connection))
                    {
                        command.Parameters.AddWithValue("@ProductName", productName);
                        command.Parameters.AddWithValue("@Quantity", quantity);

                        command.ExecuteNonQuery();
                    }
                }
            }

            // Setelah semua operasi selesai, refresh DataGridView jika perlu
            BillDGV.DataSource = null; // Menghapus sumber data
                                       // Lakukan pengisian ulang dengan data terbaru dari database jika diperlukan
        }



        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
