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
                string query = "SELECT * from Music";
                using (System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(query, connection))
                {
                    using (System.Data.SqlClient.SqlDataReader reader = command.ExecuteReader())
                    {
                        MusicTrack = new List<MusicTrack>();
                        while (reader.Read())
                        {
                            MusicTrack.Add(new MusicTrack
                            {
        MusicID = reader.GetInt32(reader.GetOrdinal("MusicID")),
        SongName = reader.IsDBNull(reader.GetOrdinal("SongName")) ? string.Empty : reader.GetString(reader.GetOrdinal("SongName")),
        MusicFile = reader.IsDBNull(reader.GetOrdinal("MusicFile")) ? string.Empty : reader.GetString(reader.GetOrdinal("MusicFile"))
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
