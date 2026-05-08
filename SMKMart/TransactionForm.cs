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
    public partial class TransactionForm : Form
    {
        DatabaseDataContext db = new DatabaseDataContext();
        public TransactionForm()
        {
            InitializeComponent();
        }

        public void setProdukTrx(string id, string name, int price)
        {
            tbId.Text = id;
            tbName.Text = name;
            tbPrice.Text = price.ToString();
        }

        void generatedId()
        {
            var query = db.DetailTrxes.OrderByDescending(x => x.TrxId).FirstOrDefault()?.TrxId;
            var substring = query != null ? (Convert.ToInt32(query.Substring(5, 5)) + 1) : 1;
            var id = $"{DateTime.Now:yyyyMM}{substring.ToString().PadLeft(5, '0')}";

            lblTrxID.Text = id;
        }

        void showData()
        {
            dgvData.Columns.Clear();
            
            var query = db.DetailTrxes.Select(x => new
            {
                ProductID = x.ProdukId,
                x.Produk.Name,
                x.Qty,
                x.Produk.SellPrice,
            }).ToList();

            dgvData.DataSource = query;
        }

        private void TransactionForm_Load(object sender, EventArgs e)
        {
            tbId.Enabled = false;
            tbName.Enabled = false;
            tbPrice.Enabled = false;
            showData();
            generatedId();
            lblDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        void promo()
        {  
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new ChooseProductForm(this).Show();
            this.Hide();
        }
    }
}
