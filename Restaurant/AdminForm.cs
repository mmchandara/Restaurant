using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restaurant
{
    public partial class AdminForm : Form
    {
        SqlConnection conn;
        
        public AdminForm()
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
        private void Display()
        {
            try
            {
                SqlCommand command;
                string displayUsers = "SELECT * FROM Users";
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
            Display();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtUser.Text;
                string password = txtPass.Text;
                string role = comboRole.Text;
                if (int.TryParse(comboAccess.Text, out int accessLevel))
                {
                    using (SqlCommand command = new SqlCommand("INSERT INTO Users (Username, Password, Role, AccessLevel) VALUES (@Username, @Password, @Role, @AccessLevel)", conn))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@Role", role);
                        command.Parameters.AddWithValue("@AccessLevel", accessLevel);

                        command.ExecuteNonQuery();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Access Level. Please enter a valid integer.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            txtUser.Clear();
            txtPass.Clear();
            comboRole.ResetText();
            comboAccess.ResetText();
            Display();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("UPDATE Users SET Username=@Username, Password=@Password, Role=@Role, AccessLevel=@AccessLevel WHERE userid = @UserID", conn))
                {
                    command.Parameters.AddWithValue("@Username", txtUser.Text);
                    command.Parameters.AddWithValue("@Password", txtPass.Text);
                    command.Parameters.AddWithValue("@Role", comboRole.Text);
                    command.Parameters.AddWithValue("@AccessLevel", comboAccess.Text);
                    command.Parameters.AddWithValue("@UserID", txtID.Text);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            txtID.Clear();
            txtUser.Clear();
            txtPass.Clear();
            comboRole.ResetText();
            comboAccess.ResetText();
            Display();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("DELETE FROM Users WHERE userid = @UserID", conn))
                {
                    command.Parameters.AddWithValue("@UserID", txtID.Text);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            txtID.Clear();
            Display();
        }
        private void DisplayMenu()
        {
            try
            {
                SqlCommand command;
                string displayUsers = "SELECT * FROM Menu";
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
        private void btnMenuDisplay_Click(object sender, EventArgs e)
        {
            DisplayMenu();
        }

        private void btnMenuAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("INSERT INTO Menu (Name, Category, Description, Price) VALUES (@Name, @Category, @Description, @Price)", conn))
                {
                    command.Parameters.AddWithValue("@Name", txtMenuName.Text);
                    command.Parameters.AddWithValue("@Category", txtMenuCat.Text);
                    command.Parameters.AddWithValue("@Description", richDes.Text);
                    command.Parameters.AddWithValue("@Price", decimal.Parse(txtMenuPrice.Text));
                    

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            txtMenuName.Clear();
            txtMenuCat.Clear();
            richDes.Clear();
            txtMenuPrice.Clear();
            DisplayMenu();
        }

        private void btnMenuEdit_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("UPDATE Menu SET Name=@Name, Category=@Category, Description=@Description, Price=@Price WHERE MenuID = @MenuID", conn))
                {
                    command.Parameters.AddWithValue("@Name", txtMenuName.Text);
                    command.Parameters.AddWithValue("@Category", txtMenuCat.Text);
                    command.Parameters.AddWithValue("@Description", richDes.Text);
                    command.Parameters.AddWithValue("@Price", double.Parse(txtMenuPrice.Text));
                    command.Parameters.AddWithValue("@MenuID", txtMenuID.Text);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            txtMenuName.Clear();
            txtMenuCat.Clear();
            richDes.Clear();
            txtMenuPrice.Clear();
            txtMenuID.Clear();
            DisplayMenu();
        }

        private void btnMenuDelete_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("DELETE FROM Menu WHERE MenuID = @MenuID", conn))
                {
                    command.Parameters.AddWithValue("@MenuID", txtMenuID.Text);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            txtMenuID.Clear();
            DisplayMenu();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtID.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtUser.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtPass.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            comboRole.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            comboAccess.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMenuID.Text = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtMenuName.Text = dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtMenuCat.Text = dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString();
            richDes.Text = dataGridView2.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtMenuPrice.Text = dataGridView2.Rows[e.RowIndex].Cells[4].Value.ToString();
        }
    }
}
