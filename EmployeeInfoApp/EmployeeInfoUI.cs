using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace EmployeeInfoApp
{
    public partial class EmployeeInfoUI : Form
    {

        private bool isUpdateMode = false;
        private int employeeId;
        public String connectionString =ConfigurationManager.ConnectionStrings["EmployeeInfoConnString"].ConnectionString;
        Employee employeeObj=new Employee();
        public EmployeeInfoUI()
        {
            InitializeComponent();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            employeeObj.name = nameTextBox.Text;
            employeeObj.address = addressTextBox.Text;
            employeeObj.email = emailTExtBox.Text;
            employeeObj.salary = Convert.ToDouble(salaryTextBox.Text);

            if (isUpdateMode)
            {
                SqlConnection connection = new SqlConnection(connectionString);


                //2. write query 

                string query = "UPDATE Employee SET name ='" +employeeObj.name  + "', address ='" + employeeObj.address +"',email='"+employeeObj.email+ "',salary='"+employeeObj.salary+"' WHERE ID = '" +
                               employeeId + "'";


                // 3. execute query 

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                int rowAffected = command.ExecuteNonQuery();
                connection.Close();



                //4. see result

                if (rowAffected > 0)
                {
                    MessageBox.Show("Updated Successfully!");

                    saveButton.Text = "Save";
                    employeeId = 0;
                    isUpdateMode = false;

                    ShowAllEmplyeeInfo();

                }
                else
                {
                    MessageBox.Show("Update Failed!");
                }
            }

            else
            {




                if (IsEmailExists(employeeObj.email))
                {
                    MessageBox.Show("Email name  already exists!");
                    return;
                }


                SqlConnection connection = new SqlConnection(connectionString);

                String query = "INSERT INTO Employee Values( '" + employeeObj.name + "','" + employeeObj.address + "','" +
                               employeeObj.email + "','" + employeeObj.salary + "');";

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();

                int rowAffected = command.ExecuteNonQuery();
                connection.Close();

                if (rowAffected > 0)
                {
                    MessageBox.Show("Insertion successful.");
                    ShowAllEmplyeeInfo();
                }

                else
                {
                    MessageBox.Show("Operation Fail!!!");
                }
            }
        }

        public bool IsEmailExists(string email)
        {
            
            SqlConnection connection = new SqlConnection(connectionString);

            //2. write query 

            string query = "SELECT * FROM Employee WHERE email ='" + email + "'";


            // 3. execute query 

            SqlCommand command = new SqlCommand(query, connection);

            bool isEmailExists = false;

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                isEmailExists = true;
                break;
            }
            reader.Close();
            connection.Close();

            return isEmailExists;

        }


        public void ShowAllEmplyeeInfo()
        {
            SqlConnection connection=new SqlConnection(connectionString);
            string query = "SELECT * FROM Employee";
            SqlCommand command=new SqlCommand(query,connection);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            List<Employee>employeeList=new List<Employee>();
            while (reader.Read())
            {
                Employee employee=new Employee();
                employee.ID = int.Parse(reader["id"].ToString());
                employee.name = reader["name"].ToString();
                employee.address = reader["address"].ToString();
                employee.email = reader["email"].ToString();
                employee.salary = reader["salary"].GetHashCode();

                employeeList.Add(employee);
            }

            reader.Close();
            connection.Close();

            LoadEmployeeListView(employeeList);
        }

        public void LoadEmployeeListView(List<Employee> employees )
        {
            EmployeeListView.Items.Clear();
            foreach (var employee in employees)
            {
                ListViewItem item=new ListViewItem(employee.ID.ToString());
                item.SubItems.Add(employee.name);
                item.SubItems.Add(employee.address);
                item.SubItems.Add(employee.email);
                item.SubItems.Add(employee.email);


                EmployeeListView.Items.Add(item);
            }
        }

        private void EmployeeInfoUI_Load(object sender, EventArgs e)
        {
            ShowAllEmplyeeInfo();
        }

        private void EmployeeListView_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem item = EmployeeListView.SelectedItems[0];
            int Id = int.Parse(item.Text.ToString());

            Employee employee = GetEmployeeById(Id);

            if (employee!=null)
            {
                isUpdateMode = true;

                saveButton.Text = "Update";

                employeeId = employee.ID;


                nameTextBox.Text = employee.name;
                addressTextBox.Text = employee.address;
                emailTExtBox.Text = employee.email;
                salaryTextBox.Text = employee.salary.ToString();

            }


        }

        public Employee GetEmployeeById(int id)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            //2. write query 

            string query = "SELECT * FROM Employee WHERE id ='" + id + "'";


            // 3. execute query 

            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            List<Employee> employeeList = new List<Employee>();

            while (reader.Read())
            {

                Employee employee = new Employee();
                employee.ID = int.Parse(reader["id"].ToString());
                employee.name = reader["name"].ToString();
                employee.address = reader["address"].ToString();
                employee.email = reader["email"].ToString();
                employee.salary = double.Parse(reader["salary"].ToString());

                employeeList.Add(employee);

            }
            reader.Close();
            connection.Close();

            return employeeList.FirstOrDefault();


        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            ListViewItem item = EmployeeListView.SelectedItems[0];
            int Id = int.Parse(item.Text.ToString());

            Employee employee = GetEmployeeById(Id);

            if (employee!=null)
            {
                SqlConnection connection = new SqlConnection(connectionString);

                string query = "DELETE  FROM Employee WHERE id ='" + Id + "'";

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();

                int rowAffected = command.ExecuteNonQuery();
                connection.Close();

                if (rowAffected > 0)
                {
                    MessageBox.Show("Delete successful.");
                    ShowAllEmplyeeInfo();
                }

                else
                {
                    MessageBox.Show("Operation Fail!!!");
                }
            }
        }

        private void nameTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            
        }

        
    }
}
