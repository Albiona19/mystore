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
            // Nevojitet vet�m p�r inicializimin e form�s, nuk ka nevoj� t� b�jm� asgj� k�tu.
        }

        public void OnPost()
        {
            // Merrni t� dh�nat nga formulari
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];

            // Kontrolloni n�se t� gjitha fushat jan� plot�suar
            if (string.IsNullOrEmpty(clientInfo.name) || string.IsNullOrEmpty(clientInfo.email) ||
                string.IsNullOrEmpty(clientInfo.phone) || string.IsNullOrEmpty(clientInfo.address))
            {
                errorMessage = "All the fields are required";
                return;
            }

            try
            {
                // Krijo lidhjen me baz�n e t� dh�nave
                string connectionString = "Data Source=DESKTOP-B5BM191;Initial Catalog=store-clients;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL p�r t� shtuar klientin n� tabel�
                    string sql = "INSERT INTO clients (name, email, phone, address) VALUES (@name, @email, @phone, @address)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // Shto parametrat p�r vlerat e fushave
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);

                        // Ekzekuto komanden p�r t� shtuar t� dh�nat
                        command.ExecuteNonQuery();
                    }
                }

                // Pas ekzekutimit t� suksessh�m, pastrojm� fushat dhe shfaqim nj� mesazh suksesi
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
                // N�se ka ndodhur nj� gabim, shfaqim mesazhin e gabimit
                errorMessage = "An error occurred while adding the client: " + ex.Message;
            }
        }
    }
}
