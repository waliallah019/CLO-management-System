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
    public partial class UserControl4 : UserControl
    {
        public UserControl4()
        {
            InitializeComponent();
            dataGridView2.Refresh();
            showData();
        }

        private void button2_Click(object sender, EventArgs e)
        {


            try
            {
                var con = Configuration.getInstance().getConnection();


                // Fetch Id from the database
                SqlCommand fetchIdCmd = new SqlCommand("SELECT Id FROM Student WHERE Id = @Id", con);
                fetchIdCmd.Parameters.AddWithValue("@Id", id);

                int id1 = Convert.ToInt32(fetchIdCmd.ExecuteScalar());
                if (id1 == 0)
                {
                    MessageBox.Show("Error: Student with this ID does not exist.");
                    cleardata();
                    return;
                }

                // Update the student details
                SqlCommand updateCmd = new SqlCommand("UPDATE Student SET FirstName = @FirstName, LastName = @LastName, Contact = @Contact, Email = @Email ,RegistrationNumber=@RegistrationNumber,Status = @Status WHERE Id = @Id", con);
                updateCmd.Parameters.AddWithValue("@FirstName", textBox2.Text);
                updateCmd.Parameters.AddWithValue("@LastName", textBox3.Text);
                updateCmd.Parameters.AddWithValue("@Contact", textBox4.Text);
                updateCmd.Parameters.AddWithValue("@Email", textBox5.Text);
                updateCmd.Parameters.AddWithValue("@RegistrationNumber", textBox6.Text);
                updateCmd.Parameters.AddWithValue("@Status", textBox1.Text);
                int status;
                if (!int.TryParse(textBox1.Text, out status) || (status != 5 && status != 6))
                {
                    MessageBox.Show("Error: Status must be either 5 or 6.");
                    return;
                }
                updateCmd.Parameters.AddWithValue("@Id", id);
                SqlCommand checkRegNoCmd = new SqlCommand("SELECT COUNT(*) FROM Student WHERE RegistrationNumber = @RegistrationNumber AND Id != @Id", con);
                checkRegNoCmd.Parameters.AddWithValue("@RegistrationNumber", textBox4.Text);
                checkRegNoCmd.Parameters.AddWithValue("@Id", id);

                int regNoCount = (int)checkRegNoCmd.ExecuteScalar();
                if (regNoCount > 0)
                {
                    MessageBox.Show("Error: Registration number already exists. Please choose a different one.");
                    return;
                }
                updateCmd.ExecuteNonQuery();
                MessageBox.Show("Successfully updated");
                showData();

                cleardata();
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }


        }

        private void sizeset()
        {
            for (int i = 0; i < dataGridView2.Columns.Count; i++)
            {
                dataGridView2.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }
        private void showData()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from Student where Status = 5", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView2.DataSource = dt;
            sizeset();
            dataGridView2.Refresh();
        }
        private void cleardata()
        {

            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox1.Text = "";
        }
        int indexrow;
        string id;
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            indexrow = e.RowIndex;
            DataGridViewRow row = dataGridView2.Rows[indexrow];


            id = row.Cells[0].Value.ToString();
           
            textBox2.Text = row.Cells[1].Value.ToString();
            textBox3.Text = row.Cells[2].Value.ToString();
            textBox4.Text = row.Cells[3].Value.ToString();
            textBox5.Text = row.Cells[4].Value.ToString();
            textBox6.Text = row.Cells[5].Value.ToString();
            textBox1.Text = row.Cells[6].Value.ToString();
        }
    }
}
