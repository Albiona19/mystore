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

        // Metoda OnGet p�r ngarkimin e t� dh�nave p�r klientin
        public void OnGet()
        {
            string id = Request.Query["id"];  // Merrni ID-n� nga query string

            // Kontrolloni n�se ID �sht� e vlefshme
            if (string.IsNullOrEmpty(id))
            {
                errorMessage = "Client ID is missing from the query string.";  // N�se ID-ja �sht� e mang�t
                return;
            }

            if (!int.TryParse(id, out int parsedId))  // Kontrolloni n�se ID �sht� num�r i vlefsh�m
            {
                errorMessage = "Invalid client ID";  // N�se ID-ja nuk �sht� num�r
                return;
            }

            try
            {
                string connectionString = "Data Source=DESKTOP-B5BM191;Initial Catalog=store-clients;Integrated Security=True;Encrypt=False";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM clients WHERE id=@id";  // SQL p�r t� marr� klientin nga DB
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
                                errorMessage = "Client not found";  // N�se nuk ka klient me at� ID
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;  // P�rgjigjuni me mesazhin e gabimit
            }
        }

        // Metoda OnPost p�r p�rdit�simin e t� dh�nave
        public void OnPost()
        {
            clientInfo.id = Request.Form["id"];
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];

            // Kontrolloni n�se t� gjitha fushat jan� t� mbushura
            if (string.IsNullOrEmpty(clientInfo.id) || string.IsNullOrEmpty(clientInfo.name) || string.IsNullOrEmpty(clientInfo.email) ||
                string.IsNullOrEmpty(clientInfo.phone) || string.IsNullOrEmpty(clientInfo.address))
            {
                errorMessage = "All the fields are required";  // N�se ka fusha bosh
                return;
            }

            // Kontrolloni n�se ID �sht� num�r i vlefsh�m
            if (!int.TryParse(clientInfo.id, out int parsedId))
            {
                errorMessage = "Invalid client ID";  // N�se ID-ja nuk �sht� num�r i vlefsh�m
                return;
            }

            try
            {
                string connectionString = "Data Source=DESKTOP-B5BM191;Initial Catalog=store-clients;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE clients SET name = @name, email = @email, phone = @phone, address = @address WHERE id = @id";  // SQL p�r p�rdit�sim

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);
                        command.Parameters.Add("@id", SqlDbType.Int).Value = parsedId;  // Kaloni ID-n� si num�r

                        command.ExecuteNonQuery();  // Ekzekuto p�rdit�simin
                    }
                }

                successMessage = "Client updated successfully";  // Mesazh p�r sukses
                Response.Redirect("/Clients/Index");  // Ridrejtoni pas p�rdit�simit
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;  // P�rgjigjuni me mesazhin e gabimit
            }
        }
    }
}
