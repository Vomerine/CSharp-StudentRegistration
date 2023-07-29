using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace StudentRegistration
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            load();
        }

        // config1 format is for sqlexpress while the other is for localdb
        // static string config1 = "Data Source=DESKTOP-9H7F9KV\\SQLEXPRESS; Initial Catalog=student_registration; User id=sa; Password=admin123";
        static string config = "Server= (localdb)\\ProjectModels; Database= student_registration; Integrated Security=True;";
        SqlConnection conn = new SqlConnection(config);
        SqlCommand cmd;
        SqlDataReader read;

        string id;
        bool Mode = true;
        string sql;

        public const int ID = 0;
        public const int NAME = 1;
        public const int COURSE = 2;
        public const int FEE = 3;

        public void load()
        {
            try
            {
                Mode = true;
                sql = "select * from students";
                conn.Open();
                cmd = new SqlCommand(sql, conn);
                read = cmd.ExecuteReader();
                dataGridView1.Rows.Clear();

                while (read.Read()) 
                {
                    dataGridView1.Rows.Add(read[ID], read[NAME], read[COURSE], read[FEE]);
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void getByID(string id)
        {
            sql = "select * from students where id = " + id;
            conn.Open();
            cmd = new SqlCommand(sql, conn);

            read = cmd.ExecuteReader();

            while (read.Read())
            {
                nameTextBox.Text = read[NAME].ToString();
                courseTextBox.Text = read[COURSE].ToString();
                feeTextBox.Text = read[FEE].ToString();
            }

            conn.Close();
        }

        public void deleteByID(string id)
        {
            sql = "DELETE FROM students WHERE id = " + id;

            conn.Open();
            cmd = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            conn.Close();

            load();

            MessageBox.Show("Record Deleted");
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            string message;
            string name = nameTextBox.Text;
            string course = courseTextBox.Text;
            string fee = feeTextBox.Text;

            if (Mode)
            {
                // '@' on strings to distinguish it as variables
                sql = "INSERT into students(name, course, fee) values(@name, @course, @fee)";

                conn.Open();
                cmd = new SqlCommand(sql, conn);
                
                // Insert the values to the query
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@course", course);
                cmd.Parameters.AddWithValue("@fee", fee);

                cmd.ExecuteNonQuery();

                nameTextBox.Clear();
                courseTextBox.Clear();
                feeTextBox.Clear();

                nameTextBox.Focus();
                message = "Record Added";
            }
            else
            {
                sql = "UPDATE students SET name = @name, course = @course, fee = @fee WHERE id = @id";
                conn.Open();
                cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@course", course);
                cmd.Parameters.AddWithValue("@fee", fee);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();

                nameTextBox.Clear();
                courseTextBox.Clear();
                feeTextBox.Clear();

                
                nameTextBox.Focus();
                message = "Record Updated";

            }
            
            conn.Close();

            load();
            MessageBox.Show(message);

        }

        private void deleteButton_Click(object sender, EventArgs e)
        {

        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            nameTextBox.Clear();
            courseTextBox.Clear();
            feeTextBox.Clear();

            nameTextBox.Focus();
            load();
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            load();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Edit"].Index && e.ColumnIndex >= 0)
            {
                Mode = false;
                id = dataGridView1.CurrentRow.Cells[ID].Value.ToString();
                getByID(id);
            }
            else if (e.ColumnIndex == dataGridView1.Columns["Delete"].Index && e.ColumnIndex >= 0)
            {
                Mode = false;
                id = dataGridView1.CurrentRow.Cells[ID].Value.ToString();
                deleteByID(id);
            }
        }

    }
}
