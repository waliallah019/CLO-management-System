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

namespace MidDB
{
    public partial class UserControl9 : UserControl
    {
        public UserControl9()
        {
            InitializeComponent();
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

        private bool IsValidCharacterInput(string input)
        {
            return Regex.IsMatch(input, @"^[a-zA-Z]+$");
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            string assessmentTitle = textBox1.Text.Trim();
          
            string marks = textBox3.Text.Trim();
            string weightage = textBox4.Text.Trim();

            if (string.IsNullOrWhiteSpace(assessmentTitle) || string.IsNullOrWhiteSpace(marks)|| string.IsNullOrWhiteSpace(weightage))
            {
                MessageBox.Show("Please enter Data about assessment.");
            }
            else if((!IsAllDigits(marks)) && (!IsAllDigits(weightage)) &&(IsValidCharacterInput(assessmentTitle)))
            {
                MessageBox.Show("Please enter Correct format of data.");
            }
            else if(!IsAllDigits(marks) ||(!IsAllDigits(weightage)) || (IsValidCharacterInput(assessmentTitle)))
            {
                MessageBox.Show("Invalid format!Digits in the marks and weightage only and character allowed only title .");
            }
            
            
           
            else
            {
                try
                {
                    var con = Configuration.getInstance().getConnection();

                    SqlCommand cmdAssessment = new SqlCommand("INSERT INTO Assessment (Title, DateCreated, TotalMarks, TotalWeightage) VALUES (@Title, @DateCreated, @TotalMarks, @TotalWeightage)", con);
                    cmdAssessment.Parameters.AddWithValue("@Title", assessmentTitle);
                    cmdAssessment.Parameters.AddWithValue("@DateCreated", DateTime.Now);
                    cmdAssessment.Parameters.AddWithValue("@TotalMarks", marks); 
                    cmdAssessment.Parameters.AddWithValue("@TotalWeightage", weightage); 
                    cmdAssessment.ExecuteNonQuery();

                    MessageBox.Show("Assessment Successfully added");
                    cleardata(); //Clear the assessment textboxes

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
         

        }
        private void cleardata()
        {

            textBox1.Text = "";
           
            textBox3.Text = "";
            textBox4.Text = "";
        }
    }
}
