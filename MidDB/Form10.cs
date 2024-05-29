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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MidDB
{
    public partial class Form10 : Form
    {
        public Form10()
        {
            InitializeComponent();
            FetchingData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form f = new Form4();
            f.Show();
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form10_Load(object sender, EventArgs e)
        {
            
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

             
                SqlCommand cmdFetchCLOIds = new SqlCommand("SELECT Id FROM Assessment  WHERE Title NOT LIKE '&%'", con);
                SqlDataReader reader = cmdFetchCLOIds.ExecuteReader();

           
                while (reader.Read())
                {
                    comboBox1.Items.Add(reader["Id"].ToString());
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching Assessment IDs: " + ex.Message);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string stText = comboBox1.Text.Trim();
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT stud.FirstName ,stud.LastName, stud.RegistrationNumber, " +
             "Assessment.Title, assess.Name AS AssessmentComponent, assess.TotalMarks AS Totalmarks, " +
             "rl.MeasurementLevel AS ObtainedLevel, MAX(rl2.MeasurementLevel) AS MaxLevel, " +
             "CAST(CAST(rl.MeasurementLevel AS FLOAT) / MAX(CAST(rl2.MeasurementLevel AS FLOAT)) * assess.TotalMarks AS FLOAT) AS ObtainedMarks " +
            "FROM StudentResult AS st " +
            "JOIN Student AS stud ON st.StudentId = stud.Id " +
             "JOIN AssessmentComponent AS assess ON assess.Id = st.AssessmentComponentId " +
             "JOIN Rubric AS r ON r.Id = assess.RubricId " +
             "JOIN RubricLevel AS rl ON rl.Id = st.RubricMeasurementId " +
             "JOIN RubricLevel AS rl2 ON rl2.RubricId = r.Id " +
              "JOIN Assessment ON Assessment.Id = assess.AssessmentId " +
              "WHERE Assessment.Id = @Assesstitle " +
              "GROUP BY assess.Name, assess.TotalMarks, rl.MeasurementLevel, stud.FirstName, stud.LastName,stud.RegistrationNumber, Assessment.Title", con);
                cmd.Parameters.AddWithValue("@Assesstitle", stText);


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

        private void button3_Click(object sender, EventArgs e)
        {
            string stText = comboBox1.Text.Trim();
            var con = Configuration.getInstance().getConnection();

            string query = @"SELECT stud.FirstName, stud.LastName, stud.RegistrationNumber, 
       Assessment.Title, AssessmentComponent.Name AS AssessmentComponent, 
       AssessmentComponent.TotalMarks AS TotalMarks, 
       rl.MeasurementLevel AS ObtainedLevel, 
       MAX(rl2.MeasurementLevel) AS MaxLevel, 
       CAST(CAST(rl.MeasurementLevel AS FLOAT) / MAX(CAST(rl2.MeasurementLevel AS FLOAT)) * AssessmentComponent.TotalMarks AS FLOAT) AS ObtainedMarks 
FROM StudentResult AS st 
JOIN Student AS stud ON st.StudentId = stud.Id 
JOIN AssessmentComponent AS AssessmentComponent ON AssessmentComponent.Id = st.AssessmentComponentId 
JOIN Rubric AS r ON r.Id = AssessmentComponent.RubricId 
JOIN RubricLevel AS rl ON rl.Id = st.RubricMeasurementId 
JOIN RubricLevel AS rl2 ON rl2.RubricId = r.Id 
JOIN Assessment ON Assessment.Id = AssessmentComponent.AssessmentId 
WHERE Assessment.Id = @Assesstitle 
GROUP BY AssessmentComponent.Name, AssessmentComponent.TotalMarks, rl.MeasurementLevel, stud.FirstName, stud.LastName, stud.RegistrationNumber, Assessment.Title
";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Assesstitle", stText);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            PDFgenerate.GeneratePdf(dt, "Report of Assessment wise Result");

        }
    }
}
    
