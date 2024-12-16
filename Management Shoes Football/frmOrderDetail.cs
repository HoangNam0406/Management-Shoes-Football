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
    public partial class frmOrderDetail : Form
    {
        public frmOrderDetail()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void LoadOrderDetail()
        {
            string query = "SELECT OrderDetailID, OrderID, BootID, Quantity, Price FROM OrderDetail";

            try
            {
                using (SqlConnection connection = new DatabaseConnection().GetConnection())
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Xóa tất cả các mục trong ListView trước khi thêm mới
                        lvOrderDetailManagement.Items.Clear();

                        while (reader.Read())
                        {
                            // Tạo một ListViewItem mới cho mỗi nhân viên
                            ListViewItem item = new ListViewItem(reader["OrderDetailID"].ToString());
                            item.SubItems.Add(reader["OrderID"].ToString());
                            item.SubItems.Add(reader["BootID"].ToString());
                            item.SubItems.Add(reader["Quantity"].ToString());
                            item.SubItems.Add(reader["Price"].ToString());

                            // Thêm mục vào ListView
                            lvOrderDetailManagement.Items.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void frmOrderDetail_Load(object sender, EventArgs e)
        {
            LoadOrderDetail(); 
        }

        private void AddOrderDetail()
        {
            string query = "INSERT INTO OrderDetail (OrderID, BootID, Quantity, Price) " +
                           "VALUES (@OrderID, @BootID, @Quantity, @Price)";

            try
            {
                using (SqlConnection connection = new DatabaseConnection().GetConnection())
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Lấy và kiểm tra dữ liệu đầu vào
                    string orderID = txtOrderID.Text.Trim();
                    string bootID = txtbootID.Text.Trim();
                    string quantityText = txtQuantity.Text.Trim();
                    string priceText = txtPrice.Text.Trim();

                    // Kiểm tra dữ liệu không được để trống
                    if (string.IsNullOrEmpty(orderID) || string.IsNullOrEmpty(bootID) ||
                        string.IsNullOrEmpty(quantityText) || string.IsNullOrEmpty(priceText))
                    {
                        MessageBox.Show("All fields are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Kiểm tra và chuyển đổi kiểu dữ liệu
                    if (!int.TryParse(orderID, out int parsedOrderID))
                    {
                        MessageBox.Show("OrderID must be a valid integer.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!int.TryParse(bootID, out int parsedBootID))
                    {
                        MessageBox.Show("BootID must be a valid integer.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!int.TryParse(quantityText, out int parsedQuantity) || parsedQuantity <= 0)
                    {
                        MessageBox.Show("Quantity must be a positive integer.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!decimal.TryParse(priceText, out decimal parsedPrice) || parsedPrice <= 0)
                    {
                        MessageBox.Show("Price must be a positive number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Gán tham số vào câu lệnh SQL
                    command.Parameters.AddWithValue("@OrderID", parsedOrderID);
                    command.Parameters.AddWithValue("@BootID", parsedBootID);
                    command.Parameters.AddWithValue("@Quantity", parsedQuantity);
                    command.Parameters.AddWithValue("@Price", parsedPrice);

                    // Mở kết nối và thực thi câu lệnh
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    // Kiểm tra kết quả
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("OrderDetail added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadOrderDetail(); // Tải lại danh sách chi tiết đơn hàng
                    }
                    else
                    {
                        MessageBox.Show("Failed to add OrderDetail.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddOrderDetail();
        }


        private void UpdateOrderDetail()
        {
            string query = "UPDATE OrderDetail SET OrderID = @OrderID, BootID = @BootID, " +
                   "Quantity = @Quantity, Price = @Price WHERE OrderDetailID = @OrderDetailID";

            try
            {
                using (SqlConnection connection = new DatabaseConnection().GetConnection())
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderDetailID", txtOrderDetailID.Text);
                    command.Parameters.AddWithValue("@OrderID", txtOrderID.Text);
                    command.Parameters.AddWithValue("@BootID", txtbootID.Text);
                    command.Parameters.AddWithValue("@Quantity", txtQuantity.Text);
                    command.Parameters.AddWithValue("@Price", txtPrice.Text);
                    

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Employee updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadOrderDetail(); // Hàm tải lại dữ liệu
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
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateOrderDetail();
        }

        private void lvOrderDetailManagement_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvOrderDetailManagement.SelectedItems.Count > 0)
            {
                // Lấy dòng được chọn trong ListView
                ListViewItem selectedItem = lvOrderDetailManagement.SelectedItems[0];

                // Điền dữ liệu vào các TextBox
                txtOrderDetailID.Text = selectedItem.SubItems[0].Text;  
                txtOrderID.Text = selectedItem.SubItems[1].Text;  
                txtbootID.Text = selectedItem.SubItems[2].Text;  
                txtQuantity.Text = selectedItem.SubItems[3].Text;  
                txtPrice.Text = selectedItem.SubItems[4].Text;  
            }
        }


        private void DeleteOrderDetail()
        {
            if (MessageBox.Show("Are you sure you want to delete this OrderDetail?", "Confirm Delete",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string query = "DELETE FROM OrderDetail WHERE OrderDetailID = @OrderDetailID";

                try
                {
                    using (SqlConnection connection = new DatabaseConnection().GetConnection())
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OrderDetailID", int.Parse(txtOrderDetailID.Text));

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("OrderDetail deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadOrderDetail(); // Hàm tải lại dữ liệu
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete OrderDetail.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            DeleteOrderDetail();
        }


        private void SearchOrderDetail(string searchKeyword)
        {
            string query = "SELECT OrderDetailID, OrderID, BootID, Quantity, Price " +
                           "FROM OrderDetail " +
                           "WHERE OrderDetailID LIKE @SearchKeyword OR Price LIKE @SearchKeyword";

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
                        lvOrderDetailManagement.Items.Clear();

                        while (reader.Read())
                        {
                            // Tạo một ListViewItem mới cho mỗi kết quả
                            ListViewItem item = new ListViewItem(reader["OrderDetailID"].ToString());
                            item.SubItems.Add(reader["OrderID"].ToString());
                            item.SubItems.Add(reader["BootID"].ToString());
                            item.SubItems.Add(reader["Quantity"].ToString());
                            item.SubItems.Add(reader["Price"].ToString());

                            // Thêm kết quả vào ListView
                            lvOrderDetailManagement.Items.Add(item);
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
            SearchOrderDetail(searchKeyword);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            frmHome fromhome = new frmHome();
            fromhome.Show();
            this.Hide();
        }


        private void Reset()
        {
            txtSearch.Clear();
            LoadOrderDetail();
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }
    }
}
