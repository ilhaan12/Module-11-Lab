using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace RubberDuckStore.Pages {
    // This is a PageModel for a Razor Page that handles displaying rubber duck products
    public class RubberDucksModel : PageModel {
        // Property that will store the selected duck ID from form submissions
        [BindProperty]
        public int SelectedDuckId { get; set; }
        // List that will hold all ducks for the dropdown selection
        public List<SelectListItem> DuckList { get; set; }
        
        // Property that will store the currently selected duck object
        public Duck SelectedDuck { get; set; }

        // Handles HTTP GET requests to the page - loads the list of ducks
        public void OnGet()
        {
            LoadDuckList();
        }
        // Handles HTTP POST requests (when user selects a duck) - loads the duck list
        // and retrieves the selected duck's details
        public IActionResult OnPost()
        {
            LoadDuckList();
            if (SelectedDuckId != 0)
            {
                SelectedDuck = GetDuckById(SelectedDuckId);
            }//end if
            return Page();
        }
        // Helper method that loads the list of ducks from the SQLite database
        // for displaying in a dropdown menu
        private void LoadDuckList()
        {
            //Create a new list to hold ducks
            DuckList = new List<SelectListItem>();
            //Create a databse connection
            using (var connection = new SqliteConnection("Data Source=RubberDucks.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Name FROM Ducks";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DuckList.Add(new SelectListItem
                        {
                            Value = reader.GetInt32(0).ToString(), // Duck ID as the value
                            Text = reader.GetString(1)             // Duck name as the display text
                        });
                    }
                }
            }
        }
        // Helper method that retrieves a specific duck by its ID from the database
        // Returns all details of the duck
        private Duck GetDuckById(int id)
        {
            using (var connection = new SqliteConnection("Data Source=RubberDucks.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Ducks WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id); // Using parameterized query for security
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
                    }//end if
                }//end using reader
            }//end using connection
            return null;
        }//end method
    }//end class
    // Simple model class representing a rubber duck product
    public class Duck
    {
        //Attributes for the duck class
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageFileName { get; set; }
    }//end duck class
}  // end namespace  





