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
    public partial class TableForm : Form
    {
        SqlConnection conn;
        public TableForm()
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
        private void DisplayTable()
        {
            try
            {
                SqlCommand command;
                string displayUsers = "SELECT * FROM Tables";
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
            DisplayTable();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("INSERT INTO Tables (TableID, Status, ReservationDetails) VALUES (@TableID, @Status, @Reservation)", conn))
                {
                    command.Parameters.AddWithValue("@Table",txtTableID.Text);
                    command.Parameters.AddWithValue("@Status", comboStatus.Text);
                    command.Parameters.AddWithValue("@Reservation", txtRes.Text);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            txtTableID.Clear();
            comboStatus.Items.Clear();
            txtRes.Clear();
            DisplayTable();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("UPDATE Tables SET Status=@Status, Reservation=@Reservation WHERE TableID = @TableID", conn))
                {
                    command.Parameters.AddWithValue("@Status", comboStatus.Text);
                    command.Parameters.AddWithValue("@Reservation", txtRes.Text);
                    command.Parameters.AddWithValue("@TableID", txtTableID.Text);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            comboStatus.Items.Clear();
            txtRes.Clear();
            DisplayTable();
        }
    }
}
