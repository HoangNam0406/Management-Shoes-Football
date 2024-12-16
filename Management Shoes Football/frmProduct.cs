using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Management_Shoes_Football.Form1;

namespace Management_Shoes_Football
{
    public partial class frmProduct : Form
    {
        public frmProduct()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void SearchProduct(string keyword)
        {
            string query = "SELECT BootID, BootName, TypeID, Size, Price, StockQuantity, " +
                           "FROM FootballBoots " +
                           "WHERE BootName LIKE @Keyword OR BootID LIKE @Keyword " +
                           "OR Size LIKE @Keyword ";

            try
            {
                using (SqlConnection connection = new DatabaseConnection().GetConnection())
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Thêm tham số tìm kiếm với ký tự đại diện
                    command.Parameters.AddWithValue("@Keyword", $"%{keyword}%");

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Xóa dữ liệu cũ trong ListView trước khi hiển thị kết quả mới
                        lvProduct.Items.Clear();

                        while (reader.Read())
                        {
                            ListViewItem item = new ListViewItem(reader["BootID"].ToString());
                            item.SubItems.Add(reader["BootName"].ToString());
                            item.SubItems.Add(reader["TypeID"].ToString());
                            item.SubItems.Add(reader["Size"].ToString());
                            item.SubItems.Add(reader["Price"].ToString());
                            item.SubItems.Add(reader["StockQuantity"].ToString());
                            item.SubItems.Add(reader["SupplierID"].ToString());
                            lvProduct.Items.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Please enter a keyword to search.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SearchProduct(keyword);
        }

        private void frmProduct_Load(object sender, EventArgs e)
        {
            LoadProduct();
        }

        private void LoadProduct()
        {
            string query = "SELECT BootID, BootName, TypeID, Size, Price, StockQuantity, SupplierID FROM FootballBoots";

            try
            {
                using (SqlConnection connection = new DatabaseConnection().GetConnection())
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Xóa tất cả các mục trong ListView trước khi tải dữ liệu mới
                        lvProduct.Items.Clear();

                        while (reader.Read())
                        {
                            // Tạo một ListViewItem mới cho mỗi nhà cung cấp
                            ListViewItem item = new ListViewItem(reader["BootID"].ToString());
                            item.SubItems.Add(reader["BootName"].ToString());
                            item.SubItems.Add(reader["TypeID"].ToString());
                            item.SubItems.Add(reader["Size"].ToString());
                            item.SubItems.Add(reader["Price"].ToString());
                            item.SubItems.Add(reader["Stockquantity"].ToString());
                            item.SubItems.Add(reader["SupplierID"].ToString());
                            // Thêm mục vào ListView
                            lvProduct.Items.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void AddFootballBoot()
        {
            string bootID = txtBootID.Text;
            string bootName = txtBootName.Text;
            string typeID = txtTypeID.Text.ToString();
            string size = txtSize.Text.ToString();
            string price = txtPrice.Text.ToString();
            string stockQuantity = txtStockQuantity.Text.ToString();
            string supplierID = txtSupplierID.Text.ToString();


            string query = "INSERT INTO FootballBoots (BootName, TypeID, Size, Price, StockQuantity, SupplierID) VALUES (@BootName, @TypeID, @Size, @Price, @StockQuantity, @SupplierID)";

            using (SqlConnection conn = new DatabaseConnection().GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@BootID", bootID);
                    cmd.Parameters.AddWithValue("@BootName", bootName);
                    cmd.Parameters.AddWithValue("@TypeID", typeID);
                    cmd.Parameters.AddWithValue("@Size", size);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@StockQuantity", stockQuantity);
                    cmd.Parameters.AddWithValue("@SupplierID", supplierID);


                    cmd.ExecuteNonQuery();
                    LoadProduct();
                }
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string bootName = txtBootName.Text.Trim();
                int typeID = int.Parse(txtTypeID.Text.Trim());
                int size = int.Parse(txtSize.Text.Trim());
                decimal price = decimal.Parse(txtPrice.Text.Trim());
                int stockQuantity = int.Parse(txtStockQuantity.Text.Trim());
                int supplierID = int.Parse(txtSupplierID.Text.Trim());

                // Kiểm tra dữ liệu hợp lệ
                if (price <= 0 || stockQuantity <= 0)
                {
                    MessageBox.Show("Price and Stock Quantity must be greater than 0.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                MessageBox.Show("Product added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Refresh data (Hàm load lại dữ liệu nếu có)
                AddFootballBoot();
                LoadProduct();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Add Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateProduct()
        {
            string bootID = txtBootID.Text;
            string bootName = txtBootName.Text;
            string typeID = txtTypeID.Text.ToString();
            string size = txtSize.Text.ToString();
            string price = txtPrice.Text.ToString();
            string stockQuantity = txtStockQuantity.Text.ToString();
            string supplierID = txtSupplierID.Text.ToString();


            string query = "UPDATE FootballBoots SET BootName = @BootName, " +
                "TypeID = @TypeID, " +
                "Size = @Size, " +
                "Price = @Price, " +
                "StockQuantity = @StockQuantity, " +
                "SupplierID = @SupplierID WHERE BootID = @BootID";

            try
            {
                using (SqlConnection connection = new DatabaseConnection().GetConnection())
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Gán giá trị tham số
                    command.Parameters.AddWithValue("@BootID", bootID);
                    command.Parameters.AddWithValue("@BootName", bootName);
                    command.Parameters.AddWithValue("@TypeID", typeID);
                    command.Parameters.AddWithValue("@Size", size);
                    command.Parameters.AddWithValue("@Price", price);
                    command.Parameters.AddWithValue("@StockQuantity", stockQuantity);
                    command.Parameters.AddWithValue("@SupplierID", supplierID);

                    connection.Open();
                    int result = command.ExecuteNonQuery();

                    // Kiểm tra dữ liệu hợp lệ
                    if (string.IsNullOrEmpty(bootName) || string.IsNullOrEmpty(typeID) ||
                        string.IsNullOrEmpty(size) || string.IsNullOrEmpty(price))
                    {
                        MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (result > 0)
                    {
                        MessageBox.Show("Football Boots updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to update Football Boots.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
             UpdateProduct();
            LoadProduct();
        }

        private void lvProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvProduct.SelectedItems.Count > 0)
            {
                // Lấy dòng được chọn trong ListView
                ListViewItem selectedItem = lvProduct.SelectedItems[0];

                // Điền dữ liệu vào các TextBox
                txtBootID.Text = selectedItem.SubItems[0].Text;  // EmployeeID
                txtBootName.Text = selectedItem.SubItems[1].Text;  // EmployeeName
                txtTypeID.Text = selectedItem.SubItems[2].Text;
                txtSize.Text = selectedItem.SubItems[3].Text;
                txtPrice.Text = selectedItem.SubItems[4].Text;  // Email
                txtStockQuantity.Text = selectedItem.SubItems[5].Text;  // PhoneNumber
                txtSupplierID.Text = selectedItem.SubItems[6].Text;  // Position
            }
        }

        private void DeleteFootballBoots(int bootID)
        {
            string query = "DELETE FROM FootBallBoots WHERE BootID = @BootID";

            try
            {
                using (SqlConnection connection = new DatabaseConnection().GetConnection())
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Gán giá trị tham số
                    command.Parameters.AddWithValue("@BootID", bootID);

                    connection.Open();
                    int result = command.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Football Boots deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete Football Boots.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lvProduct.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = lvProduct.SelectedItems[0];
                int supplierId = int.Parse(selectedItem.SubItems[0].Text);

                var confirmResult = MessageBox.Show("Are you sure to delete this supplier?",
                                                    "Confirm Delete",
                                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmResult == DialogResult.Yes)
                {
                    DeleteFootballBoots(supplierId);

                    // Làm mới danh sách sau khi xóa
                    LoadProduct();
                }
            }
            else
            {
                MessageBox.Show("Please select a supplier to delete.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
            LoadProduct();
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }
    }
}
