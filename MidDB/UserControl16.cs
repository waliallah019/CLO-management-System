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
    public partial class UserControl16 : UserControl
    {
        public UserControl16()
        {
            InitializeComponent();
            
        }
        private void DataBind()
        {


            DataGridViewComboBoxColumn status = new DataGridViewComboBoxColumn();
            status.HeaderText = "Status";
            status.Items.Add("Present");
            status.Items.Add("Absent");
            status.Items.Add("Late");
            status.Items.Add("Leave");

            dataGridView1.Columns.Add(status);

        }
        public void showdata()
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT Id, FirstName, LastName, Contact,Email,RegistrationNumber FROM Student WHERE Status = @Status", con);

                // Set the parameter for Status
                cmd.Parameters.AddWithValue("@Status", 5); // Assuming '5' represents 'Active' status

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                sizeset();
                DataBind();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        string id;
        private void button2_Click(object sender, EventArgs e)
        {


           
                var con = Configuration.getInstance().getConnection();

                DateTime attendanceDate = DateTime.Now;
                SqlCommand classAttendanceCmd = new SqlCommand("INSERT INTO ClassAttendance (AttendanceDate) VALUES (@AttendanceDate); SELECT SCOPE_IDENTITY();", con);
                classAttendanceCmd.Parameters.AddWithValue("@AttendanceDate", attendanceDate);
                int classId = Convert.ToInt32(classAttendanceCmd.ExecuteScalar());
                SqlCommand studentCOunt = new SqlCommand("Select COUNT(Student.Id) from Student  Where status = 5", con);
                int count = (int)studentCOunt.ExecuteScalar();
                for (int i = 0; i < count; i++)
                {
                    int std = 1;
                    string Stdindent = dataGridView1.Rows[i].Cells[0].Value.ToString();

                    if (Stdindent != null)
                    {
                        string StdRole = dataGridView1.Rows[i].Cells[4].Value.ToString();


                        if (StdRole == "Present")
                        {
                            std = 1;
                        }
                        else if (StdRole == "Absent")
                        {
                            std = 2;
                        }
                        else if (StdRole == "Leave")
                        {
                            std = 3;
                        }
                        else if (StdRole == "Late")
                        {
                            std = 4;
                        }
                    }
                    SqlCommand studentAttendanceCmd = new SqlCommand("INSERT INTO StudentAttendance (AttendanceId, StudentId, AttendanceStatus) VALUES (@AttendanceId, @StudentId, @AttendanceStatus)", con);
                    studentAttendanceCmd.Parameters.AddWithValue("@AttendanceId", classId);
                    studentAttendanceCmd.Parameters.AddWithValue("@StudentId", Stdindent);
                    studentAttendanceCmd.Parameters.AddWithValue("@AttendanceStatus", std);

                    studentAttendanceCmd.ExecuteNonQuery();
                }
               MessageBox.Show("Attendance added successfully.");
            }

        private void sizeset()
        {
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }

}