using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Restaurant
{
    public partial class MainForm : Form
    {
        SqlConnection conn;

        public MainForm()
        {
            InitializeComponent();
            DatabaseConnection();
            CreateDatabase();
        }
        private void CreateDatabase()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'MitchelRestaurant') CREATE DATABASE MitchelRestaurant", conn))
                {
                    cmd.ExecuteNonQuery();
                }
                conn.ChangeDatabase("MitchelRestaurant");
                using (SqlCommand user = new SqlCommand("IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Users') CREATE TABLE Users (UserID INT IDENTITY(1,1) PRIMARY KEY, Username VARCHAR(50), Password VARCHAR(50), Role VARCHAR(50), AccessLevel INT)", conn))
                {
                    user.ExecuteNonQuery();
                }
                using (SqlCommand menu = new SqlCommand("IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Menu') CREATE TABLE Menu (MenuID INT IDENTITY(1,1) PRIMARY KEY, Name VARCHAR(50), Category VARCHAR(50), Description VARCHAR(255), Price DECIMAL)", conn))
                {
                    menu.ExecuteNonQuery();
                }
                using (SqlCommand order  = new SqlCommand("IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Orders') CREATE TABLE Orders (OrderID INT IDENTITY(1, 1) PRIMARY KEY, UserID INT, ItemList VARCHAR(255), TotalAmount DECIMAL, Discounts DECIMAL, CONSTRAINT FK_UserID FOREIGN KEY(UserID) REFERENCES Users(UserID))", conn))
                {
                    order.ExecuteNonQuery();
                }
                using (SqlCommand table  = new SqlCommand("IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Tables') CREATE TABLE Tables (TableID INT IDENTITY(1,1) PRIMARY KEY, Status VARCHAR(50), ReservationDetails VARCHAR(50))", conn))
                {
                    table.ExecuteNonQuery();
                }
                using (SqlCommand insert = new SqlCommand("INSERT INTO Users(Username, Password, Role, AccessLevel) VALUES ('Johndoe545', 'johndoeiscool', 'Admin', 3), ('Mary01Jane', 'Mmary@134', 'Staff', 1), ('Jane12', 'janjan', 'Manager', 2)", conn))
                {
                    insert.ExecuteNonQuery();
                }
                using (SqlCommand menus = new SqlCommand("INSERT INTO Menu(Name, Category, Description, Price) VALUES ('Fried Rice', 'Entrée', 'Plate of Fried Rice', 12.99), ('California Roll', 'Sushi', 'Sushi Roll with imitation crab, avocado, and cucumber', 13.99), ('Crab Puffs', 'Appetizer', 'Deep fried crab ragoons', 8.99)", conn))
                {
                    menus.ExecuteNonQuery();
                }
                using (SqlCommand orders = new SqlCommand("INSERT INTO Orders(UserID, ItemList, TotalAmount) VALUES (1, 'California Roll', 12.99), (2, 'Fried Rice', 13.99), (3, 'Crab puffs', 8.99)", conn))
                {
                    orders.ExecuteNonQuery();
                }
                using (SqlCommand tables = new SqlCommand("INSERT INTO Tables(Status, ReservationDetails) VALUES ('Available', 'No Reservations'), ('Occupied', 'No Reservations'), ('Reserved', 'Reservations at 6pm')", conn))
                {
                    tables.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void DatabaseConnection()
        {
            try
            {
                conn = new SqlConnection(@"Data Source=(localdb)\MSSQLLOCALDB;Initial Catalog=master;User ID=sa;Password=password");
                conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
           try
            {
                using (SqlCommand cmd = new SqlCommand("Select Role FROM Users WHERE Username = @username AND Password = @password", conn))
                {
                    cmd.Parameters.AddWithValue("@username", txtUser.Text);
                    cmd.Parameters.AddWithValue("@password", txtPassword.Text);
                    
                    object roles = cmd.ExecuteScalar();
                    if (roles != null)
                    {
                        string role = roles.ToString();
                        if (role == "Admin")
                        {
                            AdminForm admin = new AdminForm();
                            admin.Show();
                        }
                        else if (role == "Manager")
                        {
                            ManagerForm manager = new ManagerForm();
                            manager.Show();
                        }
                        else
                        {
                            StaffForm staffForm = new StaffForm();
                            staffForm.Show();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password");
                    }
                }           
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            txtUser.Clear();
            txtPassword.Clear();
        }
    }
}

