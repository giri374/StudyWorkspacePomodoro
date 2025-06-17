using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace StudyPage.Pages.Music
{
    public class IndexModel : PageModel
    {
        public List<MusicTrack> MusicTrack = new List<MusicTrack>();
        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source= localhost\\sqlexpress; Initial Catalog=music;" + "Integrated Security=True; Pooling=False;TrustServerCertificate=True";
                using (Microsoft.Data.SqlClient.SqlConnection connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString)) 
                {
                    connection.Open();
                    string query = "SELECT * from music";
                    using (Microsoft.Data.SqlClient.SqlCommand command = new Microsoft.Data.SqlClient.SqlCommand(query, connection)) 
                    {
                        using (Microsoft.Data.SqlClient.SqlDataReader reader = command.ExecuteReader()) 
                        {
                            MusicTrack = new List<MusicTrack>();
                            while (reader.Read())
                            {
                                MusicTrack.Add(new MusicTrack
                                {
                                    MusicID = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    MusicFile = reader.GetString(2)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error, show a message to the user)
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
