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

namespace Restaurant
{
    public partial class ManagerForm : Form
    {
        SqlConnection conn;

        public ManagerForm()
        {
            InitializeComponent();
            DatabaseConnection();
        }
        private void DatabaseConnection()
        {
            try
            {
                conn = new SqlConnection(@"Data Source=(localdb)\MSSQLLOCALDB;Initial Catalog=MitchelRestaurant;User ID=sa;Password=password");
                conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void DisplayOrders()
        {
            try
            {
                SqlCommand command;
                string displayUsers = "SELECT * FROM Orders";
                command = new SqlCommand(displayUsers, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            DisplayOrders();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("INSERT INTO Orders (UserID, ItemList, TotalAmount) VALUES (@UserID, @Name, @Total)", conn))
                {
                    command.Parameters.AddWithValue("@Name", txtOrderName.Text);
                    command.Parameters.AddWithValue("@UserID", int.Parse(txtUser.Text));
                    command.Parameters.AddWithValue("@Total", decimal.Parse(txtAmount.Text));

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            txtOrderName.Clear();
            txtUser.Clear();
            txtAmount.Clear();
            DisplayOrders();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("UPDATE Orders SET UserID=@UserID, ItemList=@Name, TotalAmount=@Total WHERE OrderID = @OrderID", conn))
                {
                    command.Parameters.AddWithValue("@Name", txtOrderName.Text);
                    command.Parameters.AddWithValue("@UserID", int.Parse(txtUser.Text));
                    command.Parameters.AddWithValue("@Total", decimal.Parse(txtAmount.Text));
                    command.Parameters.AddWithValue("@OrderID", txtOrderID.Text);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            txtOrderName.Clear();
            txtUser.Clear();
            txtAmount.Clear();
            txtOrderID.Clear();
            DisplayOrders();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("DELETE FROM Orders WHERE OrderID = @OrderID", conn))
                {
                    command.Parameters.AddWithValue("@OrderID", txtOrderID.Text);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            txtOrderID.Clear();
            DisplayOrders();
        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            try
            {
                if (decimal.TryParse(txtDiscount.Text, out decimal discountPercentage))
                {
                    decimal totalAmount = CalculateTotalAmount();
                    decimal discountedAmount = ApplyDiscount(totalAmount, discountPercentage);

                    UpdateTotalAmountInDatabase(discountedAmount);
                }
                else
                {
                    MessageBox.Show("Invalid discount percentage. Please enter a valid decimal value.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            txtDiscount.Clear();
            txtOrderID.Clear();
            DisplayOrders();
        }
        private decimal CalculateTotalAmount()
        {
            decimal totalAmount = 0;

            try
            {

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[4].Value != null && dataGridView1.DataSource != null)
                    {
                        if (decimal.TryParse(row.Cells[4].Value.ToString(), out decimal orderAmount))
                        {
                            totalAmount += orderAmount;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return totalAmount;
        }
        private decimal ApplyDiscount(decimal totalAmount, decimal discountPercentage)
        {
            try
            {
                decimal discountAmount = (totalAmount * discountPercentage) / 100;
                return totalAmount - discountAmount;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return totalAmount;
            }
        }
        private void UpdateTotalAmountInDatabase(decimal discountedAmount)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("UPDATE Orders SET TotalAmount=@Total, Discounts=@Discounts WHERE OrderID = @OrderID", conn))
                {
                    command.Parameters.AddWithValue("@Discounts", decimal.Parse(txtDiscount.Text));
                    command.Parameters.AddWithValue("@Total", discountedAmount);
                    command.Parameters.AddWithValue("@OrderID", txtOrderID.Text);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void DisplayUsers()
        {
            try
            {
                SqlCommand command;
                string displayUsers = "SELECT * FROM Users";
                command = new SqlCommand(displayUsers, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView2.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DisplayUsers();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("UPDATE Users SET Role=@Role WHERE UserID = @UserID", conn))
                {
                    command.Parameters.AddWithValue("@UserID", txtRoleID.Text);
                    command.Parameters.AddWithValue("@Role", comboRole.Text);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            txtRoleID.Clear();
            comboRole.ResetText();
            DisplayUsers();
        }

        private void btnTable_Click(object sender, EventArgs e)
        {
            TableForm table = new TableForm();
            table.Show();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtOrderID.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtUser.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtOrderName.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtAmount.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            object discountValue = dataGridView1.Rows[e.RowIndex].Cells[4].Value;
            txtDiscount.Text = discountValue != null ? discountValue.ToString() : string.Empty;
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtRoleID.Text = dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString();
            comboRole.Text = dataGridView2.Rows[e.RowIndex].Cells[3].Value.ToString();
        }
    }
}
