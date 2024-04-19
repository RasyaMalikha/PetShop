using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PetShop
{
    public partial class Customers : Form
    {
        public Customers()
        {
            InitializeComponent();
            DisplayCustomers();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=HP-EC0XXX;Initial Catalog=PetShop;Integrated Security=True;TrustServerCertificate=True");
        private void DisplayCustomers()
        {
            Con.Open();
            string Query = "Select * from CustomerTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            CustomerDGV.DataSource = ds.Tables[0]; //pstikan menggunakan datagridview supaya kebaca menggunakan datasource
            Con.Close();

        }
        private void Clear()
        {
            CustNameTb.Text = "";
            CustAddTb.Text = "";
            CustPhoneTb.Text = "";
            
        }
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (CustNameTb.Text == "" || CustAddTb.Text == "" || CustPhoneTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO CustomerTbl (CustName,CustAdd,CustPhone) VALUES (@CN,@CA,@CP)", Con);
                    cmd.Parameters.AddWithValue("@CN", CustNameTb.Text);
                    cmd.Parameters.AddWithValue("@CA", CustAddTb.Text);
                    cmd.Parameters.AddWithValue("@CP", CustPhoneTb.Text);
                    
                    cmd.ExecuteNonQuery(); //sebelumnya error pakai executenon query, setelah diihapus-run-undo lagi-run, berhasil
                    MessageBox.Show("Customer Added");
                    Con.Close();
                    DisplayCustomers();
                    Clear();

                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void EmployeeDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        int Key = 0;
        private void CustomerDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Pastikan bahwa event dipicu oleh sel yang memiliki data, bukan header atau baris kosong
            if (e.RowIndex >= 0)
            {
                // Mendapatkan nilai dari sel yang diklik
                DataGridViewRow row = CustomerDGV.Rows[e.RowIndex];

                // Misalkan Anda memiliki kolom dengan nama EmpNum, Nama, Posisi, dll.
                // Gantilah sesuai dengan nama kolom yang sesuai di tabel Anda
                string EmpName = row.Cells["CustName"].Value.ToString();
                string EmpAdd = row.Cells["CustAdd"].Value.ToString();
                string EmpPhone = row.Cells["CustPhone"].Value.ToString();
                
                // Lanjutkan untuk kolom lainnya sesuai kebutuhan

                // Menetapkan nilai-nilai yang diperoleh ke kontrol di form Anda
                CustNameTb.Text = EmpName;
                CustAddTb.Text = EmpAdd;
                CustPhoneTb.Text = EmpPhone;
                

                if (CustNameTb.Text == "")
                {
                    Key = 0;
                }
                // Lanjutkan untuk kontrol lainnya sesuai kebutuhan
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (CustomerDGV.SelectedRows.Count > 0)
            {
                int customerId = Convert.ToInt32(CustomerDGV.SelectedRows[0].Cells["CustId"].Value);
                string query = "DELETE FROM CustomerTbl WHERE CustId = @CustomerId";

                // Sediakan string koneksi Anda di sini
                string connectionString = "Data Source=HP-EC0XXX;Initial Catalog=PetShop;Integrated Security=True;Encrypt=True; TrustServerCertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@CustomerId", customerId);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Data berhasil dihapus.");
                            Clear();
                            DisplayCustomers();
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

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (CustomerDGV.SelectedRows.Count > 0)
            {
                // Ambil ID produk dari baris terpilih
                int custId = Convert.ToInt32(CustomerDGV.SelectedRows[0].Cells["CustId"].Value);

                // Ambil data dari elemen UI
                string custName = CustNameTb.Text;
                string newAddress = CustAddTb.Text;
                int newPhone;
                




                // Validasi input untuk Quantity dan Price
                if (!int.TryParse(CustPhoneTb.Text, out newPhone))
                {
                    MessageBox.Show("Invalid quantity value.");
                    return;
                }


                // Buat string koneksi dan query SQL
                string connectionString = "Data Source=HP-EC0XXX;Initial Catalog=PetShop;Integrated Security=True";
                string query = "UPDATE CustomerTbl SET CustName = @CustomerName, CustAdd = @Address, CustPhone = @Phone  WHERE CustId = @CustomerID";

                // Gunakan koneksi SQL untuk melakukan update
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@CustomerName", custName);
                        command.Parameters.AddWithValue("@Address", newAddress);
                        command.Parameters.AddWithValue("@Phone", newPhone);
                        command.Parameters.AddWithValue("@CustomerID", custId);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Product information updated successfully.");
                            DisplayCustomers();
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

        private void label1_Click(object sender, EventArgs e)
        {
            Products Obj = new Products();
            Obj.Show();
            this.Hide();
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

        private void label3_Click(object sender, EventArgs e)
        {
            Employeess Obj = new Employeess();
            Obj.Show();
            this.Hide();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Login Obj = new Login();
            Obj.Show();
            this.Hide();
        }

        private void CustNameTb_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click_1(object sender, EventArgs e)
        {
            Billings Obj = new Billings();
            Obj.Show();
            this.Hide();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Font font = new Font("Arial", 12);
            Brush brush = Brushes.Black;
            Pen pen = new Pen(Brushes.Black);
            float yPos = 50;
            float columnWidth = 150;

            // Judul kolom
            e.Graphics.DrawRectangle(pen, 50, yPos, columnWidth, 20); // Customer Id
            e.Graphics.DrawString("Customer Id", font, brush, new RectangleF(50, yPos, columnWidth, 20));

            e.Graphics.DrawRectangle(pen, 200, yPos, columnWidth, 20); // Customer Name
            e.Graphics.DrawString("Customer Name", font, brush, new RectangleF(200, yPos, columnWidth, 20));

            e.Graphics.DrawRectangle(pen, 350, yPos, columnWidth, 20); // Address
            e.Graphics.DrawString("Address", font, brush, new RectangleF(350, yPos, columnWidth, 20));

            e.Graphics.DrawRectangle(pen, 500, yPos, columnWidth, 20); // Phone
            e.Graphics.DrawString("Phone", font, brush, new RectangleF(500, yPos, columnWidth, 20));

            // Menggambar garis horizontal di bawah judul kolom
            e.Graphics.DrawLine(pen, 50, yPos + 20, 650, yPos + 20);

            yPos += 40; // Menyesuaikan posisi untuk baris berikutnya setelah judul kolom

            // Menggambar isi tabel dari DataGridView BillDGV
            foreach (DataGridViewRow row in CustomerDGV.Rows)
            {
                // Pastikan baris tidak kosong
                if (!row.IsNewRow)
                {
                    // Mengakses data dari kolom-kolom yang ada dalam setiap baris DataGridView
                    string customerId = row.Cells["CustId"].Value.ToString();
                    string customerName = row.Cells["CustName"].Value.ToString();
                    string address = row.Cells["CustAdd"].Value.ToString();
                    string phone = row.Cells["CustPhone"].Value.ToString();

                    // Menggambar data ke kertas cetak
                    e.Graphics.DrawString(customerId, font, brush, new PointF(50, yPos));
                    e.Graphics.DrawString(customerName, font, brush, new PointF(200, yPos));
                    e.Graphics.DrawString(address, font, brush, new PointF(350, yPos));
                    e.Graphics.DrawString(phone, font, brush, new PointF(500, yPos));

                    // Menambahkan garis untuk memisahkan setiap sel
                    e.Graphics.DrawRectangle(pen, 50, yPos, 150, 20); // Customer Id
                    e.Graphics.DrawRectangle(pen, 200, yPos, 150, 20); // Customer Name
                    e.Graphics.DrawRectangle(pen, 350, yPos, 150, 20); // Address
                    e.Graphics.DrawRectangle(pen, 500, yPos, 150, 20); // Phone

                    yPos += 20; // Menyesuaikan posisi untuk baris berikutnya
                }
            }
        }


        private void PrintBtn_Click(object sender, EventArgs e)
        {
            printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("pprnm", 785, 700);
            if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void printPreviewDialog1_Load(object sender, EventArgs e)
        {
            PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
            printPreviewDialog.Document = printDocument1;

            // Menampilkan dialog cetak pratinjau
            printPreviewDialog.ShowDialog();
        }
    }
}
    