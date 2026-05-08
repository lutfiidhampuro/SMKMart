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
    public partial class ManageMemberForm : Form
    {
        DatabaseDataContext db = new DatabaseDataContext();
        string id = "";

        public ManageMemberForm()
        {
            InitializeComponent();
        }

        private void ManageMemberForm_Load(object sender, EventArgs e)
        {
            tbId.Enabled = false;
            showData();
            enableButtonAndFields(false);
        }

        void showData()
        {
            dgvData.Columns.Clear();

            var query = db.Members.Select(x => new {
                MemberID = x.MemberId, 
                x.Name,
                x.Email,
                x.Handphone,
                x.Expired
            });

            dgvData.DataSource =  query;

        }

        void generatedId()
        {
            var query = db.Members.OrderByDescending(x => x.MemberId).FirstOrDefault()?.MemberId;
            var substring = query != null ? (Convert.ToInt32(query.Substring(5, 5)) + 1) : 1;
            var id = $"M{2018}{substring.ToString().PadLeft(5, '0')}";

            tbId.Text = id;
        }

        void enableFields(bool e)
        {
            tbEmail.Enabled = !e;
            tbName.Enabled = !e;
            tbHandphone.Enabled = !e;
            dtpExpired.Enabled = !e;
        }

        void enableButton(bool e)
        {
            btnInsert.Enabled = e;
            btnUpdate.Enabled = e;
            btnSave.Enabled = !e;
            btnCancel.Enabled = !e;
        }

        void enableButtonAndFields(bool e)
        {
            enableFields(!e);
            enableButton(!e);
        }

        void clearFields()
        {
            tbEmail.Text = "";
            tbName.Text = "";
            tbHandphone.Text = "";
            dtpExpired.Text = "";
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            id = "";
            enableButtonAndFields(true);
            generatedId();
            clearFields();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            enableButtonAndFields(false);
            clearFields();
            tbId.Text = "";
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                id = dgvData.Rows[e.RowIndex].Cells["MemberID"].Value.ToString();

                var query = db.Members.FirstOrDefault(x => x.MemberId == id);

                if (query != null)
                {
                    tbId.Text = query.MemberId;
                    tbName.Text = query.Name;
                    tbEmail.Text = query.Email;
                    tbHandphone.Text = query.Handphone;
                    dtpExpired.Value = query.Expired ?? DateTime.Now;

                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (id == "")
            {
                MessageBox.Show("Please Select Your Data");
                return;
            }

            enableButtonAndFields(true);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (tbEmail.Text == string.Empty || tbName.Text == string.Empty || tbHandphone.Text == string.Empty)
            {
                MessageBox.Show("All Fields Must Be Filled");
                return;
            }

            if (!tbEmail.Text.Contains("@"))
            {
                MessageBox.Show("Email Must Be Valid Format");
                return;
            }

            if (!tbHandphone.Text.All(char.IsDigit))
            {
                MessageBox.Show("Phone Number must be numeric");
                return;
            }

            if (tbHandphone.Text.Length < 10 || tbHandphone.Text.Length > 12)
            {
                MessageBox.Show("Phone Number must between 10-12 digits");
                return;
            }

            if (dtpExpired.Value.Date < DateTime.Now) 
            {
                MessageBox.Show("Expiry date must be greater than today");
                return;
            }

            if (id != "")
            {
                var queryUpdate = db.Members.FirstOrDefault(x => x.MemberId == id);

                if (queryUpdate != null)
                {
                    queryUpdate.Name = tbName.Text;
                    queryUpdate.Email = tbEmail.Text;
                    queryUpdate.Handphone = tbHandphone.Text;
                    queryUpdate.Expired = dtpExpired.Value;

                    db.SubmitChanges();
                    MessageBox.Show("Update Data Successfully");
                    enableButtonAndFields(false);
                    clearFields();
                    generatedId();
                    showData();
                    id = "";
                    return;

                }
            }

            var queryNew = new Member();
            queryNew.MemberId = tbId.Text;
            queryNew.Name = tbName.Text;
            queryNew.Email = tbEmail.Text;
            queryNew.Handphone = tbHandphone.Text;
            queryNew.Expired = dtpExpired.Value;

            db.Members.InsertOnSubmit(queryNew);
            db.SubmitChanges();
            MessageBox.Show("Insert data successfully");
            enableButtonAndFields(false);
            clearFields();
            generatedId();
            showData();
        }
    }
}
