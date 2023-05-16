using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task
{
    public partial class Form1 : Form
    {
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=CrudOperations;Integrated Security=True");
        SqlCommand cmd;
        string query = "select * from Student";
        int ID = 0;
        SqlDataAdapter adapter;
        public Form1()
        {
            InitializeComponent();
        }
        DataTable dt = new DataTable("Student");
        private void DisplayData()
        {
            con.Open();
            SqlDataAdapter sqlDa = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            sqlDa.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }


        private void ClearData()
        {
            txtName.Text = "";
            txtAge.Text = "";
            txtUName.Text = "";
            txtProgram.Text = "";
            ID = 0;
        }
        private void dataGridView1_RowHeaderCellChanged(object sender, DataGridViewRowEventArgs e)
        {
           
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (txtName.Text != "" && txtAge.Text != "" && txtUName.Text != "" && txtProgram.Text != "")
            {
                string query = $"insert into Student values ('{txtName.Text}','{txtAge.Text}','{txtUName.Text}','{txtProgram.Text}')";
                con.Open();
                bool x = false;
                cmd = new SqlCommand(query, con);
                int a = cmd.ExecuteNonQuery();
                con.Close();
                if (a>0)
                {
                    MessageBox.Show("User Added Successfully");
                    DisplayData();
                    ClearData();
                }
                else
                {
                    MessageBox.Show("User Has Not been added , there must be an error");
                }
            }
            else
            {
                MessageBox.Show("Input all the values before performing any actions");
            }

        }

        private void btnupdate_Click(object sender, EventArgs e)
        {

            if (txtName.Text != "" && txtAge.Text != "" && txtUName.Text != "" && txtProgram.Text != "")
            {
                string query = $"update Student set Name='{txtName.Text}',Age='{txtAge.Text}',University='{txtUName.Text}',Program='{txtProgram.Text}' where ID={ID}";
                con.Open();
                cmd = new SqlCommand(query, con);
                int a = cmd.ExecuteNonQuery();
                con.Close();
                if (a>0)
                {
                    MessageBox.Show("Record Updated Successfully!");
                    DisplayData();
                    ClearData();
                }
                else
                {
                    MessageBox.Show("There's an error");
                }
                DisplayData();
                ClearData();
            }
            else
            {
                MessageBox.Show("Please Select Record to Update");
            }
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            if (ID != 0)
            {
                string query = $"delete Student where ID={ID}";
                con.Open();
                cmd = new SqlCommand(query, con);
                int a = cmd.ExecuteNonQuery();
                con.Close();
                if (a > 0)
                {
                    MessageBox.Show("Record Deleted Successfully!");
                    DisplayData();
                    ClearData();
                }
                else
                {
                    MessageBox.Show("There's an error");
                }
            }
            else
            {
                MessageBox.Show("Please Select Record to Delete");
            }
        }

        private void btnreset_Click(object sender, EventArgs e)
        {
            ClearData();
            DisplayData();
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            txtName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtAge.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtUName.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtProgram.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString(); txtName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'crudOperationsDataSet1.Student' table. You can move, or remove it, as needed.
            this.studentTableAdapter.Fill(this.crudOperationsDataSet1.Student);
            DisplayData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text != "")
            {
                string searchValue = txtSearch?.Text ?? string.Empty;
                query = $"SELECT * FROM Student WHERE Name='{searchValue}'";
                con.Open();
                cmd = new SqlCommand(query, con);


                int a = (int)cmd.ExecuteScalar();
                con.Close();
                if (a > 0)
                {
                    MessageBox.Show("User Found");
                    DisplayData();
                    ClearData();
                }
                else
                {
                    MessageBox.Show("Can't find User by this username");
                }
            }
            else
            {
                MessageBox.Show("Input all the values before performing any actions");
            }
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
          
        }
        public void searchData(string valueToSearch)
        {
            string query = "SELECT * FROM Student WHERE CONCAT([Name], [Age], [University], [Program]) LIKE @valueToSearch";
    
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@valueToSearch", "%" + valueToSearch + "%");
            adapter = new SqlDataAdapter(cmd);
            dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                string valueToSearch = txtSearch.Text;
                searchData(valueToSearch);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            searchData("");
        }
    }
}
