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
    public partial class Category : Form
    {
        public Category()
        {
            InitializeComponent();
            DisplayCategory();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=HP-EC0XXX;Initial Catalog=PetShop;Integrated Security=True;TrustServerCertificate=True");

        private void label6_Click(object sender, EventArgs e)
        {
            Login Obj = new Login();
            Obj.Show();
            this.Hide();
        }
        private void DisplayCategory()
        {
            Con.Open();
            string Query = "Select * from CategoryTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            CategoryDGV.DataSource = ds.Tables[0]; //pstikan menggunakan datagridview supaya kebaca menggunakan datasource
            Con.Close();

        }
        private void Clear()
        {
            CatNameTb.Text = "";
          
        }
        int Key = 0;
        private void CategoryDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Pastikan bahwa event dipicu oleh sel yang memiliki data, bukan header atau baris kosong
            if (e.RowIndex >= 0)
            {
                // Mendapatkan nilai dari sel yang diklik
                DataGridViewRow row = CategoryDGV.Rows[e.RowIndex];

                // Misalkan Anda memiliki kolom dengan nama EmpNum, Nama, Posisi, dll.
                // Gantilah sesuai dengan nama kolom yang sesuai di tabel Anda
                string EmpName = row.Cells["CatName"].Value.ToString();
             

                // Lanjutkan untuk kolom lainnya sesuai kebutuhan

                // Menetapkan nilai-nilai yang diperoleh ke kontrol di form Anda
                CatNameTb.Text = EmpName;
     


                if (CatNameTb.Text == "")
                {
                    Key = 0;
                }
                // Lanjutkan untuk kontrol lainnya sesuai kebutuhan
            }
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {

            if (CatNameTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO CategoryTbl (CatName) VALUES (@CN)", Con);
                    cmd.Parameters.AddWithValue("@CN", CatNameTb.Text);
                  

                    cmd.ExecuteNonQuery(); //sebelumnya error pakai executenon query, setelah diihapus-run-undo lagi-run, berhasil
                    MessageBox.Show("Customer Added");
                    Con.Close();
                    DisplayCategory();
                    Clear();

                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (CategoryDGV.SelectedRows.Count > 0)
            {
                // Ambil ID produk dari baris terpilih
                int categoryid = Convert.ToInt32(CategoryDGV.SelectedRows[0].Cells["CatId"].Value);

                // Ambil data dari elemen UI
                string cateName = CatNameTb.Text;
              
        
              
                // Buat string koneksi dan query SQL
                string connectionString = "Data Source=HP-EC0XXX;Initial Catalog=PetShop;Integrated Security=True";
                string query = "UPDATE CategoryTbl SET CatName = @categoryName WHERE CatId = @CategoryId";

                // Gunakan koneksi SQL untuk melakukan update
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@categoryName", cateName);
                        command.Parameters.AddWithValue("@CategoryId", categoryid);


                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Product information updated successfully.");
                            DisplayCategory();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update product information.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a product to update.");
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (CategoryDGV.SelectedRows.Count > 0)
            {
                int customerId = Convert.ToInt32(CategoryDGV.SelectedRows[0].Cells["CatId"].Value);
                string query = "DELETE FROM CategoryTbl WHERE CatId = @CategoryId";

                // Sediakan string koneksi Anda di sini
                string connectionString = "Data Source=HP-EC0XXX;Initial Catalog=PetShop;Integrated Security=True;Encrypt=True; TrustServerCertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@CategoryId", customerId);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Data berhasil dihapus.");
                            Clear();
                            DisplayCategory();
                        }
                        else
                        {
                            MessageBox.Show("Gagal menghapus data.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Terjadi kesalahan: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Silakan pilih produk untuk dihapus.");
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

            Billings Obj = new Billings();
            Obj.Show();
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Homes Obj = new Homes();
            Obj.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Products Obj = new Products();
            Obj.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Employeess Obj = new Employeess();
            Obj.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Customers Obj = new Customers();
            Obj.Show();
            this.Hide();
        }
    }
}
