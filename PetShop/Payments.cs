using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PetShop
{
    public partial class Payments : Form
    {
        public Payments()
        {
            InitializeComponent();
            DisplayOrderDetail();
            GetCategoryId();
            DisplayPayment();
            loadPaymentType();
            loadCBBank();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=HP-EC0XXX;Initial Catalog=PetShop;Integrated Security=True;TrustServerCertificate=True");

        private void GetCategoryId()
        {
            Con.Open();
            string Query = "Select * from OrderDetailTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);

            OrdNumCb.DataSource = ds.Tables[0];
            OrdNumCb.DisplayMember = "OrderNum"; //gunakan display membber kalau menemukan masalah system.data.dataRowView pada comboboxnya
            Con.Close();
        }
        private void DisplayOrderDetail()
        {
            string Query = "Select * from OrderDetailTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            OrderDetailDGV.DataSource = ds.Tables[0]; //pstikan menggunakan datagridview supaya kebaca menggunakan datasource
        }

        private void DisplayPayment()
        {
            string Query = "Select * from PaymentsTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            PaymentDGV.DataSource = ds.Tables[0]; //pstikan menggunakan datagridview supaya kebaca menggunakan datasource
        }
        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void ProductDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void OrderDetailDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = OrderDetailDGV.Rows[e.RowIndex];

                string OrdNum = row.Cells["OrderNum"].Value.ToString();
                string CustName = row.Cells["OrdCustName"].Value.ToString();
                string PrPrice = row.Cells["OrdPrice"].Value.ToString();

                OrdNumCb.Text = OrdNum;
                CustNameTb.Text = CustName;
                TotalLbl.Text = PrPrice;

            }
        }



        private void loadPaymentType()
        {
            PaymentCb.Items.Clear();

            PaymentCb.Items.Add("Cash");
            PaymentCb.Items.Add("Credit");
        }

        private void loadCBBank()
        {
            BankCb.Items.Clear();

            BankCb.Items.Add("BCA");
            BankCb.Items.Add("BNI");
            BankCb.Items.Add("MANDIRI");
        }
        private void PymTypeCb_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void BankCb_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void NominalTb_TextChanged(object sender, EventArgs e)
        {
           UpdateKembalian();
        }
        private void UpdateKembalian()
        {
            int nominal;
            int total;
            if (int.TryParse(NominalTb.Text, out nominal) && int.TryParse(TotalLbl.Text, out total))
            {
                int kembalian = nominal - total;
                KembalianLbl.Text = kembalian.ToString();
            }
            else
            {
                KembalianLbl.Text = "0"; 
            }
        }

        private void ResetBtn_Click(object sender, EventArgs e)
        {
            if (OrdNumCb.DataSource != null)
            {
                OrdNumCb.DataSource = null;
            }
            else
            {
                OrdNumCb.Items.Clear();
            }
            CustNameTb.Clear();
            PaymentCb.Items.Clear();
            NominalTb.Clear();
            BankCb.Items.Clear();
        }

        private void Clear()
        {
            OrdNumCb.Text = "";
            CustNameTb.Text = "";
            dateTimePicker1.Text = "";
            PaymentCb.Text = "";
            NominalTb.Text = "";

        }
        private void PayBtn_Click(object sender, EventArgs e)
        {
            if (OrdNumCb.Text == "" || CustNameTb.Text == "" || dateTimePicker1.Text == "" || PaymentCb.Text == "" || NominalTb.Text == "" || BankCb.Text == "")
            {
                MessageBox.Show("Data harus di Isi dengan Lengkap");
            }
            else
            {
                try
                {
                    Con.Open();

                    string getOrderDetailIdQuery = "SELECT OrderNum FROM OrderDetailTbl WHERE OrderNum = @ON";
                    SqlCommand getIdCommand = new SqlCommand(getOrderDetailIdQuery, Con);
                    getIdCommand.Parameters.AddWithValue("@ON", int.Parse(OrdNumCb.Text));
                    int orderDetailId = (int)getIdCommand.ExecuteScalar(); 

                    string insertPaymentQuery = @"
            SET IDENTITY_INSERT PaymentsTbl ON
            INSERT INTO PaymentsTbl (OrderNum, CustName, PaymentDate, PaymentPrice, PaymentType, Amount, PaymentBank) 
            VALUES (@OrderDetailId, @CN, @PD, @PP, @PT, @AM, @PB);
            SET IDENTITY_INSERT PaymentsTbl OFF";

                    SqlCommand cmd = new SqlCommand(insertPaymentQuery, Con);
                    cmd.Parameters.AddWithValue("@OrderDetailId", orderDetailId); 
                    cmd.Parameters.AddWithValue("@CN", CustNameTb.Text);
                    cmd.Parameters.AddWithValue("@PD", dateTimePicker1.Value); 
                    cmd.Parameters.AddWithValue("@PP", decimal.Parse(NominalTb.Text)); 
                    cmd.Parameters.AddWithValue("@PT", PaymentCb.Text);
                    cmd.Parameters.AddWithValue("@AM", decimal.Parse(KembalianLbl.Text)); 
                    cmd.Parameters.AddWithValue("@PB", BankCb.Text);
                    cmd.ExecuteNonQuery();
                    DisplayPayment();

                   
                    string updateOrderConfirmQuery = "UPDATE OrderDetailTbl SET OrdConfirm = 'Sudah Dibayar' WHERE OrderNum = @ON";
                    SqlCommand updateCmd = new SqlCommand(updateOrderConfirmQuery, Con);
                    updateCmd.Parameters.AddWithValue("@ON", orderDetailId);
                    updateCmd.ExecuteNonQuery();
                    DisplayOrderDetail();
                    MessageBox.Show("Pembayaran Berhasil!");
                    Con.Close();

                    Clear();

                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }
    }
}
