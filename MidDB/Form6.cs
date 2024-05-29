using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MidDB
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();

            userControl111.Hide();
            userControl121.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            userControl111.Show();
            userControl111.BringToFront();
        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void userControl121_Load(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            userControl121.Show();
            userControl121.BringToFront();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form f = new Form4();
            f.Show();
            this.Hide();
        }
    }
}
