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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            userControl71.Hide();
            userControl82.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form f = new Form4();
            f.Show();
            this.Hide();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            userControl71.Show();
            userControl71.BringToFront();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            userControl82.Show();
            userControl82.BringToFront();   
        }

        private void userControl82_Load(object sender, EventArgs e)
        {

        }
    }
}
