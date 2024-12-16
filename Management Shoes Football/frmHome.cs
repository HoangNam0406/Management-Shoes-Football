using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Management_Shoes_Football
{
    public partial class frmHome : Form
    {
        public frmHome()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            frmCustomer formHome=new frmCustomer();
            formHome.Show();
            this.Hide();
        }

        private void btnOder_Click(object sender, EventArgs e)
        {
            frmOrder formHome = new frmOrder();
            formHome.Show();
            this.Hide();
        }

        private void btnOrderDetail_Click(object sender, EventArgs e)
        {
            frmOrderDetail formHome = new frmOrderDetail();
            formHome.Show();
            this.Hide();
        }

        private void btnFootballboots_Click(object sender, EventArgs e)
        {
            frmProduct formHome = new frmProduct();
            formHome.Show();
            this.Hide();
        }

        private void btnTypeFootballboots_Click(object sender, EventArgs e)
        {
            frmTypeProduct formHome = new frmTypeProduct();
            formHome.Show();
            this.Hide();
        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            frmEmployee formHome = new frmEmployee();
            formHome.Show();
            this.Hide();
        }

        private void btnSupplier_Click(object sender, EventArgs e)
        {
            frmSupplier formHome = new frmSupplier();
            formHome.Show();
            this.Hide();
        }
    }
}
