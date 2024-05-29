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
    public partial class UserControl5 : UserControl
    {
        public UserControl5()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                string searchText = textBox1.Text.Trim();

                // Check if the search text is empty
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    MessageBox.Show("Please enter a search term.");
                    return;
                }

                SqlCommand cmd = new SqlCommand("SELECT * FROM Student WHERE FirstName LIKE @SearchText OR LastName LIKE @SearchText OR Contact LIKE @SearchText OR Email LIKE @SearchText OR RegistrationNumber LIKE @SearchText OR Status LIKE @SearchText", con);
                cmd.Parameters.AddWithValue("@SearchText",searchText ); 

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    // Bind the DataTable to the DataGridView
                    dataGridView2.DataSource = dt;
                    cleardata();
                }
                else
                {
                    MessageBox.Show("No matching records found.");
                    cleardata();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                cleardata();
            }



        }
        private void cleardata()
        {

            textBox1.Text = "";
        
        }
    }
}
