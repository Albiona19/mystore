using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Managment_Storee.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
            // Nevojitet vetëm për inicializimin e formës, nuk ka nevojë të bëjmë asgjë këtu.
        }

        public void OnPost()
        {
            // Merrni të dhënat nga formulari
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];

            // Kontrolloni nëse të gjitha fushat janë plotësuar
            if (string.IsNullOrEmpty(clientInfo.name) || string.IsNullOrEmpty(clientInfo.email) ||
                string.IsNullOrEmpty(clientInfo.phone) || string.IsNullOrEmpty(clientInfo.address))
            {
                errorMessage = "All the fields are required";
                return;
            }

            try
            {
                // Krijo lidhjen me bazën e të dhënave
                string connectionString = "Data Source=DESKTOP-B5BM191;Initial Catalog=store-clients;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL për të shtuar klientin në tabelë
                    string sql = "INSERT INTO clients (name, email, phone, address) VALUES (@name, @email, @phone, @address)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // Shto parametrat për vlerat e fushave
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);

                        // Ekzekuto komanden për të shtuar të dhënat
                        command.ExecuteNonQuery();
                    }
                }

                // Pas ekzekutimit të suksesshëm, pastrojmë fushat dhe shfaqim një mesazh suksesi
                clientInfo.name = "";
                clientInfo.email = "";
                clientInfo.phone = "";
                clientInfo.address = "";

                successMessage = "New Client Added Successfully!";

                // Redirekto pas suksesit
                Response.Redirect("/Clients/Index");
            }
            catch (Exception ex)
            {
                // Nëse ka ndodhur një gabim, shfaqim mesazhin e gabimit
                errorMessage = "An error occurred while adding the client: " + ex.Message;
            }
        }
    }
}
