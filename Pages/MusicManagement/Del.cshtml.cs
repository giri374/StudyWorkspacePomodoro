using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.IO;

namespace StudyPage.Pages.MusicManagement
{
    public class DelModel : PageModel
    {
            private readonly IConfiguration _configuration;
            
        private readonly IWebHostEnvironment _env;

        public DelModel(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        public IActionResult OnPost()
        {
            var id = Request.Form["id"];
            var action = Request.Form["action"];

            if (action == "delete" && !string.IsNullOrEmpty(id))
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                string musicFile = null;

                // 1. Get the file path from the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string selectSql = "SELECT MusicFile FROM music WHERE MusicID=@id";
                    using (SqlCommand selectCmd = new SqlCommand(selectSql, connection))
                    {
                        selectCmd.Parameters.AddWithValue("@id", id);
                        using (var reader = selectCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                musicFile = reader.GetString(0);
                            }
                        }
                    }
                }

                // 2. Delete the file from wwwroot if it exists
                if (!string.IsNullOrEmpty(musicFile))
                {
                    var filePath = Path.Combine(_env.WebRootPath, musicFile.Replace("/", Path.DirectorySeparatorChar.ToString()));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                // 3. Delete the record from the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM music WHERE MusicID=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }
            }

            // Redirect to Index in all cases
            return RedirectToPage("/MusicManagement/Index");
        }
    }
}
