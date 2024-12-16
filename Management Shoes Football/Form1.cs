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

namespace Management_Shoes_Football
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            txtPassword.PasswordChar = '*';
        }

        public class DatabaseConnection
        {
            // Chuỗi kết nối đến SQL Server
            private readonly string connectionString;

            // Constructor khởi tạo chuỗi kết nối
            public DatabaseConnection()
            {
                connectionString = @"Server=DESKTOP-TJHT9JP\NGUYENHOANGNAM;Database=soccer_shoes_store_management;Trusted_Connection=True;";
            }

            // Hàm trả về đối tượng SqlConnection
            public SqlConnection GetConnection()
            {
                return new SqlConnection(connectionString);
            }
        }
        private bool checkLogin(string username, string password)
        {
            DatabaseConnection conn = new DatabaseConnection();
            using (SqlConnection connection = conn.GetConnection())
            {
                try
                {
                    // Mở kết nối
                    connection.Open();

                    // Câu lệnh SQL kiểm tra thông tin đăng nhập
                    string query = "SELECT COUNT(1) FROM LoginForm WHERE USERNAME = @Username AND PASSWORDS = @Password";

                    // Sử dụng SqlCommand để thực thi truy vấn
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Tham số hóa để tránh SQL Injection
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);

                        // Thực hiện truy vấn và lấy kết quả
                        int count = Convert.ToInt32(command.ExecuteScalar());

                        // Trả về true nếu thông tin hợp lệ
                        return count == 1;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi kết nối cơ sở dữ liệu: " + ex.Message);
                    return false;
                }
            }
        }


        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text;
            string password = txtPassword.Text;

            if (checkLogin(username, password))
            {
                //MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Mở form frmMain khi đăng nhập thành công
                frmHome mainForm = new frmHome();
                mainForm.Show();

                // Ẩn form đăng nhập (LoginForm)
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void chkShow_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShow.Checked)
            {
                txtPassword.PasswordChar = '\0'; // Hiển thị mật khẩu
            }
            else
            {
                txtPassword.PasswordChar = '*'; // Ẩn mật khẩu
            }
        }
    }
}
