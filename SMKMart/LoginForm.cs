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
    public partial class LoginForm : Form
    {
        MainForm main;

        public LoginForm(MainForm form)
        {
            InitializeComponent();
            main = form;
        }

        public static Employee employee;

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if ( tbEmail.Text == string.Empty || tbPass.Text == string.Empty )
            {
                MessageBox.Show("All Field Must Be Filled");
                return;
            }

            var db = new DatabaseDataContext();
            //var query = db.Employees.FirstOrDefault(x => x.Email == tbEmail.Text && x.Password == tbPass.Text);
            var emailEmployees = db.Employees.FirstOrDefault(x => x.Email == tbEmail.Text);
            var passEmployees = db.Employees.FirstOrDefault(x => x.Password == tbPass.Text);

            if (emailEmployees == null) 
            {
                MessageBox.Show("Email not Found");
            }

            if (passEmployees == null) 
            {
                MessageBox.Show("Incorect Password");
            }

            MessageBox.Show("Login Succesfully");
            main.IsLogin(true);
            this.Close();


        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            tbEmail.Text = "mutia@gmail.com";
            tbPass.Text = "mutia123";   
        }
    }
}
