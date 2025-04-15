using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace RubberDuckStore.Pages
{
    public class OrderConfirmationModel : PageModel
    {
        public Order Order { get; set; }
        public Duck Duck { get; set; }

        public IActionResult OnGet(int orderId)
        {
            Order = GetOrderById(orderId);
            if (Order == null)
            {
                return NotFound();
            }
            Duck = GetDuckById(Order.DuckId);
            return Page();
        }
        //Gets the order from database and populates the order object with the values

        private Order GetOrderById(int id)
        {
            using (var connection = new SqliteConnection("Data Source=RubberDucks.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Orders WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Order
                        {
                            Id = reader.GetInt32(0),
                            DuckId = reader.GetInt32(1),
                            CustomerName = reader.GetString(2),
                            CustomerEmail = reader.GetString(3),
                            Quantity = reader.GetInt32(4)
                        };
                    }
                }
            }
            return null;
        }

        //Get the duck values from the databse and populate the attributes
        //on a new Duck object

        private Duck GetDuckById(int id)
        {
            using (var connection = new SqliteConnection("Data Source=RubberDucks.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Ducks WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Duck
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            Price = reader.GetDecimal(3),
                            ImageFileName = reader.GetString(4)
                        };
                    }
                }
            }
            return null;
        }
    }
}

