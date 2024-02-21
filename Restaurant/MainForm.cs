using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restaurant
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            AdminForm admin = new AdminForm();
            admin.Show();
        }

        private void btnManager_Click(object sender, EventArgs e)
        {
            ManagerForm manager = new ManagerForm();
            manager.Show();
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            StaffForm staff = new StaffForm();
            staff.Show();
        }
    }
}
