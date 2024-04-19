using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PetShop
{
    public partial class mainForm : Form
    {

        public mainForm()
        {
            InitializeComponent();
        }
    public void loadform(object Form)
        {
            if (this.mainPanel.Controls.Count > 0)
                this.mainPanel.Controls.RemoveAt(0);
            Form f = Form as Form;
            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            this.mainPanel.Controls.Add(f);
            this.mainPanel.Tag = f;
            f.Show();
        }

        public void ShowAdminMenu()
        {
            // Menampilkan semua menu untuk admin
            productsToolStripMenuItem.Visible = true;
            employeesToolStripMenuItem.Visible = true;
            customersToolStripMenuItem.Visible = true;
            billingsToolStripMenuItem.Visible = true;
            Admin.Visible = true;
            label1.Visible = false;
            manajerLbl.Visible = false;
            SvrLbl.Visible = false;

            // Menyembunyikan menu untuk karyawan
            homeToolStripMenuItem.Visible = true;
        }

        public void ShowEmployeeMenu()
        {
            // Menampilkan menu yang sesuai untuk karyawan
            homeToolStripMenuItem.Visible = true;
            label1.Visible = true;
            Admin.Visible = false;
            manajerLbl.Visible = false;
            SvrLbl.Visible = false;

            // Menyembunyikan menu yang tidak diperlukan untuk karyawan
            productsToolStripMenuItem.Visible = true;
            employeesToolStripMenuItem.Visible = false;
            customersToolStripMenuItem.Visible = true;
            billingsToolStripMenuItem.Visible = true;
            managerToolStripMenuItem.Visible = true;
            supervisorToolStripMenuItem .Visible = true;
            reportToolStripMenuItem .Visible = true;
        }

        public void ShowManajerMenu()
        {
            // Menampilkan menu yang sesuai untuk karyawan
            homeToolStripMenuItem.Visible = true;
            label1.Visible = false;
            Admin.Visible = false;
            manajerLbl.Visible = true;
            SvrLbl.Visible = false;

            // Menyembunyikan menu yang tidak diperlukan untuk karyawan
            productsToolStripMenuItem.Visible = false;
            employeesToolStripMenuItem.Visible = false;
            customersToolStripMenuItem.Visible = false;
            billingsToolStripMenuItem.Visible = true;
            supervisorToolStripMenuItem.Visible = true;
            reportToolStripMenuItem.Visible = false;
        }

        public void ShowSupervisorMenu()
        {
            // Menampilkan menu yang sesuai untuk karyawan
            homeToolStripMenuItem.Visible = true;
            label1.Visible = false;
            Admin.Visible = false;
            manajerLbl.Visible = false;
            SvrLbl.Visible = true;

            // Menyembunyikan menu yang tidak diperlukan untuk karyawan
            productsToolStripMenuItem.Visible = true;
            employeesToolStripMenuItem.Visible = true;
            customersToolStripMenuItem.Visible = true;
            billingsToolStripMenuItem.Visible = true;
            managerToolStripMenuItem .Visible = false;
            reportToolStripMenuItem .Visible = true;

            
        }

        private void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadform(new Homes());
        }

        private void billingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadform(new Billings());

        }

        private void productsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadform(new Products());

        }

        private void customersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadform(new Customers());

        }

        private void categoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadform(new Category());

        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login Obj = new Login();
            this.Hide();
            MessageBox.Show("You Has Been Logged Out");
            Obj.ShowDialog();
            this.Close();
        }

        private void employeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadform(new Employeess());

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void paymentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadform(new Payments());
        }

        private void reportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadform(new Reports());

        }

        private void managerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadform(new Manager());

        }

        private void supervisorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadform(new Supervisor());

        }

        private void reportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadform(new Reports());

        }
    }
}
