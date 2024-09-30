using MySql.Data.MySqlClient;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace etteremdejo
{
    public partial class Adminpanel : Window
    {
        private string connectionString = "Server=localhost;Database=restaurant_app;User ID=root;Password=;";
        public ObservableCollection<Ingredient> Ingredients { get; set; }

        public Adminpanel()
        {
            InitializeComponent();
            Ingredients = new ObservableCollection<Ingredient>();
            IngredientList.ItemsSource = Ingredients;
            LoadIngredients();
        }



        // Load all ingredients from the database
        private void LoadIngredients()
        {
            Ingredients.Clear();
            string query = "SELECT id, name, quantity_in_stock, unit FROM ingredients";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                connection.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Ingredients.Add(new Ingredient
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
                            QuantityInStock = reader.GetInt32("quantity_in_stock"),
                            Unit = reader.GetString("unit")
                        });
                    }
                }
            }
        }

        // Add new ingredient to the database
        private void AddIngredient_Click(object sender, RoutedEventArgs e)
        {
            string name = IngredientName.Text;
            int quantity;
            string unit = IngredientUnit.Text;

            // Check if the ingredient already exists
            if (Ingredients.Any(i => i.Name == name))
            {
                MessageBox.Show("Ez az alapanyag már létezik.");
                return;
            }

            if (int.TryParse(IngredientQuantity.Text, out quantity) && !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(unit))
            {
                string query = "INSERT INTO ingredients (name, quantity_in_stock, unit) VALUES (@name, @quantity_in_stock, @unit)";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@quantity_in_stock", quantity);
                    cmd.Parameters.AddWithValue("@unit", unit);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Alapanyag sikeresen hozzáadva!");
                LoadIngredients(); // Refresh the list
            }
            else
            {
                MessageBox.Show("Kérlek, adj meg érvényes adatokat.");
            }
        }

        // Modify existing ingredient in the database
        private void ModifyIngredient_Click(object sender, RoutedEventArgs e)
        {
            Ingredient selectedIngredient = (sender as Button)?.DataContext as Ingredient;

            if (selectedIngredient != null)
            {
                ModifyIngredientWindow modifyWindow = new ModifyIngredientWindow(selectedIngredient);
                if (modifyWindow.ShowDialog() == true) // Check if the dialog was closed with OK
                {
                    // Update the ingredient in the ObservableCollection after the dialog is closed
                    selectedIngredient.Name = modifyWindow.ModifiedIngredient.Name;
                    selectedIngredient.QuantityInStock = modifyWindow.ModifiedIngredient.QuantityInStock;
                    selectedIngredient.Unit = modifyWindow.ModifiedIngredient.Unit;

                    // Refresh the UI
                    IngredientList.Items.Refresh();
                }
            }
        }



        // Search filter for ingredients
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filter = SearchBox.Text.ToLower();
            IngredientList.ItemsSource = Ingredients.Where(i => i.Name.ToLower().Contains(filter)).ToList();
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        // Delete selected ingredient from the database
        private void DeleteIngredient_Click(object sender, RoutedEventArgs e)
        {
            Ingredient selectedIngredient = (sender as Button)?.DataContext as Ingredient;

            if (selectedIngredient != null)
            {
                string query = "DELETE FROM ingredients WHERE id = @id";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@id", selectedIngredient.Id);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Alapanyag sikeresen törölve!");
                LoadIngredients(); // Refresh the list
            }
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }

    // Ingredient class representing the items in the database
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int QuantityInStock { get; set; }
        public string Unit { get; set; }
    }
}
