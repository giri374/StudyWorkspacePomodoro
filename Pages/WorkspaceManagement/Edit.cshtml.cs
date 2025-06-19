using System;
using System.Data.SqlClient;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace StudyPage.Pages.WorkspaceManagement
{
    public class EditModel : PageModel
    {
        private readonly IConfiguration _configuration;
        
        public EditModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public Workspace workspace = new Workspace();
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
                    string sql = "SELECT * FROM Workspaces WHERE WorkspaceID = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                workspace.WorkspaceID = reader.GetInt32(reader.GetOrdinal("WorkspaceID"));
                                workspace.WorkspaceName = reader.IsDBNull(reader.GetOrdinal("WorkspaceName")) ? string.Empty : reader.GetString(reader.GetOrdinal("WorkspaceName"));
                                workspace.BackgroundImage = reader.IsDBNull(reader.GetOrdinal("BackgroundImage")) ? string.Empty : reader.GetString(reader.GetOrdinal("BackgroundImage"));
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
            workspace.WorkspaceID = int.Parse(Request.Form["WorkspaceID"]);
            workspace.WorkspaceName = Request.Form["WorkspaceName"];
            string oldBackgroundImage = Request.Form["OldBackgroundImage"];
            IFormFile uploadedFile = Request.Form.Files["BackgroundImage"];

            // Kiểm tra tên workspace
            if (string.IsNullOrEmpty(workspace.WorkspaceName))
            {
                errorMessage = "Tên workspace không được để trống!";
                return;
            }

            // Xử lý file upload nếu có
            string newBackgroundImage = oldBackgroundImage;
            if (uploadedFile != null && uploadedFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "backgrounds");
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
                if (!string.IsNullOrEmpty(oldBackgroundImage))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldBackgroundImage.Replace("/", Path.DirectorySeparatorChar.ToString()));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                newBackgroundImage = "uploads/backgrounds/" + uniqueFileName;
            }

            // Cập nhật vào database
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE Workspaces SET WorkspaceName=@name, BackgroundImage=@backgroundImage WHERE WorkspaceID=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", workspace.WorkspaceName);
                        command.Parameters.AddWithValue("@backgroundImage", newBackgroundImage);
                        command.Parameters.AddWithValue("@id", workspace.WorkspaceID);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            Response.Redirect("/WorkspaceManagement/Index");
        }
    }
}