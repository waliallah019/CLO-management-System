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
    public partial class Form9 : Form
    {
        int assesId = 0;
        int studenttId = 0;
        int datId = 0;
        public Form9()
        {
            InitializeComponent();
            FetchingId();
            showanyData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form f = new Form4();
            f.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();

            SqlCommand cmdCLO = new SqlCommand("SELECT RL.Id, RL.Details, RL.MeasurementLevel " +
                "FROM AssessmentComponent AC " +
                "JOIN Rubric R ON AC.RubricId = R.Id " +
                "JOIN RubricLevel Rl ON RL.RubricId = R.Id " +
                "WHERE AC.Id = @ACId", con);
            string asessmentIdString = comboBox2.Text;
            if (string.IsNullOrWhiteSpace(asessmentIdString))
            {
                MessageBox.Show("Please select an assessment component");
                return;
            }
            assesId = int.Parse(asessmentIdString);
            cmdCLO.Parameters.AddWithValue("@ACId", assesId);

            SqlDataAdapter da = new SqlDataAdapter(cmdCLO);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView3.DataSource = dt;
            sizeset();
            dataGridView1.Refresh();
        }
        private void sizeset()
        {
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }
        public void showanyData()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from Student where status=5 ", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            sizeset();
            dataGridView1.Refresh();
        }
        void FetchingId()
        {
            try
            {
                var con = Configuration.getInstance().getConnection();


                comboBox1.Items.Clear();


                SqlCommand cmdFetchCLOIds = new SqlCommand("SELECT Title FROM  Assessment  WHERE Title NOT LIKE '&%'", con);
                SqlDataReader reader = cmdFetchCLOIds.ExecuteReader();

                while (reader.Read())
                {
                    comboBox1.Items.Add(reader["Title"].ToString());
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching Assessment  IDs: " + ex.Message);
            }

        }
        private void Form9_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        

        private void comboBox1_VisibleChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();


                comboBox2.Items.Clear();
                string data = comboBox1.Text;

                SqlCommand cmdFetchCLOIds = new SqlCommand("SELECT AC.Id FROM  AssessmentComponent AC join Assessment A on AC.AssessmentId=A.Id WHere A.Title = @Title And Name NOT LIKE '&%' ", con);
                cmdFetchCLOIds.Parameters.AddWithValue("@Title", data);
                SqlDataReader reader = cmdFetchCLOIds.ExecuteReader();
                while (reader.Read())
                {
                    comboBox2.Items.Add(reader["Id"].ToString());
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching AssessmentComponent  IDs: " + ex.Message);
            }
        }

        int indexrow;
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            indexrow = e.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[indexrow];
            string idstring = row.Cells[0].Value.ToString();
            if (string.IsNullOrWhiteSpace(idstring))
            {
                MessageBox.Show("Please select a valid column");
                return;
            }

            studenttId = int.Parse(idstring);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();

            SqlCommand cmd = new SqlCommand("Insert INTO StudentResult " +
                "VALUES (@StudentId, @AssessmentComponentId, @RubricMeasurementId, @EvaluationDate)", con);

            if (studenttId == 0)
            {
                MessageBox.Show("Please select a student");
                return;
            }
            cmd.Parameters.AddWithValue("@StudentId", studenttId);

            if (assesId == 0)
            {
                MessageBox.Show("Please select an assessment component");
                return;
            }

            cmd.Parameters.AddWithValue("@AssessmentComponentId", assesId);

            if (datId == 0)
            {
                MessageBox.Show("Please select an rubric level from tabel");
                return;
            }
            cmd.Parameters.AddWithValue("@RubricMeasurementId", datId);

            cmd.Parameters.AddWithValue("@EvaluationDate", DateTime.Now);
            cmd.ExecuteNonQuery();
            ShowEvaluatedData();
            MessageBox.Show("Evaluated Successfully");
        }
        private void ShowEvaluatedData()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from StudentResult ", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView2.DataSource = dt;
            sizeset();
            dataGridView1.Refresh();
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            indexrow = e.RowIndex;
            DataGridViewRow row = dataGridView3.Rows[indexrow];
            string idstring = row.Cells[0].Value.ToString();
            string dat = row.Cells[2].Value.ToString();
            if (string.IsNullOrWhiteSpace(idstring))
            {
                MessageBox.Show("Please select a valid column");
                return;
            }
            textBox3.Text = dat;
            datId = int.Parse(idstring);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from StudentResult ", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            PDFgenerate.GeneratePdf(dt, "Report of Student Achievement");
        }
    }
}
