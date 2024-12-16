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
    public partial class frmSupplier : Form
    {
        public frmSupplier()
        {
            InitializeComponent();
        }
        private void LoadSuppliers()
        {
            string query = "SELECT SupplierID, SupplierName, ContactName, ContactPhone, ContactEmail FROM Supplier";

            try
            {
                using (SqlConnection connection = new DatabaseConnection().GetConnection())
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Xóa tất cả các mục trong ListView trước khi tải dữ liệu mới
                        lvSupplierManagement.Items.Clear();

                        while (reader.Read())
                        {
                            // Tạo một ListViewItem mới cho mỗi nhà cung cấp
                            ListViewItem item = new ListViewItem(reader["SupplierID"].ToString());
                            item.SubItems.Add(reader["SupplierName"].ToString());
                            item.SubItems.Add(reader["ContactName"].ToString());
                            item.SubItems.Add(reader["ContactPhone"].ToString());
                            item.SubItems.Add(reader["ContactEmail"].ToString());

                            // Thêm mục vào ListView
                            lvSupplierManagement.Items.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void frmSupplier_Load(object sender, EventArgs e)
        {
            LoadSuppliers();
        }

        private void AddSupplier(string supplierName, string contactName, string contactPhone, string contactEmail)
        {
            string query = "INSERT INTO Supplier (SupplierName, ContactName, ContactPhone, ContactEmail) " +
                           "VALUES (@SupplierName, @ContactName, @ContactPhone, @ContactEmail)";

            try
            {
                using (SqlConnection connection = new DatabaseConnection().GetConnection())
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Gán giá trị tham số từ chuỗi đã nhận
                    command.Parameters.AddWithValue("@SupplierName", supplierName);
                    command.Parameters.AddWithValue("@ContactName", contactName);
                    command.Parameters.AddWithValue("@ContactPhone", contactPhone);
                    command.Parameters.AddWithValue("@ContactEmail", contactEmail);

                    connection.Open();
                    int result = command.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Supplier added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to add supplier.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            string supplierName = txtSupplierName.Text.Trim();
            string contactName = txtContactName.Text.Trim();
            string contactPhone = txtContactPhone.Text.Trim();
            string contactEmail = txtContactEmail.Text.Trim();

            // Kiểm tra dữ liệu hợp lệ
            if (string.IsNullOrEmpty(supplierName) || string.IsNullOrEmpty(contactName) ||
                string.IsNullOrEmpty(contactPhone) || string.IsNullOrEmpty(contactEmail))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Gọi hàm thêm dữ liệu
            AddSupplier(supplierName, contactName, contactPhone, contactEmail);

            // Làm mới danh sách sau khi thêm
            LoadSuppliers();
        }

        private void UpdateSupplier(int supplierId, string supplierName, string contactName, string contactPhone, string contactEmail)
        {
            string query = "UPDATE Supplier " +
                           "SET SupplierName = @SupplierName, ContactName = @ContactName, " +
                           "ContactPhone = @ContactPhone, ContactEmail = @ContactEmail " +
                           "WHERE SupplierID = @SupplierID";

            try
            {
                using (SqlConnection connection = new DatabaseConnection().GetConnection())
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Gán giá trị tham số
                    command.Parameters.AddWithValue("@SupplierID", supplierId);
                    command.Parameters.AddWithValue("@SupplierName", supplierName);
                    command.Parameters.AddWithValue("@ContactName", contactName);
                    command.Parameters.AddWithValue("@ContactPhone", contactPhone);
                    command.Parameters.AddWithValue("@ContactEmail", contactEmail);

                    connection.Open();
                    int result = command.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Supplier updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to update supplier.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (lvSupplierManagement.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = lvSupplierManagement.SelectedItems[0];
                int supplierId = int.Parse(selectedItem.SubItems[0].Text);

                string supplierName = txtSupplierName.Text.Trim();
                string contactName = txtContactName.Text.Trim();
                string contactPhone = txtContactPhone.Text.Trim();
                string contactEmail = txtContactEmail.Text.Trim();

                // Kiểm tra dữ liệu hợp lệ
                if (string.IsNullOrEmpty(supplierName) || string.IsNullOrEmpty(contactName) ||
                    string.IsNullOrEmpty(contactPhone) || string.IsNullOrEmpty(contactEmail))
                {
                    MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                UpdateSupplier(supplierId, supplierName, contactName, contactPhone, contactEmail);

                // Làm mới danh sách sau khi sửa
                LoadSuppliers();
            }
            else
            {
                MessageBox.Show("Please select a supplier to update.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DeleteSupplier(int supplierId)
        {
            string query = "DELETE FROM Supplier WHERE SupplierID = @SupplierID";

            try
            {
                using (SqlConnection connection = new DatabaseConnection().GetConnection())
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Gán giá trị tham số
                    command.Parameters.AddWithValue("@SupplierID", supplierId);

                    connection.Open();
                    int result = command.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Supplier deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete supplier.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (lvSupplierManagement.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = lvSupplierManagement.SelectedItems[0];
                int supplierId = int.Parse(selectedItem.SubItems[0].Text);

                var confirmResult = MessageBox.Show("Are you sure to delete this supplier?",
                                                    "Confirm Delete",
                                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmResult == DialogResult.Yes)
                {
                    DeleteSupplier(supplierId);

                    // Làm mới danh sách sau khi xóa
                    LoadSuppliers();
                }
            }
            else
            {
                MessageBox.Show("Please select a supplier to delete.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SearchSupplier(string keyword)
        {
            string query = "SELECT SupplierID, SupplierName, ContactName, ContactPhone, ContactEmail " +
                           "FROM Supplier " +
                           "WHERE SupplierName LIKE @Keyword OR ContactName LIKE @Keyword " +
                           "OR ContactPhone LIKE @Keyword OR ContactEmail LIKE @Keyword";

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
                        lvSupplierManagement.Items.Clear();

                        while (reader.Read())
                        {
                            ListViewItem item = new ListViewItem(reader["SupplierID"].ToString());
                            item.SubItems.Add(reader["SupplierName"].ToString());
                            item.SubItems.Add(reader["ContactName"].ToString());
                            item.SubItems.Add(reader["ContactPhone"].ToString());
                            item.SubItems.Add(reader["ContactEmail"].ToString());

                            lvSupplierManagement.Items.Add(item);
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
            string keyword = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Please enter a keyword to search.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SearchSupplier(keyword);
        }

        private void Reset()
        {
            txtSearch.Clear();
            LoadSuppliers();
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            frmHome fromhome = new frmHome();
            fromhome.Show();
            this.Hide();
        }
    }
}
