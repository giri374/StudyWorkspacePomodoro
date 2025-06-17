using System;
using System.Data.SqlClient;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace StudyPage.Pages.Music
{
    public class EditModel : PageModel
    {
        public MusicTrack musicTrack = new MusicTrack();
        public string errorMessage = "";

        public void OnGet()
        {
            string id = Request.Query["id"];
            try
            {
                string connectionString = "Data Source=localhost\\sqlexpress;Initial Catalog=music;Integrated Security=True;Pooling=False;TrustServerCertificate=True";
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
                                musicTrack.MusicID = reader.GetInt32(0);
                                musicTrack.Name = reader.GetString(1);
                                musicTrack.MusicFile = reader.GetString(2);
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
            musicTrack.Name = Request.Form["Name"];
            string oldMusicFile = Request.Form["OldMusicFile"];
            IFormFile uploadedFile = Request.Form.Files["MusicFile"];

            // Kiểm tra tên bài hát
            if (string.IsNullOrEmpty(musicTrack.Name))
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
                string connectionString = "Data Source=localhost\\sqlexpress;Initial Catalog=music;Integrated Security=True;Pooling=False;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE music SET Name=@name, MusicFile=@musicfile WHERE MusicID=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", musicTrack.Name);
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
            Response.Redirect("/Music/Index");
        }
    }

}
