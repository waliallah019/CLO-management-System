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
    public partial class UserControl14 : UserControl
    {
        public UserControl14()
        {
            InitializeComponent();
            dataGridView1.Refresh();
        }
        private void sizeset()
        {
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT * FROM AssessmentComponent", con);

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

        private void UserControl14_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();


                string searchTerm = textBox4.Text.Trim();

                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    MessageBox.Show("Please enter a search term.");
                    return; // Exit the method if the search term is empty
                }

                SqlCommand searchCmd = new SqlCommand("SELECT * FROM AssessmentComponent WHERE Name Like @SearchTerm", con);
                searchCmd.Parameters.AddWithValue("@SearchTerm", searchTerm);

                SqlDataAdapter da = new SqlDataAdapter(searchCmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No records found for the provided search term.");
                }
                else
                {

                    dataGridView1.DataSource = dt;
                }


                textBox4.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please select a assessment component to delete.");
                return;
            }

            var con = Configuration.getInstance().getConnection();

            SqlCommand updateCmd = new SqlCommand("UPDATE AssessmentComponent SET Name = '&' + Name WHERE Id = @Id", con);
            updateCmd.Parameters.AddWithValue("@Id", id);
            updateCmd.ExecuteNonQuery();


            MessageBox.Show("Successfully deleted ");

            cleardata();
            showData();
        }
        private void cleardata()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
        }
        public void showData()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT * FROM AssessmentComponent", con);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            sizeset();
            dataGridView1.Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();

                // Fetch Id from the database
                SqlCommand fetchIdCmd = new SqlCommand("SELECT Id FROM AssessmentComponent WHERE Id = @Id", con);
                fetchIdCmd.Parameters.AddWithValue("@Id", id);

                // Check if the textboxes are empty
                if (string.IsNullOrWhiteSpace(textBox1.Text.Trim()) || string.IsNullOrWhiteSpace(textBox2.Text.Trim()) || string.IsNullOrWhiteSpace(textBox3.Text.Trim()))
                {
                    MessageBox.Show("Error: Please fill in all fields for updating.");
                    return;
                }

                SqlCommand updateCLOCmd = new SqlCommand("UPDATE AssessmentComponent SET Name = @Name, RubricId= @RubricId,TotalMarks=@TotalMarks ,DateUpdated = GETDATE(), AssessmentId = @AssessmentId WHERE Id = @Id", con);
                updateCLOCmd.Parameters.AddWithValue("@Name", textBox1.Text.Trim());
                updateCLOCmd.Parameters.AddWithValue("@RubricId", textBox2.Text.Trim());
                updateCLOCmd.Parameters.AddWithValue("@TotalMarks", textBox3.Text.Trim());
                updateCLOCmd.Parameters.AddWithValue("@DateCreated", textBox5.Text.Trim());
                updateCLOCmd.Parameters.AddWithValue("@DateUpdated", textBox6.Text.Trim());
                updateCLOCmd.Parameters.AddWithValue("@AssessmentId", textBox7.Text.Trim());
                updateCLOCmd.Parameters.AddWithValue("@Id", id);

               
                updateCLOCmd.ExecuteNonQuery();
                MessageBox.Show("Successfully updated");
                showData();

                cleardata(); // Assuming this method clears form fields after updating
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        int indexrow;

        string id;
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            indexrow = e.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[indexrow];

            id = row.Cells[0].Value.ToString();
            textBox1.Text = row.Cells[1].Value.ToString();
            textBox2.Text = row.Cells[2].Value.ToString();
            textBox3.Text = row.Cells[3].Value.ToString();
            textBox5.Text = row.Cells[4].Value.ToString();  
            textBox6.Text = row.Cells[5].Value.ToString();
            textBox7.Text = row.Cells[6].Value.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
           
            
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("Select * from AssessmentComponent WHERE Name NOT LIKE '&%' ", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                sizeset();
                dataGridView1.Refresh();
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from AssessmentComponent WHERE Name NOT LIKE '&%' ", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);



            PDFgenerate.GeneratePdf(dt, "Report of Assessment Components of Subjects ");
        }
    }
}
