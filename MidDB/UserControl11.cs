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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MidDB
{
    public partial class UserControl11 : UserControl
    {
        public UserControl11()
        {
            InitializeComponent();
            Fetching();
        }


        private void cleardata()
        {


            comboBox1.Text = "";
            textBox3.Text = "";
            textBox2.Text = "";
        }
        //validation for numbers
        private bool IsAllDigits(string str)
        {
            foreach (char c in str)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }
        //validation for characters
        private bool IsValidCharacterInput(string input)
        {
            return Regex.IsMatch(input, @"^[a-zA-Z]+$");
        }
        void Fetching()
        {
            try
            {
                var con = Configuration.getInstance().getConnection();

                // Clear existing items in the ComboBox
                comboBox1.Items.Clear();

                // Fetch CLO IDs from the CLO table
                SqlCommand cmdFetchCLOIds = new SqlCommand("SELECT Id FROM Rubric WHERE Details NOT LIKE '&%'", con);
                SqlDataReader reader = cmdFetchCLOIds.ExecuteReader();

                // Loop through the result set and add CLO IDs to the ComboBox
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
        private void button2_Click(object sender, EventArgs e)
        {
            string idtext = comboBox1.Text.Trim();
            string detailText = textBox2.Text.Trim();
            string MeasurementText = textBox3.Text.Trim();

            if (string.IsNullOrWhiteSpace(detailText) || string.IsNullOrWhiteSpace(MeasurementText))
            {
                MessageBox.Show("Please enter data about Rubric level.");
            }
            else if ((!IsAllDigits(MeasurementText)) && (!IsValidCharacterInput(detailText)))
            {
                MessageBox.Show("Please enter correct format of data.");

            }
            else if (!IsAllDigits(MeasurementText))
            {
                MessageBox.Show("Please enter digit in measurement box .");
            }
            else if (!IsValidCharacterInput(detailText))
            {
                MessageBox.Show("Please enter charactes in the detail portion box.");
            }
            else
            {
                // Check if MeasurementText contains only valid levels
                string[] measurementLevels = MeasurementText.Split(',');

                // Check if there are no more than 4 levels
                if (measurementLevels.Length > 4)
                {
                    MessageBox.Show("You cannot add more than 4 levels.");
                }
                else
                {
                    // Validate each level
                    bool isValid = true;
                    foreach (string level in measurementLevels)
                    {
                        int levelValue;
                        if (!int.TryParse(level, out levelValue) || levelValue <= 0 || levelValue > 4)
                        {
                            isValid = false;
                            break;
                        }
                    }

                    if (!isValid)
                    {
                        MessageBox.Show("Invalid measurement levels. Please enter values between 1 and 4.");
                    }
                    else
                    {
                        try
                        {
                            var con = Configuration.getInstance().getConnection();

                            // Check if the RubricId and MeasurementLevel combination already exists
                            string query = "SELECT COUNT(*) FROM RubricLevel WHERE RubricId = @RubricId AND MeasurementLevel = @MeasurementLevel";
                            SqlCommand checkCommand = new SqlCommand(query, con);
                            checkCommand.Parameters.AddWithValue("@RubricId", idtext);

                            foreach (string level in measurementLevels)
                            {
                                checkCommand.Parameters.Clear();
                                checkCommand.Parameters.AddWithValue("@RubricId", idtext);
                                checkCommand.Parameters.AddWithValue("@MeasurementLevel", level);

                                int existingCount = (int)checkCommand.ExecuteScalar();
                                if (existingCount > 0)
                                {
                                    MessageBox.Show("Rubric level with the same Rubric ID and Measurement level already exists.");
                                    return;
                                }
                            }

                            // If no duplicates found, proceed with the insertion
                            SqlCommand cmdAssessment = new SqlCommand("INSERT INTO RubricLevel (RubricId , Details, MeasurementLevel) VALUES (@RubricId,@Details, @MeasurementLevel)", con);
                            cmdAssessment.Parameters.AddWithValue("@RubricId", idtext);
                            cmdAssessment.Parameters.AddWithValue("@Details", detailText);
                            cmdAssessment.Parameters.AddWithValue("@MeasurementLevel", MeasurementText);

                            cmdAssessment.ExecuteNonQuery();

                            MessageBox.Show("Rubric level Successfully added");
                            cleardata();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message);
                        }
                    }
                }
            }

        }
    }
            }

   

