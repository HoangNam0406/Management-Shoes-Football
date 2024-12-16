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
    public partial class frmTypeProduct : Form
    {
        public frmTypeProduct()
        {
            InitializeComponent();
        }

        private void LoadTypeProduct()
        {
            DatabaseConnection conn = new DatabaseConnection();
            using (SqlConnection connection = conn.GetConnection())

                try
                {
                    connection.Open();
                    string query = "SELECT TypeID, TypeName FROM TypeFootballBoots ";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Xóa dữ liệu cũ trong ListView nếu có
                        lvTypeFootballBootsManagement.Items.Clear();

                        while (reader.Read())
                        {
                            // Thêm dòng dữ liệu vào ListView
                            ListViewItem item = new ListViewItem(reader["TypeID"].ToString());
                            item.SubItems.Add(reader["TypeName"].ToString());
                            

                            lvTypeFootballBootsManagement.Items.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }
        private void frmTypeProduct_Load(object sender, EventArgs e)
        {
            LoadTypeProduct();
        }

        private void lvTypeFootballBootsManagement_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvTypeFootballBootsManagement.SelectedItems.Count > 0)
            {
                // Lấy dòng được chọn trong ListView
                ListViewItem selectedItem = lvTypeFootballBootsManagement.SelectedItems[0];

                // Điền dữ liệu vào các TextBox
                txtTypeProductID.Text = selectedItem.SubItems[0].Text;
                txtTypeProductName.Text = selectedItem.SubItems[1].Text; 
                
            }
        }


        private void AddTypeProduct()
        {
            string query = "INSERT INTO TypeFootballBoots (TypeName) VALUES (@TypeName)";

            try
            {
                using (SqlConnection connection = new DatabaseConnection().GetConnection())
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Lấy dữ liệu từ TextBox và kiểm tra
                    string typeProductName = txtTypeProductName.Text.Trim();

                    if (string.IsNullOrEmpty(typeProductName))
                    {
                        MessageBox.Show("Type name cannot be empty!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // Dừng hàm nếu dữ liệu không hợp lệ
                    }

                    // Gán tham số cho câu lệnh SQL
                    command.Parameters.AddWithValue("@TypeName", typeProductName);

                    // Mở kết nối và thực thi câu lệnh
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    // Kiểm tra kết quả
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("TypeFootballBoots added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadTypeProduct(); // Tải lại danh sách loại giày
                    }
                    else
                    {
                        MessageBox.Show("Failed to add TypeFootballBoots.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            AddTypeProduct();
        }


        private void UpdateTypeProduct()
        {
            string query = "UPDATE TypeFootballBoots SET TypeName = @TypeName WHERE TypeID = @TypeID";

            try
            {
                using (SqlConnection connection = new DatabaseConnection().GetConnection())
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Lấy dữ liệu từ TextBox và kiểm tra
                    string typeProductID = txtTypeProductID.Text.Trim();
                    string typeProductName = txtTypeProductName.Text.Trim();

                    if (string.IsNullOrEmpty(typeProductID) || string.IsNullOrEmpty(typeProductName))
                    {
                        MessageBox.Show("Both TypeID and TypeName must be provided.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // Dừng hàm nếu dữ liệu không hợp lệ
                    }

                    if (!int.TryParse(typeProductID, out int typeID))
                    {
                        MessageBox.Show("TypeID must be a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // Dừng nếu TypeID không phải số hợp lệ
                    }

                    // Gán tham số cho câu lệnh SQL
                    command.Parameters.AddWithValue("@TypeID", typeID);
                    command.Parameters.AddWithValue("@TypeName", typeProductName);

                    // Mở kết nối và thực thi câu lệnh
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    // Kiểm tra kết quả
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("TypeFootballBoots updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadTypeProduct(); // Tải lại danh sách loại sản phẩm
                    }
                    else
                    {
                        MessageBox.Show("Failed to update TypeFootballBoots.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            UpdateTypeProduct();
        }

        private void DeleteTypeProduct()
        {
            if (MessageBox.Show("Are you sure you want to delete this TypeProduct?", "Confirm Delete",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string query = "DELETE FROM TypeFootballBoots WHERE TypeID = @TypeID";

                try
                {
                    using (SqlConnection connection = new DatabaseConnection().GetConnection())
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeID", int.Parse(txtTypeProductID.Text));

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Employee deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadTypeProduct(); // Hàm tải lại dữ liệu
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
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteTypeProduct();
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
                        lvTypeFootballBootsManagement.Items.Clear();

                        while (reader.Read())
                        {
                            // Tạo một ListViewItem mới cho mỗi kết quả
                            ListViewItem item = new ListViewItem(reader["EmployeeID"].ToString());
                            item.SubItems.Add(reader["EmployeeName"].ToString());
                            item.SubItems.Add(reader["Email"].ToString());
                            item.SubItems.Add(reader["PhoneNumber"].ToString());
                            item.SubItems.Add(reader["Position"].ToString());

                            // Thêm kết quả vào ListView
                            lvTypeFootballBootsManagement.Items.Add(item);
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

        private void btnBack_Click(object sender, EventArgs e)
        {
            frmHome fromhome = new frmHome();
            fromhome.Show();
            this.Hide();
        }

        private void Reset()
        {
            txtSearch.Clear();
            LoadTypeProduct();
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }
    }
}
