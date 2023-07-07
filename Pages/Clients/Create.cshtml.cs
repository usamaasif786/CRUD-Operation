using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyStore.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public String errorMessage = ""; // Global variable that shows the Error message 
        public String successMessage = ""; // Global variable that shows the Success message
		public void OnGet()
        {
        }

        // In this method we can read the data from (form data)
        public void OnPost() 
        { 
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];

            // If any field is empty than it shows the error message
            if (clientInfo.name.Length == 0 || clientInfo.email.Length == 0 || clientInfo.phone.Length == 0 || clientInfo.address.Length == 0) 
            {
                errorMessage = "All fields are required";
                return;
            }

            // Otherwise it store the client data in the database
            try
            {   
                // This is the connection string
				String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True";
				// This is for SQL connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // For open the connection
                    connection.Open();
                    string sql = "INSERT INTO clients" + "(name, email, phone, address) VALUES" + "(@name, @email, @phone, @address);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);

                        command.ExecuteNonQuery();
                    }
                }

			}
            catch(Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            clientInfo.name = ""; clientInfo.email = ""; clientInfo.phone = ""; clientInfo.address = "";
            successMessage = "New client added successfully";

            // Than it redirect to the Index (Main) Page;
            Response.Redirect("/Clients/Index");
        }
    }
}
