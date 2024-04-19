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
    public partial class Products : Form
    {
        public Products()
        {
            InitializeComponent();
            DisplayProduct();
            GetCategoryId();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=HP-EC0XXX;Initial Catalog=PetShop;Integrated Security=True;TrustServerCertificate=True"); 
        private void DisplayProduct()
        {
            Con.Open();
            string Query = "Select * from ProductTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            ProductDGV.DataSource = ds.Tables[0]; //pstikan menggunakan datagridview supaya kebaca menggunakan datasource
            Con.Close();

        }
        private void Clear()
        {
            PrNameTb.Text = "";
            CatCb.Text = "";
            QtyTb.Text = "";
            PriceTb.Text = "";

        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (PrNameTb.Text == "" || CatCb.Text == "" || QtyTb.Text == "" || PriceTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO ProductTbl (PrName,PrCat,PrQty, PrPrice) VALUES (@PN,@PC,@PQ, @PP)", Con);
                    cmd.Parameters.AddWithValue("@PN", PrNameTb.Text);
                    cmd.Parameters.AddWithValue("@PC", CatCb.Text);
                    cmd.Parameters.AddWithValue("@PQ", QtyTb.Text);
                    cmd.Parameters.AddWithValue("@PP", PriceTb.Text);
                    cmd.ExecuteNonQuery(); //sebelumnya error pakai executenon query, setelah diihapus-run-undo lagi-run, berhasil
                    MessageBox.Show("Product Added");
                    Con.Close();
                    
                    Clear();

                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        int Key = 0;
        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (ProductDGV.SelectedRows.Count > 0)
            {
                // Ambil ID produk dari baris terpilih
                int productId = Convert.ToInt32(ProductDGV.SelectedRows[0].Cells["PrId"].Value);

                // Ambil data dari elemen UI
                string newName = PrNameTb.Text;
                string newCategory = CatCb.Text;
                int newQuantity;
                decimal newPrice;

                // Validasi input untuk Quantity dan Price
                if (!int.TryParse(QtyTb.Text, out newQuantity))
                {
                    MessageBox.Show("Invalid quantity value.");
                    return;
                }
                if (!decimal.TryParse(PriceTb.Text, out newPrice))
                {
                    MessageBox.Show("Invalid price value.");
                    return;
                }

                // Buat string koneksi dan query SQL
                string connectionString = "Data Source=HP-EC0XXX;Initial Catalog=PetShop;Integrated Security=True";
                string query = "UPDATE ProductTbl SET PrName = @ProductName, PrCat = @Category, PrQty = @Quantity, PrPrice = @Price WHERE PrId = @ProductId";

                // Gunakan koneksi SQL untuk melakukan update
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@ProductName", newName);
                        command.Parameters.AddWithValue("@Category", newCategory);
                        command.Parameters.AddWithValue("@Quantity", newQuantity);
                        command.Parameters.AddWithValue("@Price", newPrice);
                        command.Parameters.AddWithValue("@ProductId", productId);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Product information updated successfully.");
                            DisplayProduct();
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

            if (ProductDGV.SelectedRows.Count > 0)
            {
                int productId = Convert.ToInt32(ProductDGV.SelectedRows[0].Cells["PrId"].Value);
                string query = "DELETE FROM ProductTbl WHERE PrId = @ProductId";

                // Sediakan string koneksi Anda di sini
                string connectionString = "Data Source=HP-EC0XXX;Initial Catalog=PetShop;Integrated Security=True;Encrypt=True; TrustServerCertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@ProductId", productId);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Data berhasil dihapus.");
                            Clear();
                            DisplayProduct();
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
            
        
        private void ProductDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Mendapatkan nilai dari sel yang diklik
                DataGridViewRow row = ProductDGV.Rows[e.RowIndex];

                // Misalkan Anda memiliki kolom dengan nama EmpNum, Nama, Posisi, dll.
                // Gantilah sesuai dengan nama kolom yang sesuai di tabel Anda
                string EmpName = row.Cells["PrName"].Value.ToString();
                string EmpAdd = row.Cells["PrCat"].Value.ToString();
                string EmpPhone = row.Cells["PrQty"].Value.ToString();
                string Price = row.Cells["PrPrice"].Value.ToString();

                // Lanjutkan untuk kolom lainnya sesuai kebutuhan

                // Menetapkan nilai-nilai yang diperoleh ke kontrol di form Anda
                PrNameTb.Text = EmpName;
                CatCb.Text = EmpAdd;
                QtyTb.Text = EmpPhone;
                PriceTb.Text = Price;


                if (PrNameTb.Text == "")
                {
                    Key = 0;
                }
                // Lanjutkan untuk kontrol lainnya sesuai kebutuhan
            }
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void PrNameTb_TextChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void PriceTb_TextChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
            Homes Obj = new Homes();
            Obj.Show();
            this.Hide();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {
            Login Obj = new Login();
            Obj.Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Billings Obj = new Billings();
            Obj.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Customers Obj = new Customers();
            Obj.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Employeess Obj = new Employeess();
            Obj.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void GetCategoryId()
        {
            Con.Open();
            string Query = "Select * from CategoryTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);

            // Atur DataSource dan DisplayMember ComboBox
            CatCb.DataSource = ds.Tables[0];
            CatCb.DisplayMember = "CatName"; //gunakan display membber kalau menemukan masalah system.data.dataRowView pada comboboxnya
            Con.Close();
        }
        private void CatCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void QtyTb_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label5_MouseClick(object sender, MouseEventArgs e)
        {
            Billings Obj = new Billings();
            Obj.Show();
            this.Hide();
        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click_1(object sender, EventArgs e)
        {
            Category Obj = new Category();
            Obj.Show();
            this.Hide();
        }
    }
}
