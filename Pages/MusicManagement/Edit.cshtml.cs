using System;
using System.Data.SqlClient;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace StudyPage.Pages.MusicManagement
{
    public class EditModel : PageModel
    {
            private readonly IConfiguration _configuration;
            
                public EditModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public MusicTrack musicTrack = new MusicTrack();
        public string errorMessage = "";

        public void OnGet()
        {
            string id = Request.Query["id"];
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM music WHERE MusicID = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                musicTrack.MusicID = reader.GetInt32(reader.GetOrdinal("MusicID"));
                                musicTrack.SongName = reader.IsDBNull(reader.GetOrdinal("SongName")) ? string.Empty : reader.GetString(reader.GetOrdinal("SongName"));
                                musicTrack.MusicFile = reader.IsDBNull(reader.GetOrdinal("MusicFile")) ? string.Empty : reader.GetString(reader.GetOrdinal("MusicFile"));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public void OnPost()
        {
            musicTrack.MusicID = int.Parse(Request.Form["MusicID"]);
            musicTrack.SongName = Request.Form["SongName"];
            string oldMusicFile = Request.Form["OldMusicFile"];
            IFormFile uploadedFile = Request.Form.Files["MusicFile"];

            // Kiểm tra tên bài hát
            if (string.IsNullOrEmpty(musicTrack.SongName))
            {
                errorMessage = "Tên bài hát không được để trống!";
                return;
            }

            // Xử lý file upload nếu có
            string newMusicFile = oldMusicFile;
            if (uploadedFile != null && uploadedFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "music");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(uploadedFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    uploadedFile.CopyTo(fileStream);
                }

                // Xóa file cũ nếu có
                if (!string.IsNullOrEmpty(oldMusicFile))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldMusicFile.Replace("/", Path.DirectorySeparatorChar.ToString()));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                newMusicFile = "uploads/music/" + uniqueFileName;
            }

            // Cập nhật vào database
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE music SET SongName=@name, MusicFile=@musicfile WHERE MusicID=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", musicTrack.SongName);
                        command.Parameters.AddWithValue("@musicfile", newMusicFile);
                        command.Parameters.AddWithValue("@id", musicTrack.MusicID);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            Response.Redirect("/MusicManagement/Index");
        }
    }

}
