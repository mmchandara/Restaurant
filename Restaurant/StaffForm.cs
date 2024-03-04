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
    public partial class StaffForm : Form
    {
        SqlConnection conn;
        public StaffForm()
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
                    if (dataGridView1.DataSource != null)
                    {
                        if (decimal.TryParse(row.Cells[1].Value.ToString(), out decimal orderAmount))
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
        private void PrintBill(int orderID)
        {
            try
            {
                string getOrderDetails = "SELECT UserID, ItemList, TotalAmount, ISNULL(Discounts, 0) AS Discounts FROM Orders WHERE OrderID = @OrderID";
                using (SqlCommand command = new SqlCommand(getOrderDetails, conn))
                {
                    command.Parameters.AddWithValue("@OrderID", orderID);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        int userID = reader.GetInt32(0);
                        string itemList = reader.GetString(1);
                        decimal totalAmount = reader.GetDecimal(2);
                        decimal? discounts = reader.IsDBNull(3) ? (decimal?)null : reader.GetDecimal(3);

                        string discountText = discounts.HasValue ? $"Discounts: {discounts:C}" : "Discounts: N/A";

                        string billDetails = $"OrderID: {orderID} \n UserID: {userID} \n Item List: {itemList} \n Total Amount: {totalAmount:C} \n {discountText}";

                        MessageBox.Show(billDetails, "Bill Details");
                    }
                    else
                    {
                        MessageBox.Show($"Order with OrderID {orderID} not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnBill_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtOrderID.Text, out int orderIDToPrint))
            {
                PrintBill(orderIDToPrint);
            }
            else
            {
                MessageBox.Show("Please enter a valid OrderID to print the bill.");
            }
        }
        private void btnTables_Click(object sender, EventArgs e)
        {
            TableForm table = new TableForm();
            table.Show();
        }
    }
}
