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
    public partial class Login : Form
    {
        SqlConnection Con = new SqlConnection(@"Data Source=HP-EC0XXX;Initial Catalog=PetShop;Integrated Security=True;TrustServerCertificate=True");
        public static string loginname, logintype;

        public Login()
        {
            InitializeComponent();
        }

        private void lblReset_Click(object sender, EventArgs e)
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtUsername.Focus();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbRole.SelectedIndex > 0)
                {
                    if (txtUsername.Text == String.Empty || txtPassword.Text == String.Empty)
                    {
                        MessageBox.Show("Please enter valid UserName and Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string query = "";
                    string destinationFormName = ""; // Nama form tujuan

                    // Set perintah SQL berdasarkan peran pengguna
                    if (cmbRole.Text == "Admin")
                    {
                        query = "SELECT TOP(1000) [Username], [Password] FROM [PetShop].[dbo].[LoginTbl] WHERE [Username] = @name AND [Password] = @pass";
                        destinationFormName = "mainForm"; // Form untuk Admin
                    }
                    else if (cmbRole.Text == "Employee")
                    {
                        query = "SELECT EmpName, EmpPass FROM EmployeeTbl WHERE EmpName = @empName AND EmpPass = @empPass";
                        destinationFormName = "mainForm"; // Form untuk Karyawan
                    }
                    
                    else if (cmbRole.Text == "Supervisor")
                    {
                        query = "SELECT SvrName, SvrPass FROM SupervisorTbl WHERE SvrName = @svrName AND SvrPass = @svrPass";
                        destinationFormName = "mainForm"; // Form untuk supervisor
                    }
                    else if (cmbRole.Text == "Manajers")
                    {
                        query = "SELECT MnjName, MnjPass FROM ManajerTbl WHERE MnjName = @mnjName AND MnjPass = @mnjPass";
                        destinationFormName = "mainForm"; // Form untuk manajer
                    }


                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.Parameters.AddWithValue("@mnjName", txtUsername.Text); // Gunakan parameter yang benar
                    cmd.Parameters.AddWithValue("@mnjPass", txtPassword.Text);

                    cmd.Parameters.AddWithValue("@name", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@pass", txtPassword.Text);

                    cmd.Parameters.AddWithValue("@empName", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@empPass", txtPassword.Text);

                    cmd.Parameters.AddWithValue("@svrName", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@svrPass", txtPassword.Text);

                    Con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            MessageBox.Show("Login Success Welcome to Home Page", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            loginname = txtUsername.Text;
                            logintype = cmbRole.Text;
                            clrValue();
                            this.Hide();

                            // Navigasi ke form tujuan berdasarkan peran pengguna
                            Form destinationForm = null;
                            if (destinationFormName == "mainForm")
                            {
                                destinationForm = new mainForm();
                            }

                            if (destinationForm != null)
                            {
                                destinationForm.Show();

                                // Menyesuaikan visibilitas menu berdasarkan peran pengguna
                                if (logintype == "Admin")
                                {
                                    ((mainForm)destinationForm).ShowAdminMenu();
                                }
                                else if (logintype == "Employee")
                                {
                                    ((mainForm)destinationForm).ShowEmployeeMenu();
                                }
                                else if (logintype == "Supervisor")
                                {
                                    ((mainForm)destinationForm).ShowSupervisorMenu();
                                }
                                else if (logintype == "Managers")
                                {
                                    ((mainForm)destinationForm).ShowManajerMenu();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid destination form.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid Login. Please check Username and Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a role", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
            }
        }



        private void clrValue()
        {

            txtUsername.Clear();
            txtPassword.Clear();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            txtUsername.Text = "Rasya";
            txtPassword.Text = "12345";

        }

        private void Login_Load_1(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
