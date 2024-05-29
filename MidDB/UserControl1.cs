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
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            string firstText = textBox2.Text.Trim();
            string lastText = textBox3.Text.Trim();
            string phoneText = textBox4.Text.Trim();
            string mailText = textBox5.Text.Trim();
            string regText = textBox6.Text.Trim();
            string stText = comboBox2.Text.Trim();

            if (string.IsNullOrEmpty(firstText) || string.IsNullOrEmpty(lastText) || string.IsNullOrEmpty(phoneText) || string.IsNullOrEmpty(mailText) || string.IsNullOrEmpty(regText) || string.IsNullOrEmpty(stText))
            {
                MessageBox.Show("Please fill in all the fields.");
            }
            else if((!IsAllDigits(phoneText)) && (!IsValidEmail(mailText)) && (!IsValidCharacterInput(firstText)) && (!IsValidCharacterInput(lastText)) &&(!IsValidPhoneNumber(phoneText)))
                {
                MessageBox.Show("Please enter correct format of data.");
            }
            else if (!IsAllDigits(phoneText))
            {
                MessageBox.Show("Please enter a valid phone number (digits only).");
            }
            else if (!IsValidEmail(mailText))
            {
                MessageBox.Show("Please enter a valid email address.");
            }
            else if(!IsValidCharacterInput(firstText))
            {
                MessageBox.Show("Please enter any character.");
            }
            else if (!IsValidCharacterInput(lastText))
            {
                MessageBox.Show("Please enter any character.");
            }
            else if(!IsValidPhoneNumber(phoneText))
            {
                MessageBox.Show("Please enter a valid phone number.");

            }

            else
            {
                // Check if the data already exists in the database
                SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Student WHERE Email = @Email OR RegistrationNumber = @RegistrationNumber", con);
                checkCmd.Parameters.AddWithValue("@Email", mailText);
                checkCmd.Parameters.AddWithValue("@RegistrationNumber", regText);
                int existingCount = (int)checkCmd.ExecuteScalar();

                if (existingCount > 0)
                {
                    MessageBox.Show("The data already exists in the database.");
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO Student (FirstName, LastName, Contact, Email, RegistrationNumber, Status) VALUES (@FirstName, @LastName,  @Contact, @Email, @RegistrationNumber, @Status)", con);
                    cmd.Parameters.AddWithValue("@FirstName", firstText);
                    cmd.Parameters.AddWithValue("@LastName", lastText);
                    cmd.Parameters.AddWithValue("@Contact", phoneText);
                    cmd.Parameters.AddWithValue("@Email", mailText);
                    cmd.Parameters.AddWithValue("@RegistrationNumber", regText);

                    int status = 0;
                    if (comboBox2.Text == "Inactive")
                        status = 6;
                    else if (comboBox2.Text == "Active")
                        status = 5;

                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Successfully saved");
                    cleardata();
                }
            }

        }
        //Check for Contact info
        private bool IsValidPhoneNumber(string phoneNumber)
        {
            
            foreach (char c in phoneNumber)
            {
                if (!char.IsDigit(c))
                    return false;
            }

            if (phoneNumber.Length != 11)
                return false;

            return true;
        }


        // Function to check if a string contains only digits
        private bool IsAllDigits(string str)
            {
                foreach (char c in str)
                {
                    if (!char.IsDigit(c))
                        return false;
                }
                return true;
            }

            // Function to validate email address
            private bool IsValidEmail(string email)
            {
                try
                {
                    var addr = new System.Net.Mail.MailAddress(email);
                    return addr.Address == email;
                }
                catch
                {
                    return false;
                }
            }


        private bool IsValidCharacterInput(string input)
        {
            return Regex.IsMatch(input, @"^[a-zA-Z]+$");
        }


        private void cleardata()
            {

                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                comboBox2.Text = "";
            }
        }
    }
