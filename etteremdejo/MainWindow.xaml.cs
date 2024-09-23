using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;


namespace etteremdejo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string connectionString = "Server=localhost;Database=restaurant_app;User ID=root;Password=;";

        public MainWindow()
        {
            InitializeComponent();
        }
        // Switch to Registration form
        private void SwitchToRegister(object sender, RoutedEventArgs e)
        {
            LoginPanel.Visibility = Visibility.Collapsed;
            RegistrationPanel.Visibility = Visibility.Visible;
        }

        // Switch to Login form
        private void SwitchToLogin(object sender, RoutedEventArgs e)
        {
            RegistrationPanel.Visibility = Visibility.Collapsed;
            LoginPanel.Visibility = Visibility.Visible;
        }


        private void AddUserToDatabase(User user)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO users (username, email, password_hash, role, registration_date) VALUES (@username, @email, @password_hash, @role, @registration_date)";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", user.Username);
                    command.Parameters.AddWithValue("@email", user.Email);
                    command.Parameters.AddWithValue("@password_hash", ComputeSha256Hash(user.Password));
                    command.Parameters.AddWithValue("@role", user.Role);
                    command.Parameters.AddWithValue("@registration_date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    connection.Open();
                    try
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("Új felhasználó sikeresen hozzáadva!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hiba történt: " + ex.Message);
                    }
                }
            }
        }


        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            //var user = new User
            //{
            //    Username = "Imre Krsztián",
            //    Email = "anyad@gmail.com",
            //    Password = "asdasd",
            //    Role = "user"
            //};
            var user = new User("Imre Krsztián", "anyad@gmail.com", "asdasd", "user");

            AddUserToDatabase(user);
        }
        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }


        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string username = txtrUsername.Text;
            string email = txtrEmail.Text;
            string password = txtrPassword.Password;
            string cpassword = txtrcPassword.Password;

            // Ellenőrzések
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(cpassword))
            {
                MessageBox.Show("Minden mezőt ki kell tölteni!");
                return;
            }

            if (password != cpassword)
            {
                MessageBox.Show("A jelszavak nem egyeznek!");
                return;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Az e-mail formátuma nem érvényes!");
                return;
            }

            // Ellenőrizze, hogy a felhasználónév vagy az e-mail foglalt-e
            if (IsUsernameOrEmailTaken(username, email))
            {
                MessageBox.Show("A felhasználónév vagy az e-mail cím már foglalt!");
                return;
            }

            MessageBox.Show("Regisztráció sikeres!");

        }
        private bool IsValidEmail(string email)
        {
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }

        private bool IsUsernameOrEmailTaken(string username, string email)
        {
            

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username OR Email = @Email";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Email", email);

                    // Helyes típusként castoljuk az eredményt
                    long userCount = (long)command.ExecuteScalar();
                    return userCount > 0;
                }
            }
        }

    }
}