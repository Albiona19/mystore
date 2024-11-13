using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;

namespace Managment_Storee.Pages.Clients
{
    public class EditModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public string errorMessage = "";
        public string successMessage = "";

        // Metoda OnGet për ngarkimin e të dhënave për klientin
        public void OnGet()
        {
            string id = Request.Query["id"];  // Merrni ID-në nga query string

            // Kontrolloni nëse ID është e vlefshme
            if (string.IsNullOrEmpty(id))
            {
                errorMessage = "Client ID is missing from the query string.";  // Nëse ID-ja është e mangët
                return;
            }

            if (!int.TryParse(id, out int parsedId))  // Kontrolloni nëse ID është numër i vlefshëm
            {
                errorMessage = "Invalid client ID";  // Nëse ID-ja nuk është numër
                return;
            }

            try
            {
                string connectionString = "Data Source=DESKTOP-B5BM191;Initial Catalog=store-clients;Integrated Security=True;Encrypt=False";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM clients WHERE id=@id";  // SQL për të marrë klientin nga DB
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.Add("@id", SqlDbType.Int).Value = parsedId;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                clientInfo.id = reader.GetInt32(0).ToString();
                                clientInfo.name = reader.GetString(1);
                                clientInfo.email = reader.GetString(2);
                                clientInfo.phone = reader.GetString(3);
                                clientInfo.address = reader.GetString(4);
                            }
                            else
                            {
                                errorMessage = "Client not found";  // Nëse nuk ka klient me atë ID
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;  // Përgjigjuni me mesazhin e gabimit
            }
        }

        // Metoda OnPost për përditësimin e të dhënave
        public void OnPost()
        {
            clientInfo.id = Request.Form["id"];
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];

            // Kontrolloni nëse të gjitha fushat janë të mbushura
            if (string.IsNullOrEmpty(clientInfo.id) || string.IsNullOrEmpty(clientInfo.name) || string.IsNullOrEmpty(clientInfo.email) ||
                string.IsNullOrEmpty(clientInfo.phone) || string.IsNullOrEmpty(clientInfo.address))
            {
                errorMessage = "All the fields are required";  // Nëse ka fusha bosh
                return;
            }

            // Kontrolloni nëse ID është numër i vlefshëm
            if (!int.TryParse(clientInfo.id, out int parsedId))
            {
                errorMessage = "Invalid client ID";  // Nëse ID-ja nuk është numër i vlefshëm
                return;
            }

            try
            {
                string connectionString = "Data Source=DESKTOP-B5BM191;Initial Catalog=store-clients;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE clients SET name = @name, email = @email, phone = @phone, address = @address WHERE id = @id";  // SQL për përditësim

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);
                        command.Parameters.Add("@id", SqlDbType.Int).Value = parsedId;  // Kaloni ID-në si numër

                        command.ExecuteNonQuery();  // Ekzekuto përditësimin
                    }
                }

                successMessage = "Client updated successfully";  // Mesazh për sukses
                Response.Redirect("/Clients/Index");  // Ridrejtoni pas përditësimit
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;  // Përgjigjuni me mesazhin e gabimit
            }
        }
    }
}
