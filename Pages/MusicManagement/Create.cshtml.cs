using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Data.SqlClient;

namespace StudyPage.Pages.MusicManagement
{

    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;

        private readonly IWebHostEnvironment Environment;
        public string Message { get; set; }

        [BindProperty]
        public string Name { get; set; }

        public CreateModel(IConfiguration configuration, IWebHostEnvironment _environment)
        {
            Environment = _environment;
            _configuration = configuration;
        }

        public void OnGet()
        {
        }

        public void OnPostUpload(List<IFormFile> postedFiles)
        {
            string uploadsFolder = Path.Combine(Environment.WebRootPath, "uploads", "music");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            foreach (IFormFile postedFile in postedFiles)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(postedFile.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                // Save info to SQL Server
                try
                {
                    string connectionString = _configuration.GetConnectionString("DefaultConnection");
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sql = "INSERT INTO music (SongName, MusicFile) VALUES (@Name, @MusicFile);";
                        using (var command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@Name", Name ?? Path.GetFileNameWithoutExtension(postedFile.FileName));
                            command.Parameters.AddWithValue("@MusicFile", "uploads/music/" + fileName);
                            command.ExecuteNonQuery();
                        }
                    }
                    Message += $"<b>{fileName}</b> uploaded and info saved.<br />";
                }
                catch (Exception ex)
                {
                    Message += $"Error saving info for {fileName}: {ex.Message}<br />";
                }
            }
        }
    }

}
