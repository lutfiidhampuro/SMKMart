using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMKMart
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //new LoginForm().Show();
            //this.Hide();
            
            LoginForm form = new LoginForm(this);
            form.Show();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            IsLogin(false);
        }

        public void IsLogin (bool status)
        {
            loginToolStripMenuItem.Visible = !status;

            masterToolStripMenuItem.Visible = status;
            logoutToolStripMenuItem.Visible = status;
            viewToolStripMenuItem.Visible = status;
            //memberToolStripMenuItem.Visible = status;
            //promoToolStripMenuItem.Visible = status;


        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IsLogin(false);
            MessageBox.Show("Logout Succesfully");
        }

        private void memberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ManageMemberForm().Show();
        }

        private void promoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ManagePromoForm().Show();
        }

        private void transactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new TransactionForm().Show();
        }
    }
}
