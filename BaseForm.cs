using OrderManager.ContentPanel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrderManager
{
    public partial class BaseForm : Form
    {
        public BaseForm()
        {
            InitializeComponent();
        }

        private void AddNewPanel(UserControl newPanel)
        {
            
            //clear controls
            pnlBase.Controls.Clear();
            newPanel.Dock = DockStyle.Fill;
            //add as pnlBase ctr
            pnlBase.Controls.Add(newPanel);


        }


        private void BaseForm_Load(object sender, EventArgs e)
        {

        }

        private void btnHome_Click(object sender, EventArgs e)
        {

        }

        private void btnProduct_Click(object sender, EventArgs e)
        {

            AddNewPanel(new Product());
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            AddNewPanel(new Customer());
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            AddNewPanel(new Order());
        }
    }
}
