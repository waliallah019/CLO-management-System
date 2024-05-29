using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace MidDB
{
    public partial class Form8 : Form
    {
        public Form8()
        {
            InitializeComponent();
            userControl161.Hide();
            userControl151.Hide();
            userControl171.Hide();
        }

        private void Form8_Load(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form f = new Form4();
            f.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            userControl161.Show();
            userControl161.BringToFront();
            userControl161.showdata();
        }

        private void userControl161_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            userControl151.Show();
            userControl151.BringToFront();
            userControl151.showData();

        }

        private void userControl171_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            userControl171.Show();
            userControl171.BringToFront(); ;
            userControl171.showData();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            string query = @"SELECT 
                    (SELECT RegistrationNumber FROM Student WHERE Id=S.Id) AS 'Registration Number',
                    COUNT(*) AS 'Total Attended that attend class'
                FROM 
                    StudentAttendance SA
                    JOIN Student S ON S.Id = SA.StudentId
                GROUP BY 
                    S.Id";

            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            

            PDFgenerate.GeneratePdf(dt, "Attendance Report of the class");


        }

        private void userControl171_Load_1(object sender, EventArgs e)
        {

        }
    }
    }

