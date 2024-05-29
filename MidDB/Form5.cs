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
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
            userControl91.Hide();
            userControl101.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form f = new Form4();
            f.Show();
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            userControl91.Show();
            userControl91.BringToFront();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            userControl101.Show();
            userControl101.BringToFront();
        }
    }
}
