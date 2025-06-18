using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyWorkspace.Models;
using System.Data.SqlClient;

namespace StudyWorkspace.Pages.Workspaces
{
    public class CreateModel : PageModel
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _connectionString;

        public CreateModel(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        [BindProperty]
        public Workspace Workspace { get; set; }

        [BindProperty]
        public IFormFile BackgroundImageFile { get; set; }

        public IActionResult OnPost()
        {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                string imagePath = null;
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
                }

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Workspaces (WorkspaceName, BackgroundImage) VALUES (@Name, @BackgroundImage)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", Workspace.Name);
                        cmd.Parameters.AddWithValue("@BackgroundImage", (object)imagePath ?? DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }
                }

                return RedirectToPage("Index");
        }
    }
}