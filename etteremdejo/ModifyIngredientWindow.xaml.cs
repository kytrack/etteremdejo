using MySql.Data.MySqlClient;
using System.Windows;

namespace etteremdejo
{
    public partial class ModifyIngredientWindow : Window
    {
        public Ingredient ModifiedIngredient { get; private set; } // Expose the modified ingredient

        private Ingredient _ingredient;

        public ModifyIngredientWindow(Ingredient ingredient)
        {
            InitializeComponent();
            _ingredient = ingredient;

            // Pre-fill the fields with the existing data
            IngredientNameBox.Text = _ingredient.Name;
            IngredientQuantityBox.Text = _ingredient.QuantityInStock.ToString();
            IngredientUnitBox.Text = _ingredient.Unit;
        }

        // Save changes to the database
        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            string newName = IngredientNameBox.Text;
            int newQuantity;
            string newUnit = IngredientUnitBox.Text;

            if (int.TryParse(IngredientQuantityBox.Text, out newQuantity) && !string.IsNullOrEmpty(newName) && !string.IsNullOrEmpty(newUnit))
            {
                // Update the properties of the modified ingredient
                ModifiedIngredient = new Ingredient
                {
                    Id = _ingredient.Id, // Keep the same ID
                    Name = newName,
                    QuantityInStock = newQuantity,
                    Unit = newUnit
                };

                // Now save to the database
                string query = "UPDATE ingredients SET name = @newName, quantity_in_stock = @newQuantity, unit = @newUnit WHERE id = @id";
                using (MySqlConnection connection = new MySqlConnection("Server=localhost;Database=restaurant_app;User ID=root;Password=;"))
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@newName", newName);
                    cmd.Parameters.AddWithValue("@newQuantity", newQuantity);
                    cmd.Parameters.AddWithValue("@newUnit", newUnit);
                    cmd.Parameters.AddWithValue("@id", _ingredient.Id);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Alapanyag sikeresen módosítva!");
                DialogResult = true; // Set the dialog result to true
                Close(); // Close the modify window
            }
            else
            {
                MessageBox.Show("Kérlek, adj meg érvényes adatokat.");
            }
        }
        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
