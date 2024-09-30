using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace etteremdejo
{
    public partial class Kaja : Window
    {
        public ObservableCollection<Food> Foods { get; set; }
        public ObservableCollection<Food> Cart { get; set; }
        private string userFilePath = "user_data.txt"; // Fájl elérési útvonala a mentett adatokhoz

        public Kaja()
        {
            InitializeComponent();
            if (GetUserRole()=="admin")
            {
                btnadmin.Visibility = Visibility.Visible;
            }

            Foods = new ObservableCollection<Food>();
            Cart = new ObservableCollection<Food>();
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
                                ImageUrl = reader.GetString("image_url"),
                                IsAddedToCart = false // Initialize as not added
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

        private void AddAllToCart(object sender, RoutedEventArgs e)
        {
            bool itemsAdded = false; // Track if any items were added

            foreach (var food in Foods)
            {
                if (food.IsAddedToCart) // Only add if it's marked as added
                {
                    var container = FindContainerForFood(food);
                    var lblMennyiseg = container.ContentTemplate.FindName("lblMennyiseg", container) as Label;
                    int quantity = Convert.ToInt32(lblMennyiseg.Content);

                    for (int i = 0; i < quantity; i++)
                    {
                        Cart.Add(food);
                    }

                    // Reset the food item state
                    food.IsAddedToCart = false; // Reset the added state
                    lblMennyiseg.Content = 1; // Reset quantity to default
                    var btnKosarba = container.ContentTemplate.FindName("btnKosarba", container) as Button;
                    btnKosarba.Visibility = Visibility.Visible; // Show "Kosárba" button again

                    // Hide the tobbAdd StackPanel
                    var tobbAdd = container.ContentTemplate.FindName("tobbAdd", container) as StackPanel;
                    tobbAdd.Visibility = Visibility.Collapsed; // Hide the additional quantity panel

                    itemsAdded = true; // Mark that items were added
                }
            }

            // Show success message if items were added
            if (itemsAdded)
            {
                MessageBox.Show("A termékek sikeresen hozzáadva a kosárhoz!");
            }
        }

        private void AddToCart(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var container = FindVisualParent<ContentPresenter>(button);

            var food = (Food)container.DataContext; // Get the food item
            food.IsAddedToCart = true; // Mark it as added

            var tobbAdd = container.ContentTemplate.FindName("tobbAdd", container) as StackPanel;
            tobbAdd.Visibility = Visibility.Visible;
            button.Visibility = Visibility.Collapsed;
        }

        private ContentPresenter FindContainerForFood(Food food)
        {
            var itemsControl = FoodsScrollViewer.Content as ItemsControl;

            foreach (var item in itemsControl.Items)
            {
                var container = itemsControl.ItemContainerGenerator.ContainerFromItem(item) as ContentPresenter;

                if (container != null && container.Content is Food itemFood)
                {
                    if (itemFood.Id == food.Id)
                    {
                        return container;
                    }
                }
            }
            return null;
        }

        private void btnElvesz(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var container = FindVisualParent<ContentPresenter>(button);
            var lblMennyiseg = container.ContentTemplate.FindName("lblMennyiseg", container) as Label;
            var tobbAdd = container.ContentTemplate.FindName("tobbAdd", container) as StackPanel;
            var btnKosarba = container.ContentTemplate.FindName("btnKosarba", container) as Button;

            if (Convert.ToInt32(lblMennyiseg.Content) == 1)
            {
                tobbAdd.Visibility = Visibility.Collapsed;
                btnKosarba.Visibility = Visibility.Visible;
            }
            else
            {
                lblMennyiseg.Content = Convert.ToInt32(lblMennyiseg.Content) - 1;
            }
        }

        private void btnHozzaad(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var container = FindVisualParent<ContentPresenter>(button);
            var lblMennyiseg = container.ContentTemplate.FindName("lblMennyiseg", container) as Label;
            lblMennyiseg.Content = Convert.ToInt32(lblMennyiseg.Content) + 1;
        }

        private T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;

            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                return FindVisualParent<T>(parentObject);
            }
        }

        private void ShowCart(object sender, RoutedEventArgs e)
        {
            FoodsScrollViewer.Visibility = Visibility.Collapsed;
            CartScrollViewer.Visibility = Visibility.Visible; // Kosár megjelenítése
            var btnAddAllToCart = (Button)FindName("btnAddAllToCart");
            btnAddAllToCart.Visibility = Visibility.Collapsed;
            var btnRendeles = (Button)FindName("btnRendeles");
            btnRendeles.Visibility = Visibility.Visible; // Rendelés gomb látható
        }

        private void ShowFoods(object sender, RoutedEventArgs e)
        {
            FoodsScrollViewer.Visibility = Visibility.Visible;
            CartScrollViewer.Visibility = Visibility.Collapsed;
            var btnAddAllToCart = (Button)FindName("btnAddAllToCart");
            btnAddAllToCart.Visibility = Visibility.Visible;
            var btnRendeles = (Button)FindName("btnRendeles");
            btnRendeles.Visibility = Visibility.Collapsed;
        }

        private void DeleteFromCart(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            var item = btn.DataContext as Food;

            if (item != null)
            {
                // Eltávolítás a kosárból
                Cart.Remove(item); // Az elem törlése a kosárból
            }
        }

        private void RendelesLeadasa(object sender, RoutedEventArgs e)
        {
            int userId = GetUserId();
            List<CartItem> cartItems = GetCartItems();

            if (cartItems.Count > 0)
            {
                using (var connection = new MySqlConnection("Server=localhost;Database=restaurant_app;User ID=root;Password=;"))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Check if there are enough ingredients in stock
                            if (!CheckIngredientAvailability(connection, transaction, cartItems))
                            {
                                return;
                            }

                            // Insert the order
                            var orderId = InsertOrder(connection, transaction, userId);

                            // Insert the order items
                            foreach (var item in cartItems)
                            {
                                InsertOrderItem(connection, transaction, orderId, item);
                            }

                            // Update the ingredient stock after a successful order
                            UpdateIngredientStock(connection, transaction, cartItems);

                            // Commit the transaction
                            transaction.Commit();

                            // Clear the cart after a successful order
                            Cart.Clear();
                            MessageBox.Show("Rendelés sikeresen leadva!");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show("Hiba történt a rendelés leadásakor: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("A kosarad üres!");
            }
        }
        private bool CheckIngredientAvailability(MySqlConnection connection, MySqlTransaction transaction, List<CartItem> cartItems)
        {
            foreach (var item in cartItems)
            {
                string query = @"
            SELECT ingredients.id, ingredients.quantity_in_stock, food_ingredients.quantity_required 
            FROM ingredients
            JOIN food_ingredients ON food_ingredients.ingredient_id = ingredients.id
            WHERE food_ingredients.food_id = @foodId";

                using (var cmd = new MySqlCommand(query, connection, transaction))
                {
                    cmd.Parameters.AddWithValue("@foodId", item.FoodId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            decimal stockQuantity = reader.GetDecimal("quantity_in_stock"); // Use decimal for precise stock quantities
                            decimal requiredQuantity = reader.GetDecimal("quantity_required") * item.Quantity; // Calculate required quantity

                            if (stockQuantity < requiredQuantity)
                            {
                                MessageBox.Show($"Nincs elegendő alapanyag {item.FoodId} termékhez. Készlet: {stockQuantity} kg, szükséges: {requiredQuantity} kg.");
                                return false; // Not enough stock for this item
                            }

                            if (stockQuantity <= 0)
                            {
                                MessageBox.Show($"A {item.FoodId} alapanyagból nincs raktáron.");
                                return false; // No stock available
                            }
                        }
                    }
                }
            }
            return true;
        }




        private void UpdateIngredientStock(MySqlConnection connection, MySqlTransaction transaction, List<CartItem> cartItems)
        {
            foreach (var item in cartItems)
            {
                string query = @"
            UPDATE ingredients
            JOIN food_ingredients ON food_ingredients.ingredient_id = ingredients.id
            SET ingredients.quantity_in_stock = ingredients.quantity_in_stock - (food_ingredients.quantity_required * @quantity)
            WHERE food_ingredients.food_id = @foodId";

                using (var cmd = new MySqlCommand(query, connection, transaction))
                {
                    cmd.Parameters.AddWithValue("@foodId", item.FoodId);
                    cmd.Parameters.AddWithValue("@quantity", item.Quantity);
                    cmd.ExecuteNonQuery();
                }
            }
        }





        private int InsertOrder(MySqlConnection connection, MySqlTransaction transaction, int userId)
        {
            string sql = "INSERT INTO orders (user_id) VALUES (@userId); SELECT LAST_INSERT_ID();";
            using (var cmd = new MySqlCommand(sql, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@userId", userId);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private void InsertOrderItem(MySqlConnection connection, MySqlTransaction transaction, int orderId, CartItem item)
        {
            string sql = "INSERT INTO order_items (order_id, food_id, quantity, total_price) VALUES (@orderId, @foodId, @quantity, @totalPrice)";
            using (var cmd = new MySqlCommand(sql, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@orderId", orderId);
                cmd.Parameters.AddWithValue("@foodId", item.FoodId);
                cmd.Parameters.AddWithValue("@quantity", item.Quantity);
                cmd.Parameters.AddWithValue("@totalPrice", item.TotalPrice);
                cmd.ExecuteNonQuery();
            }
        }

        private List<CartItem> GetCartItems()
        {
            List<CartItem> cartItems = new List<CartItem>();

            // Az ObservableCollection<Cart> iterálása
            foreach (var food in Cart)
            {
                // Számoljuk meg, hány alkalommal szerepel az étel a kosárban
                int quantity = Cart.Count(item => item.Id == food.Id);

                // Hozzáadjuk a CartItem-ot a listához, ha van mennyiség
                if (quantity > 0)
                {
                    cartItems.Add(new CartItem
                    {
                        FoodId = food.Id,
                        Quantity = quantity,
                        TotalPrice = quantity * food.Price // TotalPrice kiszámítása
                    });
                }
            }

            return cartItems;
        }

        private int GetUserId()
        {
            string username = string.Empty;

            // Read the username from user_data.txt
            using (StreamReader reader = new StreamReader("user_data.txt"))
            {
                // Read the first line (the username)
                username = reader.ReadLine();
            }

            // Now query the database for the user ID based on the username
            int userId = -1; // Default value if not found


            using (var connection = new MySqlConnection("Server=localhost;Database=restaurant_app;User ID=root;Password=;"))
            {
                connection.Open();
                string query = "SELECT id FROM users WHERE username = @username"; // Adjust the table name and column name as needed

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);

                    // Execute the query and retrieve the user_id
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        userId = Convert.ToInt32(result);
                    }
                }
            }

            return userId;
        }

        private void KijelentkezesButton_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(userFilePath, string.Empty);
            MainWindow Login = new MainWindow();
            Login.Show();
            this.Close();
        }


        private string GetUserRole()
        {
            string[] userData = File.ReadAllLines(userFilePath);
            string username = userData[0];
            string passwordHash = userData[1]; 
            string role = "";
            string connectionString = "Server=localhost;Database=restaurant_app;User ID=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT role FROM users WHERE username = @username AND password_hash = @password_hash";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password_hash", passwordHash);

                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            role = result.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Adatbázis hiba: {ex.Message}");
                }
            }

            return role;
        }

        private void btnadmin_Click(object sender, RoutedEventArgs e)
        {
            Adminpanel adminpanel = new Adminpanel();
            adminpanel.ShowDialog();
        }
        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        private void btnTalca_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnKilep_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }

    public class Food
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public bool IsAddedToCart { get; set; } // Kosárban van-e
    }

    public class CartItem
    {
        public int FoodId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}

