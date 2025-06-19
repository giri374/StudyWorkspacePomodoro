using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyWorkspace.Models;
using System.Data.SqlClient;

namespace StudyWorkspace.Pages.Workspaces
{
    public class EditModel : PageModel
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _connectionString;

        public EditModel(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [BindProperty]
        public Workspace Workspace { get; set; }

        [BindProperty]
        public IFormFile BackgroundImageFile { get; set; }

        public IActionResult OnGet(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "SELECT WorkspaceID, WorkspaceName, BackgroundImage FROM Workspaces WHERE WorkspaceID = @Id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Workspace = new Workspace
                                {
                                    WorkspaceID = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    BackgroundImage = reader.IsDBNull(2) ? null : reader.GetString(2)
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
                ModelState.AddModelError(string.Empty, $"Error loading workspace: {ex.Message}");
                return Page();
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string imagePath = Workspace.BackgroundImage;
            if (BackgroundImageFile != null)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "workspaces");
                Directory.CreateDirectory(uploadsFolder);
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + BackgroundImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    BackgroundImageFile.CopyTo(fileStream);
                }
                imagePath = "/uploads/workspaces/" + uniqueFileName;

                // Optionally delete old image if it exists
                if (!string.IsNullOrEmpty(Workspace.BackgroundImage))
                {
                    string oldImagePath = Path.Combine(_env.WebRootPath, Workspace.BackgroundImage.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Workspaces SET WorkspaceName = @Name, BackgroundImage = @BackgroundImage WHERE WorkspaceID = @Id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", Workspace.Name);
                        cmd.Parameters.AddWithValue("@BackgroundImage", (object)imagePath ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Id", Workspace.WorkspaceID);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error updating workspace: {ex.Message}");
                return Page();
            }

            return RedirectToPage("Index");
        }
    }
}