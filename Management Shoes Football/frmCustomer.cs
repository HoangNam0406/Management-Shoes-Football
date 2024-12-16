using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Management_Shoes_Football.Form1;

namespace Management_Shoes_Football
{
    public partial class frmCustomer : Form
    {
        public frmCustomer()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void LoadCustomerData()
        {
            DatabaseConnection conn = new DatabaseConnection();
            using (SqlConnection connection = conn.GetConnection())

                try
                {
                    connection.Open();
                    string query = "SELECT CustomerID, CustomerName, PhoneNumber, Address, Email FROM Customer";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Xóa dữ liệu cũ trong ListView nếu có
                        lvCustomer.Items.Clear();

                        while (reader.Read())
                        {
                            // Thêm dòng dữ liệu vào ListView
                            ListViewItem item = new ListViewItem(reader["CustomerID"].ToString());
                            item.SubItems.Add(reader["CustomerName"].ToString());
                            item.SubItems.Add(reader["PhoneNumber"].ToString());
                            item.SubItems.Add(reader["Address"].ToString());
                            item.SubItems.Add(reader["Email"].ToString());

                            lvCustomer.Items.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }

        public void AddCustomer()
        {
            string customerID = txtCustommerID.Text.Trim();
            string customerName =txtCustomerName.Text.Trim();
            string Phone = txtPhone.Text.Trim();
            string Address = txtAddress.Text.Trim();
            string Email = txtEmail.Text.Trim();


            string query = "INSERT INTO Customer (CustomerName, PhoneNumber, Address, Email) VALUES (@CustomerName, @PhoneNumber, @Address, @Email)";

            // Gọi lớp DatabaseConnection
            DatabaseConnection dbConnection = new DatabaseConnection();

            try
            {
                using (SqlConnection connection = dbConnection.GetConnection())
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Thêm tham số
                        command.Parameters.AddWithValue("@CustomerID", customerID);
                        command.Parameters.AddWithValue("@CustomerName", customerName);
                        command.Parameters.AddWithValue("@PhoneNumber", Phone);
                        command.Parameters.AddWithValue("@Address", Address);
                        command.Parameters.AddWithValue("@Email", Email);

                        // Thực thi truy vấn
                        int rows = command.ExecuteNonQuery();
                        MessageBox.Show($"{rows} customer(s) added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCustomerData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Hàm cập nhật khách hàng
        public void UpdateCustomer()
        {
            if (lvCustomer.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng để sửa");
            }
            string customerID = txtCustommerID.Text.Trim();
            string customerName = txtCustomerName.Text.Trim();
            string Phone = txtPhone.Text.Trim();
            string Address = txtAddress.Text.Trim();
            string Email = txtEmail.Text.Trim();

            string query = "UPDATE Customer SET CustomerName = @CustomerName, PhoneNumber = @PhoneNumber, " +
               "Address = @Address, Email = @Email WHERE CustomerID = @CustomerID";
            // Gọi lớp DatabaseConnection
            DatabaseConnection dbConnection = new DatabaseConnection();

            try
            {
                using (SqlConnection connection = dbConnection.GetConnection())
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Thêm tham số
                        command.Parameters.AddWithValue("@CustomerID", customerID);
                        command.Parameters.AddWithValue("@CustomerName", customerName);
                        command.Parameters.AddWithValue("@PhoneNumber", Phone);
                        command.Parameters.AddWithValue("@Address", Address);
                        command.Parameters.AddWithValue("@Email", Email);

                        // Thực thi truy vấn
                        command.ExecuteNonQuery();

                        ListViewItem item = lvCustomer.SelectedItems[0];
                        item.SubItems[1].Text = customerName;
                        item.SubItems[2].Text = Phone;
                        item.SubItems[3].Text = Address;
                        item.SubItems[4].Text = Email;
               
                        MessageBox.Show(" customer updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCustomerData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Hàm xóa khách hàng
        public void DeleteCustomer()
        {
            string query = "DELETE FROM Customer WHERE CustomerID = @CustomerID";


            string customerID = lvCustomer.SelectedItems[0].SubItems[0].Text;
            // Gọi lớp DatabaseConnection
            DatabaseConnection dbConnection = new DatabaseConnection();

            try
            {
                using (SqlConnection connection = dbConnection.GetConnection())
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Thêm tham số
                        command.Parameters.AddWithValue("@CustomerID", customerID);

                        // Thực thi truy vấn
                        command.ExecuteNonQuery();
                        MessageBox.Show("customer deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCustomerData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void frmCustomer_Load(object sender, EventArgs e)
        {
            LoadCustomerData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddCustomer();
        }

        private void lvCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvCustomer.SelectedItems.Count > 0)
            {
                ListViewItem item = lvCustomer.SelectedItems[0];
                txtCustommerID.Text = item.SubItems[0].Text;
                txtCustomerName.Text = item.SubItems[1].Text;
                txtPhone.Text = item.SubItems[2].Text;
                txtAddress.Text = item.SubItems[3].Text;
                txtEmail.Text = item.SubItems[4].Text;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteCustomer();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateCustomer();
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            frmHome fromhome = new frmHome();
            fromhome.Show();
            this.Hide();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Please enter a search term!", "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string query = "SELECT CustomerID, CustomerName, PhoneNumber, Address, Email FROM Customer WHERE CustomerName LIKE @SearchTerm";
            try
            {
                using (SqlConnection connection = new DatabaseConnection().GetConnection())
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SearchTerm", $"%{txtSearch.Text.Trim()}%");
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        lvCustomer.Items.Clear();
                        while (reader.Read())
                        {
                            ListViewItem item = new ListViewItem(reader["CustomerID"].ToString());
                            item.SubItems.Add(reader["CustomerName"].ToString());
                            item.SubItems.Add(reader["Email"].ToString());
                            item.SubItems.Add(reader["PhoneNumber"].ToString());
                            item.SubItems.Add(reader["Address"].ToString());
                            lvCustomer.Items.Add(item);
                        }
                        if (lvCustomer.Items.Count == 0)
                            MessageBox.Show("No results found.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Reset()
        {
            txtSearch.Clear();
            LoadCustomerData();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
         Reset();
        }
    }
}
