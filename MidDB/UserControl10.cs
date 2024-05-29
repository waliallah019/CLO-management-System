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
    public partial class UserControl10 : UserControl
    {
        public UserControl10()
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
                SqlCommand cmd = new SqlCommand("SELECT * FROM Assessment", con);

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

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();

                // Search for CLOs based on the provided search term
                string searchTerm = textBox4.Text.Trim();

                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    MessageBox.Show("Please enter a search term.");
                    return; // Exit the method if the search term is empty
                }

                SqlCommand searchCmd = new SqlCommand("SELECT * FROM Assessment WHERE Title LIKE @SearchTerm OR DateCreated LIKE @SearchTerm OR TotalMarks LIKE @SearchTerm OR TotalWeightage LIKE @SearchTerm", con);
                searchCmd.Parameters.AddWithValue("@SearchTerm", searchTerm); // Add wildcard characters to search for matches

                SqlDataAdapter da = new SqlDataAdapter(searchCmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0) // Check if no records are found
                {
                    MessageBox.Show("No records found for the provided search term.");
                }
                else
                {
                    // Display the search results in a DataGridView or any other appropriate control
                    dataGridView1.DataSource = dt;
                }

                // Clear the search term textbox after displaying the results
                textBox4.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        public void showData()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Assessment", con);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            sizeset();
            dataGridView1.Refresh();
        }

        private void cleardata()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";

        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();

               
                SqlCommand fetchIdCmd = new SqlCommand("SELECT Id FROM Assessment WHERE Id = @Id", con);
                fetchIdCmd.Parameters.AddWithValue("@Id", id);

                int cloId = Convert.ToInt32(fetchIdCmd.ExecuteScalar());
                if (cloId == 0)
                {
                    MessageBox.Show("Error: Assessment with this ID does not exist.");
                    cleardata();
                    return;
                }

                // Check if the textboxes are empty
                if (string.IsNullOrWhiteSpace(textBox1.Text.Trim()) || string.IsNullOrWhiteSpace(textBox2.Text.Trim()) || string.IsNullOrWhiteSpace(textBox3.Text.Trim())||string.IsNullOrWhiteSpace(textBox5.Text.Trim()))
                {
                    MessageBox.Show("Error: Please fill in all fields for updating.");
                    return;
                }

                SqlCommand updateCLOCmd = new SqlCommand("UPDATE Assessment SET Title = @Name, TotalMarks = @TotalMarks , TotalWeightage = @TotalWeightage WHERE Id = @Id", con);
                updateCLOCmd.Parameters.AddWithValue("@Name", textBox1.Text.Trim());
                updateCLOCmd.Parameters.AddWithValue("@DateCreated", textBox5.Text.Trim());
                updateCLOCmd.Parameters.AddWithValue("@TotalMarks", textBox2.Text.Trim());
                updateCLOCmd.Parameters.AddWithValue("@TotalWeightage", textBox3.Text.Trim() );
                updateCLOCmd.Parameters.AddWithValue("@Id", id);

              
                updateCLOCmd.ExecuteNonQuery();
                MessageBox.Show("Successfully updated");
                showData();

                cleardata(); 
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
            textBox5.Text = row.Cells[2].Value.ToString();
            textBox2.Text = row.Cells[3].Value.ToString();
            textBox3.Text = row.Cells[4].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please select a Assessment to delete.");
                return;
            }

            var con = Configuration.getInstance().getConnection();

            SqlCommand updateCmd = new SqlCommand("UPDATE Assessment SET Title = '&' + Title WHERE Id = @Id; UPDATE AssessmentComponent SET Name = '&' + Name WHERE AssessmentId = @Id", con);
            updateCmd.Parameters.AddWithValue("@Id", id);
            updateCmd.ExecuteNonQuery();


            MessageBox.Show("Successfully deleted ");

            cleardata();
            showData();

        }
        public void showdata()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from Assessment WHERE Title NOT LIKE '&%' ", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            sizeset();
            dataGridView1.Refresh();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            showdata();
        }

        private void button6_Click(object sender, EventArgs e)
        {
     /*       var con = Configuration.getInstance().getConnection();
            string query = @"Select * from AssessmentComponent WHere Name NOT LIKE '&%'";

            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);


            PDFgenerate.GeneratePdf(dt, "Report of CLOs of Subjects ");*/
        }
    }
}
