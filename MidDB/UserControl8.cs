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
    public partial class UserControl8 : UserControl
    {
        public UserControl8()
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
                SqlCommand cmd = new SqlCommand("SELECT * FROM Rubric", con);

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

                // Search for Rubrics based on the provided search term
                string searchTerm = textBox4.Text.Trim();

                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    MessageBox.Show("Please enter a search term.");
                    return; // Exit the method if the search term is empty
                }

                SqlCommand searchCmd = new SqlCommand("SELECT * FROM Rubric WHERE Id LIKE @SearchTerm OR Details Like @SearchTerm OR CloId Like @SearchTerm", con);
                searchCmd.Parameters.AddWithValue("@SearchTerm",searchTerm ); 

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
        private void cleardata()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";

        }
        public void showdata()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Clo", con);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            sizeset();
            dataGridView1.Refresh();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text.Trim()) || string.IsNullOrWhiteSpace(textBox2.Text.Trim()) || string.IsNullOrWhiteSpace(textBox3.Text.Trim()))
            {
                MessageBox.Show("Error: Please fill in all fields for updating.");
                return;
            }
            int rubricId = Convert.ToInt32(textBox1.Text.Trim());
            string details = textBox3.Text.Trim();
            int cloId = Convert.ToInt32(textBox2.Text.Trim());
          
            try
            {
                var con = Configuration.getInstance().getConnection();
                // Check if the textboxes are empty
               
                // Construct the update query for Rubric
                string updateQuery = "UPDATE Rubric SET Details = @Details, CloId = @CloId WHERE Id = @RubricId";

                    using (SqlCommand command = new SqlCommand(updateQuery, con))
                    {
                        // Add parameters to the query
                        command.Parameters.AddWithValue("@Details", details);
                        command.Parameters.AddWithValue("@CloId", cloId);
                        command.Parameters.AddWithValue("@RubricId", rubricId);

                        // Execute the update command for Rubric
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Rubric successfully updated");
                        cleardata();
                        }
                        else
                        {
                            MessageBox.Show("No records updated. Rubric with the provided ID may not exist.");
                        cleardata();
                        }
                    }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating rubric: " + ex.Message);
            }
        }
        int indexrow;

        string id;
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            indexrow = e.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[indexrow];

          
            textBox1.Text = row.Cells[0].Value.ToString();
            textBox3.Text = row.Cells[1].Value.ToString();
            textBox2.Text = row.Cells[2].Value.ToString();
        }
        public void showData()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from Rubric WHere Details NOT LIKE '&%' ", con);
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
                MessageBox.Show("Please select a Rubric to delete.");
                return;
            }

            var con = Configuration.getInstance().getConnection();

            SqlCommand updateCmd = new SqlCommand("UPDATE Rubric SET Details = '&' + Details WHERE Id = @Id; UPDATE RubricLevel SET Details = '&' + Details WHERE RubricId = @Id ", con);
            updateCmd.Parameters.AddWithValue("@Id", textBox1.Text);
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
