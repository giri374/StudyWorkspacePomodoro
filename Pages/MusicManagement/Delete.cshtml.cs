using System;
using System.Data.SqlClient;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace StudyPage.Pages.MusicManagement
{
    public class DeleteModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        [BindProperty]
        public MusicTrack MusicTrack { get; set; }
        public string ErrorMessage { get; set; }

        public DeleteModel(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT MusicID, SongName, MusicFile FROM music WHERE MusicID = @id";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                MusicTrack = new MusicTrack
                                {
                                    MusicID = reader.GetInt32(reader.GetOrdinal("MusicID")),
                                    SongName = reader.GetString(reader.GetOrdinal("SongName")),
                                    MusicFile = reader.GetString(reader.GetOrdinal("MusicFile"))
                                };
                            }
                            else
                            {
                                return NotFound();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
                return Page();
            }

            return Page();
        }

        public IActionResult OnPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Step 1: Get the file path from the database before deleting the record.
                    string musicFilePath = null;
                    string sqlSelect = "SELECT MusicFile FROM music WHERE MusicID = @id";
                    using (var selectCommand = new SqlCommand(sqlSelect, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@id", id.Value);
                        var result = selectCommand.ExecuteScalar();
                        if (result != null)
                        {
                            musicFilePath = result.ToString();
                        }
                    }

                    if (string.IsNullOrEmpty(musicFilePath))
                    {
                        // If no file path is found, perhaps the record is already gone.
                        // Redirect to Index to avoid errors.
                        return RedirectToPage("./Index");
                    }


                    // Step 2: Delete the record from the database.
                    string sqlDelete = "DELETE FROM music WHERE MusicID = @id";
                    using (var deleteCommand = new SqlCommand(sqlDelete, connection))
                    {
                        deleteCommand.Parameters.AddWithValue("@id", id.Value);
                        deleteCommand.ExecuteNonQuery();
                    }

                    // Step 3: Delete the physical file from the wwwroot/uploads folder.
                    string fullPath = Path.Combine(_environment.WebRootPath, musicFilePath);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                 // In case of an error during post, log it and show the page again.
                 ErrorMessage = $"An error occurred during deletion: {ex.Message}";
                 // Re-populate the MusicTrack property to display details on the page.
                 OnGet(id);
                 return Page();
            }
        }
    }
}