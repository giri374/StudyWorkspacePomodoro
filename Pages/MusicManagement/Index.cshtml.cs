using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
// using Microsoft.Data.SqlClient;

namespace StudyPage.Pages.MusicManagement
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<MusicTrack> MusicTrack = new List<MusicTrack>();
        public void OnGet()
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new ArgumentException("Connection string 'DefaultConnection' not found");
                }
                using var connection = new SqlConnection(connectionString);

                connection.Open();
                string query = "SELECT * from music";
                using (System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(query, connection))
                {
                    using (System.Data.SqlClient.SqlDataReader reader = command.ExecuteReader())
                    {
                        MusicTrack = new List<MusicTrack>();
                        while (reader.Read())
                        {
                            MusicTrack.Add(new MusicTrack
                            {
                                MusicID = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                MusicFile = reader.IsDBNull(2) ? string.Empty : reader.GetString(2)
                            });
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
