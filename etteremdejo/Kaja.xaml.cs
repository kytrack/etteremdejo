using MySql.Data.MySqlClient;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace etteremdejo
{
    public partial class Kaja : Window
    {
        public ObservableCollection<Food> Foods { get; set; }

        public Kaja()
        {
            InitializeComponent();
            Foods = new ObservableCollection<Food>();
            LoadFoodsFromDatabase();
            DataContext = this; // Binding the data context to access in XAML
        }

        private void LoadFoodsFromDatabase()
        {
            string connectionString = "Server=localhost;Database=restaurant_app;User ID=root;Password=;";
            

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    // Az új lekérdezés a képek URL-jeinek betöltésére
                    string query = "SELECT id, name, description, price, image_url FROM foods";
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Foods.Add(new Food
                            {
                                Id = reader.GetInt32("id"),
                                Name = reader.GetString("name"),
                                Description = reader.GetString("description"),
                                Price = reader.GetDecimal("price"),
                                ImageUrl = reader.GetString("image_url") // Kép URL mező hozzáadása
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hiba az adatok betöltése közben: {ex.Message}");
                }
            }
        }
    }

    public class Food
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }  // Új property a kép URL-hez
    }
}
