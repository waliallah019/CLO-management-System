using MidDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MidDB
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            
            userControl12.Hide();
            userControl21.Hide();
            userControl41.Hide();
            userControl52.Hide();
          


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
          

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
     
        private void button3_Click(object sender, EventArgs e)
        {
           /* userControl21.Show();
            userControl21.BringToFront();*/
        }

        private void userControl11_Load(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void userControl11_Load_1(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form f = new Form4();
            f.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
           
        }

        private void userControl51_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            
        }

        private void userControl21_Load(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form f = new Form4();
            f.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            userControl12.Show();
            userControl12.BringToFront();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            userControl21.Show();
            userControl21.BringToFront();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            userControl52.Show();
            userControl52.BringToFront();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            userControl41.Show();
            userControl41.BringToFront();
        }

        private void userControl12_Load(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            string query = @"Select * from Student WHERE Status = ";

            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);


            PDFgenerate.GeneratePdf(dt, "Report of the Active Students");
        }
    }
}

