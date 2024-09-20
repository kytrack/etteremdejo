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
    } 
}