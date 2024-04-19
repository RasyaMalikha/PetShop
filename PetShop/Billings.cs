using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PetShop
{
    public partial class Billings : Form
    {

        public Billings()
        {
            InitializeComponent();
            GetCustName();
            DisplayProduct();

            DisplayProductBill();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=HP-EC0XXX;Initial Catalog=PetShop;Integrated Security=True;TrustServerCertificate=True");

        private DataTable dataTable;

        
        private void DisplayOrderDetail()
        {
            string Query = "SELECT pb.BillNum, pb.CustName, pb.OrdPrName, pb.PrPrice, pb.PrQty, pb.TotalPrice, c.CustId, p.PrId\r\nFROM ProductBillTbl pb\r\nINNER JOIN CustomerTbl c ON pb.CustName = c.CustName\r\nINNER JOIN ProductTbl p ON pb.OrdPrName = p.PrName;\r\n";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
        }

        private void DisplayProduct()
        {
            
            string Query = "Select * from ProductTbl";     
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            ProductDGV.DataSource = ds.Tables[0]; //pstikan menggunakan datagridview supaya kebaca menggunakan datasource
        }

         public void DisplayProductBill()
        {
            string Query = "Select * from ProductBillTbl2";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            BillDGV.DataSource = ds.Tables[0]; //pstikan menggunakan datagridview supaya kebaca menggunakan datasource
        }
        private void GetCustName()
        {
            Con.Open();
            string Query = "Select * from CustomerTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);

            // Atur DataSource dan DisplayMember ComboBox
            CustNameCb.DataSource = ds.Tables[0];
            CustNameCb.DisplayMember = "CustName"; //gunakan display membber kalau menemukan masalah system.data.dataRowView pada comboboxnya
            Con.Close();
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ProductDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = ProductDGV.Rows[e.RowIndex];

                string empName = row.Cells["PrName"].Value.ToString();
                string price = row.Cells["PrPrice"].Value.ToString();
                string quantity = "0";

                try
                {
                    using (SqlConnection connection = new SqlConnection(@"Data Source=HP-EC0XXX;Initial Catalog=PetShop;Integrated Security=True;TrustServerCertificate=True"))
                    {
                        connection.Open();

                        // Mengambil nilai dari ComboBox
                        string custName = CustNameCb.Text;

                        string sqlInsert = "INSERT INTO ProductBillTbl2 (CustName, PrName, PrPrice, PrQty) VALUES (@CN, @ProductName, @Price, @Quantity);";
                        sqlInsert += "UPDATE ProductTbl SET PrQty = PrQty - @Quantity WHERE PrName = @ProductName;";

                        using (SqlCommand command = new SqlCommand(sqlInsert, connection))
                        {
                            // Menambahkan parameter ke pernyataan SQL
                            command.Parameters.AddWithValue("@CN", custName);
                            command.Parameters.AddWithValue("@ProductName", empName);
                            command.Parameters.AddWithValue("@Price", price);
                            command.Parameters.AddWithValue("@Quantity", quantity);

                            // Menjalankan pernyataan SQL
                            command.ExecuteNonQuery();
                            DisplayProductBill();
                            DisplayProduct();
                        }
                    }

                    MessageBox.Show("Data berhasil ditambahkan ke dalam tabel ProductBillTbl.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }       

        private void ResetBtn_Click(object sender, EventArgs e)
        {
        }

        private void InitializeDataGridView()
        {
            dataTable = new DataTable();

            dataTable.Columns.Add("Customer Name", typeof(string));
            dataTable.Columns.Add("Product Name", typeof(string));
            dataTable.Columns.Add("Product Price", typeof(decimal));
            dataTable.Columns.Add("Product Quantity", typeof(int));
            dataTable.Columns.Add("Total Price", typeof(int));


            dataTable.Rows.Add("");
            BillDGV.DataSource = dataTable;
        }
       

        
        private void KurangiStokProduk(string productName, int quantity)
        {
            // Koneksi ke database
            SqlConnection connection = new SqlConnection("Data Source=HP-EC0XXX;Initial Catalog=PetShop;Integrated Security=True;Encrypt=True; TrustServerCertificate=True");

            // Buat perintah SQL untuk mendapatkan jumlah stok produk yang tersedia
            string checkQuery = "SELECT PrQty FROM ProductTbl WHERE PrName = @productName";

            // Buat objek SqlCommand untuk mengecek jumlah stok
            SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
            checkCommand.Parameters.AddWithValue("@productName", productName);

            // Variabel untuk menyimpan jumlah stok produk yang tersedia
            int availableQuantity = 0;

            try
            {
                // Buka koneksi
                connection.Open();

                // Eksekusi perintah SQL untuk mendapatkan jumlah stok produk yang tersedia
                SqlDataReader reader = checkCommand.ExecuteReader();
                if (reader.Read())
                {
                    availableQuantity = reader.GetInt32(0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan saat memeriksa stok produk: " + ex.Message);
            }
            finally
            {
                // Tutup koneksi
                connection.Close();
            }

            // Periksa apakah jumlah yang diminta oleh pelanggan lebih dari stok yang tersedia
            if (quantity > availableQuantity)
            {
                MessageBox.Show("Produk tidak cukup untuk memenuhi pesanan.");
            }
            else
            {
                // Buat perintah SQL UPDATE untuk mengurangi stok produk
                string updateQuery = "UPDATE ProductTbl SET PrQty = PrQty - @quantity WHERE PrName = @productName";

                // Buat objek SqlCommand untuk mengurangi stok produk
                SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                updateCommand.Parameters.AddWithValue("@quantity", quantity);
                updateCommand.Parameters.AddWithValue("@productName", productName);

                try
                {
                    // Buka koneksi
                    connection.Open();

                    // Eksekusi perintah SQL untuk mengurangi stok produk
                    int rowsAffected = updateCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Tampilkan pesan jika stok produk berhasil dikurangi
                        MessageBox.Show("Stok produk berhasil dikurangi.");
                    }
                    else
                    {
                        MessageBox.Show("Tidak ada produk dengan nama tersebut.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan saat mengurangi stok produk: " + ex.Message);
                }
                finally
                {
                    // Tutup koneksi
                    connection.Close();
                }
            }
        }


        private bool IsStockAvailable(string productName, int quantity)
        {
            // Koneksi ke database
            SqlConnection connection = new SqlConnection("Data Source=HP-EC0XXX;Initial Catalog=PetShop;Integrated Security=True;Encrypt=True; TrustServerCertificate=True");

            // Buat perintah SQL untuk mendapatkan jumlah stok produk yang tersedia
            string checkQuery = "SELECT PrQty FROM ProductTbl WHERE PrName = @productName";

            // Buat objek SqlCommand untuk mengecek jumlah stok
            SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
            checkCommand.Parameters.AddWithValue("@productName", productName);

            // Variabel untuk menyimpan jumlah stok produk yang tersedia
            int availableQuantity = 0;

            try
            {
                // Buka koneksi
                connection.Open();

                // Eksekusi perintah SQL untuk mendapatkan jumlah stok produk yang tersedia
                SqlDataReader reader = checkCommand.ExecuteReader();
                if (reader.Read())
                {
                    availableQuantity = reader.GetInt32(0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan saat memeriksa stok produk: " + ex.Message);
            }
            finally
            {
                // Tutup koneksi
                connection.Close();
            }

            // Periksa apakah jumlah yang diminta oleh pelanggan lebih dari stok yang tersedia
            return quantity <= availableQuantity;
        }

        private void CustIdCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        int Key = 0, Stock = 0;
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

        private void label6_Click(object sender, EventArgs e)
        {
            Login Obj = new Login();
            Obj.Show();
            this.Hide();
        }


        private void BillDGV_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void TotalLbl_Click(object sender, EventArgs e)
        {

        }

        string prodname;
        private void PrintBtn_Click(object sender, EventArgs e)
        {
            
        }

        private bool IsRowEmpty(DataGridViewRow row)
        {
            foreach (DataGridViewCell cell in row.Cells)
            {
                if (cell.Value != null && !string.IsNullOrWhiteSpace(cell.Value.ToString()))
                {
                    return false;
                }
            }
            return true;
        }

        int prodid, prodqty, prodprice, tottal, pos = 60;

        private void label5_Click(object sender, EventArgs e)
        {
            Category Obj = new Category();
            Obj.Show();
            this.Hide();
        }

        private void printPreviewDialog1_Load(object sender, EventArgs e)
        {

        }

        private void label6_Click_1(object sender, EventArgs e)
        {

        }

        private void label3_Click_1(object sender, EventArgs e)
        {
            Products obj = new Products();
            obj.Show();
        }

        private void BillDGV_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == BillDGV.Columns["PrQty"].Index)
            {
                try
                {
                    int totalPrice = 0;

                    // Iterasi melalui setiap baris dalam DataGridView
                    foreach (DataGridViewRow row in BillDGV.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            string productName = row.Cells["PrName"].Value.ToString();
                            int quantity = Convert.ToInt32(row.Cells["PrQty"].Value);

                            // Hitung total harga dari produk saat ini
                            int productPrice = CalculateTotalPrice(productName, quantity);

                            // Akumulasikan total harga dari semua produk
                            totalPrice += productPrice;
                        }
                    }

                    // Update totalLbl dengan total harga
                    TotalLbl.Text = totalPrice.ToString();

                    MessageBox.Show("Jumlah produk telah diperbarui.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private int CalculateTotalPrice(string productName, int quantity)
        {
            int pricePerProduct = 0;

            // Retrieve price per product from ProductTbl
            using (SqlConnection connection = new SqlConnection("Data Source=HP-EC0XXX;Initial Catalog=PetShop;Integrated Security=True;Encrypt=True;TrustServerCertificate=True"))
            {
                connection.Open();

                string sqlSelectPrice = "SELECT PrPrice FROM ProductTbl WHERE PrName = @ProductName;";
                using (SqlCommand command = new SqlCommand(sqlSelectPrice, connection))
                {
                    command.Parameters.AddWithValue("@ProductName", productName);
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        pricePerProduct = Convert.ToInt32(result);
                    }
                }
            }

            // Calculate total price based on price per product and quantity
            int totalPrice = pricePerProduct * quantity;

            return totalPrice;
        }

        private void HoldBtn_Click(object sender, EventArgs e)
        {
            KomentarForm holdForm = new KomentarForm(BillDGV); // Meneruskan BillDGV ke KomentarForm
            holdForm.ShowDialog();
        }

        private void label2_Click_1(object sender, EventArgs e)
        {
            HoldForm holdForm = new HoldForm();
            holdForm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Payments holdForm = new Payments();
            holdForm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void OrderDetailDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void PrNameTb_TextChanged(object sender, EventArgs e)
        {

        }

        private void CustNameTb_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Font font = new Font("Arial", 12);
            Brush brush = Brushes.Black;
            float yPos = 50;

            // Judul kolom
            e.Graphics.DrawString("Customer Name", font, brush, new PointF(50, yPos));
            e.Graphics.DrawString("Product Price", font, brush, new PointF(200, yPos));
            e.Graphics.DrawString("Product Quantity", font, brush, new PointF(350, yPos));
            e.Graphics.DrawString("Total Price", font, brush, new PointF(500, yPos));
            yPos += 20;

            // Menggambar isi tabel dari DataGridView BillDGV
            foreach (DataGridViewRow row in BillDGV.Rows)
            {
                // Pastikan baris tidak kosong
                if (!row.IsNewRow)
                {
                    // Mengakses data dari kolom-kolom yang ada dalam setiap baris DataGridView
                    string customerName = row.Cells["CustName"].Value.ToString();
                    string productPrice = row.Cells["OrdPrName"].Value.ToString();
                    string productQuantity = row.Cells["PrQty"].Value.ToString();
                    string totalPrice = row.Cells["TotalPrice"].Value.ToString();

                    // Menggambar data ke kertas cetak
                    e.Graphics.DrawString(customerName, font, brush, new PointF(50, yPos));
                    e.Graphics.DrawString(productPrice, font, brush, new PointF(200, yPos));
                    e.Graphics.DrawString(productQuantity, font, brush, new PointF(350, yPos));
                    e.Graphics.DrawString(totalPrice, font, brush, new PointF(500, yPos));

                    yPos += 20; // Menyesuaikan posisi untuk baris berikutnya
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Homes Obj = new Homes();
            Obj.Show();
            this.Hide();
        }

       
    }
}
