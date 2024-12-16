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
using static Management_Shoes_Football.Form1;

namespace Management_Shoes_Football
{
    public partial class frmEmployee : Form
    {
        public frmEmployee()
        {
            InitializeComponent();
        }
        private void LoadEmployees()
        {
            string query = "SELECT EmployeeID, EmployeeName, Email, PhoneNumber, Position FROM Employee";

            try
            {
                using (SqlConnection connection = new DatabaseConnection().GetConnection())
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Xóa tất cả các mục trong ListView trước khi thêm mới
                        lvEmployeeManagement.Items.Clear();

                        while (reader.Read())
                        {
                            // Tạo một ListViewItem mới cho mỗi nhân viên
                            ListViewItem item = new ListViewItem(reader["EmployeeID"].ToString());
                            item.SubItems.Add(reader["EmployeeName"].ToString());
                            item.SubItems.Add(reader["Email"].ToString());
                            item.SubItems.Add(reader["PhoneNumber"].ToString());
                            item.SubItems.Add(reader["Position"].ToString());

                            // Thêm mục vào ListView
                            lvEmployeeManagement.Items.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void AddEmployee()
        {
            string query = "INSERT INTO Employee (EmployeeName, Email, PhoneNumber, Position) " +
                  "VALUES (@EmployeeName, @Email, @PhoneNumber, @Position)";

            try
            {
                using (SqlConnection connection = new DatabaseConnection().GetConnection())
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeName", txtEmployeeName.Text);
                    command.Parameters.AddWithValue("@Email", txtEmail.Text);
                    command.Parameters.AddWithValue("@PhoneNumber", txtPhone.Text);
                    command.Parameters.AddWithValue("@Position", txtPosition.Text);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Employee added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadEmployees(); // Hàm tải lại dữ liệu
                    }
                    else
                    {
                        MessageBox.Show("Failed to add employee.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateEmployee()
        {
            string query = "UPDATE Employee SET EmployeeName = @EmployeeName, Email = @Email, " +
                   "PhoneNumber = @PhoneNumber, Position = @Position WHERE EmployeeID = @EmployeeID";

            try
            {
                using (SqlConnection connection = new DatabaseConnection().GetConnection())
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeName", txtEmployeeName.Text);
                    command.Parameters.AddWithValue("@Email", txtEmail.Text);
                    command.Parameters.AddWithValue("@PhoneNumber", txtPhone.Text);
                    command.Parameters.AddWithValue("@Position", txtPosition.Text);
                    command.Parameters.AddWithValue("@EmployeeID", int.Parse(txtEmployeeID.Text));

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Employee updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadEmployees(); // Hàm tải lại dữ liệu
                    }
                    else
                    {
                        MessageBox.Show("Failed to update employee.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void frmEmployee_Load(object sender, EventArgs e)
        {
            LoadEmployees();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddEmployee();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateEmployee();
        }

        private void DeleteEmployee()
        {
            if (MessageBox.Show("Are you sure you want to delete this employee?", "Confirm Delete",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string query = "DELETE FROM Employee WHERE EmployeeID = @EmployeeID";

                try
                {
                    using (SqlConnection connection = new DatabaseConnection().GetConnection())
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeID", int.Parse(txtEmployeeID.Text));

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Employee deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadEmployees(); // Hàm tải lại dữ liệu
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete employee.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void lvEmployeeManagement_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvEmployeeManagement.SelectedItems.Count > 0)
            {
                // Lấy dòng được chọn trong ListView
                ListViewItem selectedItem = lvEmployeeManagement.SelectedItems[0];

                // Điền dữ liệu vào các TextBox
                txtEmployeeID.Text = selectedItem.SubItems[0].Text;  // EmployeeID
                txtEmployeeName.Text = selectedItem.SubItems[1].Text;  // EmployeeName
                txtEmail.Text = selectedItem.SubItems[2].Text;  // Email
                txtPhone.Text = selectedItem.SubItems[3].Text;  // PhoneNumber
                txtPosition.Text = selectedItem.SubItems[4].Text;  // Position
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteEmployee();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            frmHome fromhome = new frmHome();
            fromhome.Show();
            this.Hide();
        }

        private void SearchEmployees(string searchKeyword)
        {
            string query = "SELECT EmployeeID, EmployeeName, Email, PhoneNumber, Position " +
                           "FROM Employee " +
                           "WHERE EmployeeName LIKE @SearchKeyword OR PhoneNumber LIKE @SearchKeyword";

            try
            {
                using (SqlConnection connection = new DatabaseConnection().GetConnection())
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Gán giá trị tham số từ khóa tìm kiếm
                    command.Parameters.AddWithValue("@SearchKeyword", "%" + searchKeyword + "%");

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Xóa dữ liệu cũ trong ListView
                        lvEmployeeManagement.Items.Clear();

                        while (reader.Read())
                        {
                            // Tạo một ListViewItem mới cho mỗi kết quả
                            ListViewItem item = new ListViewItem(reader["EmployeeID"].ToString());
                            item.SubItems.Add(reader["EmployeeName"].ToString());
                            item.SubItems.Add(reader["Email"].ToString());
                            item.SubItems.Add(reader["PhoneNumber"].ToString());
                            item.SubItems.Add(reader["Position"].ToString());

                            // Thêm kết quả vào ListView
                            lvEmployeeManagement.Items.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchKeyword = txtSearch.Text.Trim();
            SearchEmployees(searchKeyword);
        }

        private void Reset()
        {
            txtSearch.Clear();
            LoadEmployees();
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }
    }
}
