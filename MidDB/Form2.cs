﻿using System;
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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            userControl31.Hide();
            userControl61.Hide();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            userControl61.Show();
            userControl61.BringToFront();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form f = new Form4();
            f.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            userControl31.Show();
            userControl31.BringToFront();
        }

        private void userControl31_Load(object sender, EventArgs e)
        {

        }

        private void userControl61_Load(object sender, EventArgs e)
        {

        }
    }
}
