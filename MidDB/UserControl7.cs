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
    public partial class UserControl7 : UserControl
    {
        public UserControl7()
        {
            InitializeComponent();
            fetching();
        }
        //Validation for Characters
        private bool IsValidCharacterInput(string input)
        {
            return Regex.IsMatch(input, @"^[a-zA-Z]+$");
        }
        //Validation for Digits
        private bool IsAllDigits(string str)
        {
            foreach (char c in str)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }
        void fetching()
        {
            try
            {
                var con = Configuration.getInstance().getConnection();

                
                comboBox1.Items.Clear();

                SqlCommand cmdFetchCLOIds = new SqlCommand("SELECT Id FROM CLO WHERE Name NOT LIKE '&%'", con);
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
        private void button2_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            string rubricDetails = textBox2.Text.Trim();
            string cloIdText = comboBox1.Text.Trim();
            string idText = textBox1.Text.Trim();

            // Check if any of the required fields are empty
            if (string.IsNullOrEmpty(rubricDetails) || string.IsNullOrEmpty(cloIdText) || string.IsNullOrEmpty(idText))
            {
                MessageBox.Show("Please fill in all the fields.");
            }
            else if ((!IsValidCharacterInput(rubricDetails)) && (!IsAllDigits(idText)) && (!IsAllDigits(cloIdText)))
            {
                MessageBox.Show("Please enter Correct Format of Data.Only Details Textbox require string ");
            }
            else if(!IsValidCharacterInput(rubricDetails))
            {
                MessageBox.Show("Please enter characters only.");
            }
            else if(!IsAllDigits(idText) ||(!IsAllDigits(cloIdText)))
            {
                MessageBox.Show("Please enter only digits.Both cloid and id require digits only");
            }

            else
            {
                // Attempt to convert CloId to int
                if (int.TryParse(cloIdText, out int cloId))
                {
                    // Assuming you have a SqlConnection named 'con' already initialized and open
                    SqlCommand cmd = new SqlCommand("INSERT INTO Rubric (id,Details, CloId) VALUES (@id,@Details, @CloId)", con);
                    cmd.Parameters.AddWithValue("@Id", idText);
                    cmd.Parameters.AddWithValue("@Details", rubricDetails);
                    cmd.Parameters.AddWithValue("@CloId", cloId);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Rubric inserted successfully.");
                        cleardata(); // Custom method to clear textboxes or other UI elements
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error inserting rubric: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("CloId must be a valid integer.");
                }
            }
}
        private void cleardata()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            comboBox1.Text = "";
          

        }


    }
}
