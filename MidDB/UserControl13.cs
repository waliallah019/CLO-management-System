using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MidDB
{
    public partial class UserControl13 : UserControl
    {
        public UserControl13()
        {
            InitializeComponent();
            fetching();
            fetching1();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
        void fetching()
        {
            try
            {
                var con = Configuration.getInstance().getConnection();

              
                comboBox1.Items.Clear();

                SqlCommand cmdFetchCLOIds = new SqlCommand("SELECT Id FROM Rubric WHERE Details NOT LIKE '&%'", con);
                SqlDataReader reader = cmdFetchCLOIds.ExecuteReader();

               
                while (reader.Read())
                {
                    comboBox1.Items.Add(reader["Id"].ToString());
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching IDs: " + ex.Message);
            }

        }


        void fetching1()
        {
            try
            {
                var con = Configuration.getInstance().getConnection();


                comboBox2.Items.Clear();

                SqlCommand cmdFetchCLOIds = new SqlCommand("SELECT Id FROM Assessment WHERE Title NOT LIKE '&%'", con);
                SqlDataReader reader = cmdFetchCLOIds.ExecuteReader();


                while (reader.Read())
                {
                    comboBox2.Items.Add(reader["Id"].ToString());
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching IDs: " + ex.Message);
            }

        }
        private void cleardata()
        {

            comboBox1.Text = "";
            textBox3.Text = "";
            textBox1.Text = "";
            comboBox2.Text = "";
            
        }
        private bool IsAllDigits(string str)
        {
            foreach (char c in str)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }

     
        private void button2_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();

            // Retrieve and trim input data from text boxes
            string firstText = textBox1.Text.Trim();
            string lastText = comboBox1.Text.Trim();
            string marksText = textBox3.Text.Trim();
            string idText = comboBox2.Text.Trim();

            try
            {
                // Validate input
                if (string.IsNullOrEmpty(firstText) || string.IsNullOrEmpty(lastText) ||
                    string.IsNullOrEmpty(marksText) || string.IsNullOrEmpty(idText))
                {
                    MessageBox.Show("Please fill in all the fields.");
                }
               
                
                else if((!IsAllDigits(marksText)) && (!IsAllDigits(idText)))
                    {
                    MessageBox.Show("Invalid format!Please enter the data in correct format.");
                }
                else if (!IsAllDigits(marksText))
                {
                    MessageBox.Show("Invalid format! Please enter only digits in rubric ID, total marks, and assessment identity.");
                }
                else
                {
                    // Check for existing records
                    SqlCommand countCmd = new SqlCommand("SELECT COUNT(*) FROM AssessmentComponent WHERE AssessmentId = @AssessmentId", con);
                    countCmd.Parameters.AddWithValue("@AssessmentId", idText);
                    int componentCount = (int)countCmd.ExecuteScalar();

                    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM AssessmentComponent WHERE Name = @Name AND (RubricId = @RubricId OR AssessmentId = @AssessmentId)", con);
                    checkCmd.Parameters.AddWithValue("@Name", firstText);
                    checkCmd.Parameters.AddWithValue("@RubricId", lastText);
                    checkCmd.Parameters.AddWithValue("@AssessmentId", idText);
                    int existingCount = (int)checkCmd.ExecuteScalar();

                    if (existingCount > 0)
                    {
                        MessageBox.Show("An assessment component with the same name already exists for the provided rubric ID or assessment ID.");
                    }
                    else
                    {
                        // Insert new record into the database
                        SqlCommand cmd = new SqlCommand("INSERT INTO AssessmentComponent VALUES (@Name, @RubricId, @TotalMarks, @DateCreated, @DateUpdated, @AssessmentId)", con);
                        cmd.Parameters.AddWithValue("@Name", firstText);
                        cmd.Parameters.AddWithValue("@RubricId", lastText);
                        cmd.Parameters.AddWithValue("@TotalMarks", marksText);
                        cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now);
                        cmd.Parameters.AddWithValue("@DateUpdated", DateTime.Now);
                        cmd.Parameters.AddWithValue("@AssessmentId", idText);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Successfully saved");
                        cleardata(); // Clear input fields after successful insertion
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            
        }

       
      


    }

    }
    

   
    
   
   
    
    

