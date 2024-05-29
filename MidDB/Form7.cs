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
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();


            userControl131.Hide();
            userControl141.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            userControl131.Show();
            userControl131.BringToFront();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            userControl141.Show();
            userControl141.BringToFront();
        }

        private void userControl141_Load(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form f = new Form4();
            f.Show();
            this.Hide();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
