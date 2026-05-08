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

            var promo = db.Promos.FirstOrDefault(x =>
            x.ProdukId == id
            );

            if (promo != null)
            {
                if (promo.Type == "FREE")
                {
                    lblPromo.Text =
                        $"Buy {promo.BuyQty} Free {promo.Reward}";
                }
                else if (promo.Type == "DISC")
                {
                    lblPromo.Text =
                        $"Buy {promo.BuyQty} Discount {promo.Reward}";
                }
            }
            else
            {
                lblPromo.Text = "-";
            }
        }

        void generatedId()
        {
            var query = db.DetailTrxes.OrderByDescending(x => x.TrxId).FirstOrDefault()?.TrxId;
            var substring = query != null ? (Convert.ToInt32(query.Substring(5, 5)) + 1) : 1;
            var id = $"{DateTime.Now:yyyyMM}{substring.ToString().PadLeft(5, '0')}";

            lblTrxID.Text = id;
        }

        private void TransactionForm_Load(object sender, EventArgs e)
        {
            dgvData.Columns.Add("ProductID", "ProductID");
            dgvData.Columns.Add("Name", "Name");
            dgvData.Columns.Add("Qty", "Qty");
            dgvData.Columns.Add("Price", "Price");
            dgvData.Columns.Add("Subtotal", "Subtotal");
            tbId.Enabled = false;
            tbName.Enabled = false;
            tbPrice.Enabled = false;
            generatedId();
            lblDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new ChooseProductForm(this).Show();
            this.Hide();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (tbId.Text == "" || tbQty.Text == "")
            {
                MessageBox.Show("Please choose product and fill qty");
                return;
            }

            string productId = tbId.Text;
            string name = tbName.Text;

            int qty = Convert.ToInt32(tbQty.Text);
            int price = Convert.ToInt32(tbPrice.Text);

            int subtotal = qty * price;

            var promo = db.Promos.FirstOrDefault(x =>
                   x.ProdukId == productId
               );

            if (promo != null)
            {
                if (promo.Type == "DISC")
                {
                    if (qty >= promo.BuyQty)
                    {
                        subtotal -= Convert.ToInt32(promo.Reward);
                    }
                }
                else if (promo.Type == "FREE")
                {
                    if (qty >= promo.BuyQty)
                    {
                        int freeQty = Convert.ToInt32(promo.Reward);

                        subtotal =
                            (qty - freeQty) * price;
                    }
                }
            }

            dgvData.Rows.Add
                (
                    productId,
                    name,
                    qty,
                    price,
                    subtotal
                );

            calculateTotal();
        }

        void calculateTotal()
        {
            int total = 0;

            foreach (DataGridViewRow row in dgvData.Rows)
            {
                if (row.Cells["Subtotal"].Value != null)
                {
                    total += Convert.ToInt32(
                        row.Cells["Subtotal"].Value
                    );
                }
            }

            lblTotal.Text = $"{total.ToString("N0").Replace(",", ".")}";
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            tbId.Text = "";
            tbName.Text = "";
            tbPrice.Text = "";
            lblPromo.Text = "";
            tbQty.Text = "";
        }
    }
}
