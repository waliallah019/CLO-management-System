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
    public partial class UserControl12 : UserControl
    {
        public UserControl12()
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
                SqlCommand cmd = new SqlCommand("SELECT * FROM RubricLevel", con);

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

        public void showdata()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT * FROM RubricLevel", con);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            sizeset();
            dataGridView1.Refresh();
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

                SqlCommand searchCmd = new SqlCommand("SELECT * FROM RubricLevel WHERE Details Like @SearchTerm", con);
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

                // Fetch Id from the database
                SqlCommand fetchIdCmd = new SqlCommand("SELECT Id FROM RubricLevel WHERE Id = @Id", con);
                fetchIdCmd.Parameters.AddWithValue("@Id", id);

                int rubricLevelId = Convert.ToInt32(fetchIdCmd.ExecuteScalar());
                if (rubricLevelId == 0)
                {
                    MessageBox.Show("Error: Rubric level with this ID does not exist.");
                    cleardata();
                    return;
                }

             
                if (string.IsNullOrWhiteSpace(textBox1.Text.Trim()) ||
                    string.IsNullOrWhiteSpace(textBox2.Text.Trim()) ||
                    string.IsNullOrWhiteSpace(textBox3.Text.Trim()))
                {
                    MessageBox.Show("Error: Please fill in all fields for updating.");
                    return;
                }

                int measurementLevel;
                if (!int.TryParse(textBox3.Text.Trim(), out measurementLevel) || measurementLevel < 1 || measurementLevel > 4)
                {
                    MessageBox.Show("Error: Measurement level must be between 1 and 4.");
                    return;
                }

              
                SqlCommand checkExistenceCmd = new SqlCommand("SELECT COUNT(*) FROM RubricLevel WHERE RubricId = @RubricId AND MeasurementLevel = @MeasurementLevel AND Id != @Id", con);
                checkExistenceCmd.Parameters.AddWithValue("@RubricId", textBox1.Text.Trim());
                checkExistenceCmd.Parameters.AddWithValue("@MeasurementLevel", measurementLevel);
                checkExistenceCmd.Parameters.AddWithValue("@Id", id);

                int existingCount = Convert.ToInt32(checkExistenceCmd.ExecuteScalar());
                if (existingCount > 0)
                {
                    MessageBox.Show("Error: Rubric level with the same ID and measurement level already exists. Please choose another level.");
                    return;
                }

                
                SqlCommand updateCmd = new SqlCommand("UPDATE RubricLevel SET RubricId = @RubricId, Details = @Details, MeasurementLevel = @MeasurementLevel WHERE Id = @Id", con);
                updateCmd.Parameters.AddWithValue("@RubricId", textBox1.Text.Trim());
                updateCmd.Parameters.AddWithValue("@Details", textBox2.Text.Trim());
                updateCmd.Parameters.AddWithValue("@MeasurementLevel", measurementLevel);
                updateCmd.Parameters.AddWithValue("@Id", id);

                updateCmd.ExecuteNonQuery();

                MessageBox.Show("Successfully updated");
                showdata();
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
            textBox2.Text = row.Cells[2].Value.ToString();
            textBox3.Text = row.Cells[3].Value.ToString();  
        }
        public void showData()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from RubricLevel WHere Details NOT LIKE '&%' ", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            sizeset();
            dataGridView1.Refresh();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please select a Rubric level  to delete.");
                return;
            }

            var con = Configuration.getInstance().getConnection();

            SqlCommand updateCmd = new SqlCommand("UPDATE RubricLevel SET Details = '&' + Details WHERE Id = @Id", con);
            updateCmd.Parameters.AddWithValue("@Id", id);
            updateCmd.ExecuteNonQuery();


            MessageBox.Show("Successfully deleted ");

            cleardata();
            showData();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            showData();
        }
    }
}
