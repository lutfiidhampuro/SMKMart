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
    public partial class ChooseProductForm : Form
    {
        DatabaseDataContext db = new DatabaseDataContext();
        ManagePromoForm promo;
        TransactionForm transactionForm;


        public ChooseProductForm(ManagePromoForm form) 
        {
            InitializeComponent();
            promo = form;
        }

        public ChooseProductForm(TransactionForm form)
        {
            InitializeComponent();
            transactionForm = form;
        }

        private void ChooseProductForm_Load(object sender, EventArgs e)
        {
            showData();
        }

        void showData()
        {
            dgvData.Columns.Clear();

            var query = db.Produks.Where(x => x.Name.Contains(tbSearch.Text) || x.ProdukId.Contains(tbSearch.Text)).Select(x => new
            {
                ProdukID = x.ProdukId,
                x.Name,
                x.Specification,
                Price = x.SellPrice

            });

            dgvData.DataSource = query;
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            showData();
        }

        private void btnChooseProduct_Click(object sender, EventArgs e)
        {
            if (dgvData.CurrentRow != null)
            {
                string id = dgvData.CurrentRow.Cells["ProdukID"].Value.ToString();
                string name = dgvData.CurrentRow.Cells["Name"].Value.ToString();
                int price = Convert.ToInt32(dgvData.CurrentRow.Cells["Price"].Value);

                //TransactionForm trx = new TransactionForm();

                transactionForm.setProdukTrx(id, name, price);
                transactionForm.Show();
                this.Close();
            }
        }
    }
}
