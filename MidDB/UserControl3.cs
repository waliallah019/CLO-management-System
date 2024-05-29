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
    public partial class UserControl3 : UserControl
    {
        public UserControl3()
        {
            InitializeComponent();
        }

        private void UserControl3_Load(object sender, EventArgs e)
        {

        }
        private bool IsValidCharacterInput(string input)
        {
            return Regex.IsMatch(input, @"^[a-zA-Z]+$");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string cloName = textBox1.Text.Trim();

            if (string.IsNullOrWhiteSpace(cloName))
            {
                MessageBox.Show("Please enter CLO name.");
            }
            else if (!IsValidCharacterInput(cloName))
                {
                    MessageBox.Show("Please enter any character.");
                }
            else
            {
                try
                {
                    var con = Configuration.getInstance().getConnection();

                    SqlCommand cmdCLO = new SqlCommand("INSERT INTO Clo (Name, DateCreated, DateUpdated) VALUES (@Name, @DateCreated, @DateUpdated)", con);
                    cmdCLO.Parameters.AddWithValue("@Name", cloName);
                    cmdCLO.Parameters.AddWithValue("@DateCreated", DateTime.Now);
                    cmdCLO.Parameters.AddWithValue("@DateUpdated", DateTime.Now);
                    cmdCLO.ExecuteNonQuery();

                    MessageBox.Show("CLO Successfully added");
                    cleardata(); // Assuming this method clears CLO-related input fields
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
        }
    }
}
