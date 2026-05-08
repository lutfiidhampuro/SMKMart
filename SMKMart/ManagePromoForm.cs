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
    public partial class ManagePromoForm : Form
    {
        DatabaseDataContext db = new DatabaseDataContext();
        string id = "";

        public ManagePromoForm()
        {
            InitializeComponent();
        }

        private void ManagePromoForm_Load(object sender, EventArgs e)
        {
            ShowData();
            tbId.Enabled = false;
            tbName.Enabled = false;
            enableButtonAndFields(false);
            cbPromoType.Items.Add("FREE");
            cbPromoType.Items.Add("DISC");
            cbPromoType.SelectedIndex = 0;
        }

        void ShowData()
        {
            dgvData.Columns.Clear();
                var query = db.Promos.Select(x => new
                {
                    PromoID = x.PromoId,                   
                    x.ProdukId,
                    ProdukName = x.Produk.Name,
                    x.BuyQty,
                    x.Type,
                    x.Reward,
                    x.StartDate,
                    x.EndDate
                });

            dgvData.DataSource = query;
        }

        void clearFields()
        {
            tbBuyQty.Text = "";
            cbPromoType.Text = "";
            tbReward.Text = "";
            dtpStartDate.Text = "";
            dtpEndDate.Text = "";
        }

        void enableFields(bool e)
        {
            tbBuyQty.Enabled = !e;
            tbName.Enabled = !e;
            tbReward.Enabled = !e;
            cbPromoType.Enabled = !e;
            dtpStartDate.Enabled = !e;
            dtpEndDate.Enabled = !e;
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

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (id == "")
            {
                MessageBox.Show("Please select your data");
                return;
            }
            id = "";
            enableButtonAndFields(true);
            enableButtonAndFields(true);
            clearFields();
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            enableButtonAndFields(false);
            clearFields();

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (id == "")
            {
                MessageBox.Show("Please select your data");
                return;
            }

            enableButtonAndFields(true);
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                id = dgvData.Rows[e.RowIndex].Cells["PromoID"].Value.ToString();

                var query = db.Promos.FirstOrDefault(x => x.PromoId.ToString() == id);

                if (query != null)
                {
                    tbId.Text = query.ProdukId;
                    tbName.Text = query.Produk.Name;
                    tbBuyQty.Text = query.BuyQty.ToString();
                    tbReward.Text = query.Reward.ToString();
                    dtpStartDate.Value = query.StartDate ?? DateTime.Now;
                    dtpEndDate.Value = query.EndDate ?? DateTime.Now;

                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (tbBuyQty.Text == string.Empty || tbName.Text == string.Empty || tbReward.Text == string.Empty)
            {
                MessageBox.Show("All Fields Must Be Filled");
                return;
            }

            if (dtpEndDate.Value.Date < DateTime.Now)
            {
                MessageBox.Show("Promo date must be greater than today");
                return ;
            }


            if (id != "")
            {
                var queryUpdate = db.Promos.FirstOrDefault(x => x.PromoId.ToString() == id);

                if (queryUpdate != null)
                {
                    queryUpdate.Produk.Name = tbName.Text;
                    queryUpdate.BuyQty = Convert.ToInt32(tbBuyQty.Text);
                    queryUpdate.Type = cbPromoType.Text;
                    queryUpdate.Reward = Convert.ToInt32(tbReward.Text);
                    queryUpdate.StartDate = dtpStartDate.Value;
                    queryUpdate.EndDate = dtpEndDate.Value;

                    db.SubmitChanges();
                    MessageBox.Show("Update Data Successfully");
                    enableButtonAndFields(false);
                    clearFields();
                    ShowData();
                    id = "";
                    return;
                }
            }

            var queryNew = new Promo();

            queryNew.ProdukId = tbId.Text;
            queryNew.BuyQty = Convert.ToInt32(tbBuyQty.Text);
            queryNew.Type = cbPromoType.Text;
            queryNew.Reward = Convert.ToInt32(tbReward.Text);
            queryNew.StartDate = dtpStartDate.Value;
            queryNew.EndDate = dtpEndDate.Value;

            db.Promos.InsertOnSubmit(queryNew);
            db.SubmitChanges();
            MessageBox.Show("Insert data successfully");
            enableButtonAndFields(false);
            clearFields();
            //generatedId();
            ShowData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new ChooseProductForm(this).Show();
        }
    }
}
