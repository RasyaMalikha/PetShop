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
    public partial class Employeess : Form
    {
        public Employeess()
        {
            InitializeComponent();
            DisplayEmployees();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=HP-EC0XXX;Initial Catalog=PetShop;Integrated Security=True;TrustServerCertificate=True");

        // Di Form 

        public void HideEmployeeButton()
        {
            SaveBtn.Visible = false;
            EditBtn.Visible = false;
            DeleteBtn.Visible = false;
        }

        private void DisplayEmployees()
        {
            Con.Open();
            string Query = "Select * from EmployeeTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            EmployeeDGV.DataSource = ds.Tables[0]; //pstikan menggunakan datagridview supaya kebaca menggunakan datasource
            Con.Close();
        }

        private void Clear()
        {
            EmpNameTb.Text = "";
            EmpAddTb.Text = "";
            EmpPhoneTb.Text = "";
            PasswordTb.Text = "";
        }


        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (EmpNameTb.Text == "" || EmpAddTb.Text == "" || EmpPhoneTb.Text == "" || PasswordTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO EmployeeTbl (EmpName,EmpAdd,EmpPhone,EmpPass) VALUES (@EN,@EA,@EP,@EPa)", Con);
                    cmd.Parameters.AddWithValue("@EN", EmpNameTb.Text);
                    cmd.Parameters.AddWithValue("@EA", EmpAddTb.Text);
                    cmd.Parameters.AddWithValue("@EP", EmpPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@EPa", PasswordTb.Text);
                    cmd.ExecuteNonQuery(); //sebelumnya error pakai executenon query, setelah diihapus-run-undo lagi-run, berhasil
                    MessageBox.Show("Employee Added");
                    Con.Close();
                    DisplayEmployees();
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
            if (EmployeeDGV.SelectedRows.Count > 0)
            {
                // Ambil ID produk dari baris terpilih
                int productId = Convert.ToInt32(EmployeeDGV.SelectedRows[0].Cells["EmpNum"].Value);

                // Ambil data dari elemen UI
                string empName = EmpNameTb.Text;
                string newAddress = EmpAddTb.Text;
                int newPhone;
                string newPassword = PasswordTb.Text;




                // Validasi input untuk Quantity dan Price
                if (!int.TryParse(EmpPhoneTb.Text, out newPhone))
                {
                    MessageBox.Show("Invalid quantity value.");
                    return;
                }


                // Buat string koneksi dan query SQL
                string connectionString = "Data Source=HP-EC0XXX;Initial Catalog=PetShop;Integrated Security=True";
                string query = "UPDATE EmployeeTbl SET EmpName = @EmployeeName, EmpAdd = @Address, EmpPhone = @Phone, EmpPass = @Password WHERE EmpNum = @EmployeeNumber";

                // Gunakan koneksi SQL untuk melakukan update
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@EmployeeName", empName);
                        command.Parameters.AddWithValue("@Address", newAddress);
                        command.Parameters.AddWithValue("@Phone", newPhone);
                        command.Parameters.AddWithValue("@Password", newPassword);
                        command.Parameters.AddWithValue("@EmployeeNumber", productId);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Product information updated successfully.");
                            DisplayEmployees();
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
            if (EmployeeDGV.SelectedRows.Count > 0)
            {
                int Employeenumber = Convert.ToInt32(EmployeeDGV.SelectedRows[0].Cells["EmpNum"].Value);
                string query = "DELETE FROM EmployeeTbl WHERE EmpNum = @EmployeeName";

                // Sediakan string koneksi Anda di sini
                string connectionString = "Data Source=HP-EC0XXX;Initial Catalog=PetShop;Integrated Security=True;Encrypt=True; TrustServerCertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@EmployeeName", Employeenumber);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Data berhasil dihapus.");
                            Clear();
                            DisplayEmployees();
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

        private void EmployeeDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Pastikan bahwa event dipicu oleh sel yang memiliki data, bukan header atau baris kosong
            if (e.RowIndex >= 0)
            {
                // Mendapatkan nilai dari sel yang diklik
                DataGridViewRow row = EmployeeDGV.Rows[e.RowIndex];

                // Misalkan Anda memiliki kolom dengan nama EmpNum, Nama, Posisi, dll.
                // Gantilah sesuai dengan nama kolom yang sesuai di tabel Anda
                string EmpName = row.Cells["EmpName"].Value.ToString();
                string EmpAdd = row.Cells["EmpAdd"].Value.ToString();
                string EmpPhone = row.Cells["EmpPhone"].Value.ToString();
                string EmpPass = row.Cells["EmpPass"].Value.ToString();
                // Lanjutkan untuk kolom lainnya sesuai kebutuhan

                // Menetapkan nilai-nilai yang diperoleh ke kontrol di form Anda
                EmpNameTb.Text = EmpName;
                EmpAddTb.Text = EmpAdd;
                EmpPhoneTb.Text = EmpPhone;
                PasswordTb.Text = EmpPass;

                if (EmpNameTb.Text == "")
                {
                    Key = 0;
                }
                // Lanjutkan untuk kontrol lainnya sesuai kebutuhan
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Customers Obj = new Customers();
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

        private void label1_Click(object sender, EventArgs e)
        {
            Products Obj = new Products();
            Obj.Show();
            this.Hide();
        }

        private void label13_Click(object sender, EventArgs e)
        {
            Category Obj = new Category();
            Obj.Show();
            this.Hide();
        }

        private void PrintBtn_Click(object sender, EventArgs e)
        {
            printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("pprnm", 785, 700);
            if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Font font = new Font("Arial", 12);
            Brush brush = Brushes.Black;
            float yPos = 50;

            // Judul kolom
            e.Graphics.DrawString("Emp Id", font, brush, new PointF(50, yPos));
            e.Graphics.DrawString("Emp Name", font, brush, new PointF(200, yPos));
            e.Graphics.DrawString("Address", font, brush, new PointF(350, yPos));
            e.Graphics.DrawString("Phone", font, brush, new PointF(500, yPos));
            yPos += 20;

            // Menggambar isi tabel dari DataGridView BillDGV
            foreach (DataGridViewRow row in EmployeeDGV.Rows)
            {
                // Pastikan baris tidak kosong
                if (!row.IsNewRow)
                {
                    // Mengakses data dari kolom-kolom yang ada dalam setiap baris DataGridView
                    string customerName = row.Cells["EmpNum"].Value.ToString();
                    string productPrice = row.Cells["EmpName"].Value.ToString();
                    string productQuantity = row.Cells["EmpAdd"].Value.ToString();
                    string totalPrice = row.Cells["EmpPHone"].Value.ToString();

                    // Menggambar data ke kertas cetak
                    e.Graphics.DrawString(customerName, font, brush, new PointF(50, yPos));
                    e.Graphics.DrawString(productPrice, font, brush, new PointF(200, yPos));
                    e.Graphics.DrawString(productQuantity, font, brush, new PointF(350, yPos));
                    e.Graphics.DrawString(totalPrice, font, brush, new PointF(500, yPos));

                    yPos += 20; // Menyesuaikan posisi untuk baris berikutnya
                }
            }
        }
    }
}
