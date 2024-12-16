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
    public partial class frmOrder : Form
    {
        public frmOrder()
        {
            InitializeComponent();
        }

        private void LoadOrders()
        {
            string query = "SELECT OrderID, OrderDate, CustomerID, EmployeeID, TotalAmount, Status FROM Orders";

            try
            {
                using (SqlConnection connection = new DatabaseConnection().GetConnection())
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Xóa tất cả các mục trong ListView trước khi thêm mới
                        lvOrderManagement.Items.Clear();

                        while (reader.Read())
                        {
                            // Tạo một ListViewItem mới cho mỗi nhân viên
                            ListViewItem item = new ListViewItem(reader["OrderID"].ToString());
                            item.SubItems.Add(reader["OrderDate"].ToString());
                            item.SubItems.Add(reader["CustomerID"].ToString());
                            item.SubItems.Add(reader["EmployeeID"].ToString());
                            item.SubItems.Add(reader["TotalAmount"].ToString());
                            item.SubItems.Add(reader["Status"].ToString());

                            // Thêm mục vào ListView
                            lvOrderManagement.Items.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmOrder_Load(object sender, EventArgs e)
        {
            LoadOrders();

        }

        private void UpdateOrders()
        {
            string query = "UPDATE Orders SET CustomerID = @CustomerID, EmployeeID = @EmployeeID, " +
                   "OrderDate = @OrderDate, TotalAmount = @TotalAmount, Status = @Status WHERE OrderID = @OrderID";

            try
            {
                using (SqlConnection connection = new DatabaseConnection().GetConnection())
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderID", txtOrderID.Text);
                    command.Parameters.AddWithValue("@OrderDate", dtpOrderDate.Value);
                    command.Parameters.AddWithValue("@CustomerID", txtCustomerID.Text);
                    command.Parameters.AddWithValue("@EmployeeID", txtEmployeeID.Text);
                    command.Parameters.AddWithValue("@TotalAmount", txtTotalAmount.Text);
                    command.Parameters.AddWithValue("@Status", txtStatus.Text);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Orders updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadOrders(); // Hàm tải lại dữ liệu
                    }
                    else
                    {
                        MessageBox.Show("Failed to update Orders.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateOrders();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string query = "INSERT INTO Orders (OrderDate, CustomerID, EmployeeID, TotalAmount, Status) " +
                   "VALUES (@OrderDate, @CustomerID, @EmployeeID, @TotalAmount, @Status)";

            try
            {
                using (SqlConnection connection = new DatabaseConnection().GetConnection())
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderID", txtOrderID.Text);
                    command.Parameters.AddWithValue("@OrderDate", dtpOrderDate.Value);
                    command.Parameters.AddWithValue("@CustomerID", txtCustomerID.Text);
                    command.Parameters.AddWithValue("@EmployeeID", txtEmployeeID.Text);
                    command.Parameters.AddWithValue("@TotalAmount", decimal.Parse(txtTotalAmount.Text));
                    command.Parameters.AddWithValue("@Status", txtStatus.Text.ToString());

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Order added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadOrders(); // Hàm tải lại dữ liệu
                    }
                    else
                    {
                        MessageBox.Show("Failed to add order.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            LoadOrders();
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }


        private void DeleteOrders()
        {
            if (MessageBox.Show("Are you sure you want to delete this Orders?", "Confirm Delete",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string query = "DELETE FROM Orders WHERE OrderID = @OrderID";

                try
                {
                    using (SqlConnection connection = new DatabaseConnection().GetConnection())
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OrderID", int.Parse(txtOrderID.Text));

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Orders deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadOrders(); // Hàm tải lại dữ liệu
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete Orders.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteOrders();
        }

        private void SearchOrders(string searchKeyword)
        {
            string query = "SELECT OrderID, OrderDate, CustomerID, EmployeeID, TotalAmount, Status " +
                           "FROM Orders " +
                           "WHERE CustomerID LIKE @SearchKeyword OR EmployeeID LIKE @SearchKeyword";

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
                        lvOrderManagement.Items.Clear();

                        while (reader.Read())
                        {
                            // Tạo một ListViewItem mới cho mỗi kết quả
                            ListViewItem item = new ListViewItem(reader["OrderID"].ToString());
                            item.SubItems.Add(reader["OrderDate"].ToString());
                            item.SubItems.Add(reader["CustomerID"].ToString());
                            item.SubItems.Add(reader["EmployeeID"].ToString());
                            item.SubItems.Add(reader["TotalAmount"].ToString());
                            item.SubItems.Add(reader["Status"].ToString());

                            // Thêm kết quả vào ListView
                            lvOrderManagement.Items.Add(item);
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
            SearchOrders(searchKeyword);
        }

        private void lvOrderManagement_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvOrderManagement.SelectedItems.Count > 0)
            {
                // Lấy dòng được chọn trong ListView
                ListViewItem selectedItem = lvOrderManagement.SelectedItems[0];

                // Điền dữ liệu vào các TextBox
                txtOrderID.Text = selectedItem.SubItems[0].Text;
                dtpOrderDate.Text = selectedItem.SubItems[1].Text;  // EmployeeID
                txtCustomerID.Text = selectedItem.SubItems[2].Text;  // EmployeeName
                txtEmployeeID.Text = selectedItem.SubItems[3].Text;  // Email
                txtTotalAmount.Text = selectedItem.SubItems[4].Text;  // PhoneNumber
                txtStatus.Text = selectedItem.SubItems[5].Text;  // Position
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            frmHome fromhome = new frmHome();
            fromhome.Show();
            this.Hide();
        }
    }
}
