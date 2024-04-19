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
    public partial class Supervisor : Form
    {
        public Supervisor()
        {
            InitializeComponent();
            DisplaySupervisor();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=HP-EC0XXX;Initial Catalog=PetShop;Integrated Security=True;TrustServerCertificate=True");

        private void DisplaySupervisor()
        {
            Con.Open();
            string Query = "Select * from SupervisorTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            ManajerDGV.DataSource = ds.Tables[0]; //pstikan menggunakan datagridview supaya kebaca menggunakan datasource
            Con.Close();
        }
        private void Clear()
        {
            MnjNameTb.Text = "";
            MnjPhoneTb.Text = "";
            MnjPassTb.Text = "";
        }
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (MnjNameTb.Text == "" || MnjPhoneTb.Text == "" || MnjPassTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO SupervisorTbl (SvrName, SvrPhone, SvrPass) VALUES (@EN,@EA,@EP)", Con);
                    cmd.Parameters.AddWithValue("@EN", MnjNameTb.Text);
                    cmd.Parameters.AddWithValue("@EA", MnjPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@EP", MnjPassTb.Text);
                    cmd.ExecuteNonQuery(); //sebelumnya error pakai executenon query, setelah diihapus-run-undo lagi-run, berhasil
                    MessageBox.Show("Manajer Added");
                    Con.Close();
                    DisplaySupervisor();
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
            if (ManajerDGV.SelectedRows.Count > 0)
            {
                // Ambil ID produk dari baris terpilih
                int productId = Convert.ToInt32(ManajerDGV.SelectedRows[0].Cells["SvrNum"].Value);

                // Ambil data dari elemen UI
                string empName = MnjNameTb.Text;
                int newPhone;
                string newPassword = MnjPassTb.Text;




                // Validasi input untuk Quantity dan Price
                if (!int.TryParse(MnjPhoneTb.Text, out newPhone))
                {
                    MessageBox.Show("Invalid quantity value.");
                    return;
                }


                // Buat string koneksi dan query SQL
                string connectionString = "Data Source=HP-EC0XXX;Initial Catalog=PetShop;Integrated Security=True";
                string query = "UPDATE SupervisorTbl SET SvrName = @EmployeeName, SvrPhone = @Phone, SvrPass = @Password WHERE SvrNum = @EmployeeNumber";

                // Gunakan koneksi SQL untuk melakukan update
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@EmployeeName", empName);
                        command.Parameters.AddWithValue("@Phone", newPhone);
                        command.Parameters.AddWithValue("@Password", newPassword);
                        command.Parameters.AddWithValue("@EmployeeNumber", productId);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Product information updated successfully.");
                            DisplaySupervisor();
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
            if (ManajerDGV.SelectedRows.Count > 0)
            {
                int Employeenumber = Convert.ToInt32(ManajerDGV.SelectedRows[0].Cells["SvrNum"].Value);
                string query = "DELETE FROM SupervisorTbl WHERE SvrNum = @EmployeeName";

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
                            DisplaySupervisor();
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

        int Key = 0;
        private void ManajerDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Pastikan bahwa event dipicu oleh sel yang memiliki data, bukan header atau baris kosong
            if (e.RowIndex >= 0)
            {
                // Mendapatkan nilai dari sel yang diklik
                DataGridViewRow row = ManajerDGV.Rows[e.RowIndex];

                // Misalkan Anda memiliki kolom dengan nama EmpNum, Nama, Posisi, dll.
                // Gantilah sesuai dengan nama kolom yang sesuai di tabel Anda
                string EmpName = row.Cells["SvrName"].Value.ToString();
                string EmpAdd = row.Cells["SvrPhone"].Value.ToString();
                string EmpPhone = row.Cells["SvrPass"].Value.ToString();
                // Lanjutkan untuk kolom lainnya sesuai kebutuhan

                // Menetapkan nilai-nilai yang diperoleh ke kontrol di form Anda
                MnjNameTb.Text = EmpName;
                MnjPhoneTb.Text = EmpAdd;
                MnjPassTb.Text = EmpPhone;

                if (MnjNameTb.Text == "")
                {
                    Key = 0;
                }
                // Lanjutkan un
            }
        }
    }
}
