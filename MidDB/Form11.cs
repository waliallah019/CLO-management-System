using iText.StyledXmlParser.Jsoup.Select;
using PdfSharp.Pdf.Content.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static iTextSharp.awt.geom.Point2D;

namespace MidDB
{
    public partial class Form11 : Form
    {
        public Form11()
        {
            InitializeComponent();
            FetchingData();
        }
        private void sizeset()
        {
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }
        void FetchingData()
        {
            try
            {
                var con = Configuration.getInstance().getConnection();

                comboBox1.Items.Clear();


                SqlCommand cmdFetchCLOIds = new SqlCommand("SELECT Id FROM Clo  WHERE Name NOT LIKE '&%'", con);
                SqlDataReader reader = cmdFetchCLOIds.ExecuteReader();


                while (reader.Read())
                {
                    comboBox1.Items.Add(reader["Id"].ToString());
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching clo IDs: " + ex.Message);
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {


            try
            {
                string stText = comboBox1.Text.Trim();
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand(@"SELECT s.FirstName, s.LastName, s.RegistrationNumber, 
                             Clo.Name AS CloName, Clo.Id AS CloId,
                             SUM(ac.TotalMarks) AS TotalMarks, 
                             SUM(rl.MeasurementLevel) AS ObtainedLevel, 
                             MAX(rl2.MaxMeasurementLevel) AS MaxLevel,
                             ((SUM(CAST(rl.MeasurementLevel AS FLOAT)) / MAX(CAST(rl2.MaxMeasurementLevel AS FLOAT))) * SUM(CAST(ac.TotalMarks AS FLOAT)))
                             AS ObtainedMarks
                            FROM  StudentResult AS st 
                            JOIN  Student AS s ON st.StudentId = s.Id 
                            JOIN  AssessmentComponent AS ac ON ac.Id = st.AssessmentComponentId 
                            JOIN  Rubric AS r ON r.Id = ac.RubricId 
                            JOIN  RubricLevel AS rl ON rl.Id = st.RubricMeasurementId 
                            JOIN  (SELECT RubricId, MAX(MeasurementLevel) AS MaxMeasurementLevel
                                 FROM RubricLevel GROUP BY RubricId) AS rl2 ON rl2.RubricId = r.Id 
                            JOIN Assessment ON Assessment.Id = ac.AssessmentId 
                            JOIN Clo ON Clo.Id = r.CloId 
                            WHERE Clo.Name not like '!%'
                            GROUP BY  s.FirstName, s.LastName, s.RegistrationNumber, Clo.Name, Clo.Id", con);
                cmd.Parameters.AddWithValue("@CloName", stText);


                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                sizeset();


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form f = new Form4();
            f.Show();
            this.Hide();
        }

        private void Form11_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            string query = @"SELECT s.FirstName, s.LastName, s.RegistrationNumber, 
                             Clo.Name AS CloName, Clo.Id AS CloId,
                             SUM(ac.TotalMarks) AS TotalMarks, 
                             SUM(rl.MeasurementLevel) AS ObtainedLevel, 
                             MAX(rl2.MaxMeasurementLevel) AS MaxLevel,
                             ((SUM(CAST(rl.MeasurementLevel AS FLOAT)) / MAX(CAST(rl2.MaxMeasurementLevel AS FLOAT))) * SUM(CAST(ac.TotalMarks AS FLOAT)))
                             AS ObtainedMarks
                            FROM  StudentResult AS st 
                            JOIN  Student AS s ON st.StudentId = s.Id 
                            JOIN  AssessmentComponent AS ac ON ac.Id = st.AssessmentComponentId 
                            JOIN  Rubric AS r ON r.Id = ac.RubricId 
                            JOIN  RubricLevel AS rl ON rl.Id = st.RubricMeasurementId 
                            JOIN  (SELECT RubricId, MAX(MeasurementLevel) AS MaxMeasurementLevel
                                 FROM RubricLevel GROUP BY RubricId) AS rl2 ON rl2.RubricId = r.Id 
                            JOIN Assessment ON Assessment.Id = ac.AssessmentId 
                            JOIN Clo ON Clo.Id = r.CloId 
                            WHERE Clo.Name not like '&%'
                            GROUP BY  s.FirstName, s.LastName, s.RegistrationNumber, Clo.Name, Clo.Id";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);



            PDFgenerate.GeneratePdf(dt, "Report of CLO wise Result ");

        }
    }
    }

