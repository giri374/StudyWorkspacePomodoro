using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyWorkspace.Models;
using System.Data.SqlClient;

namespace StudyWorkspace.Pages.Workspaces
{
    public class DeleteModel : PageModel
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _connectionString;

        public DeleteModel(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [BindProperty]
        public Workspace Workspace { get; set; }

        public IActionResult OnGet(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "SELECT WorkspaceID, Name, BackgroundImage FROM Workspaces WHERE WorkspaceID = @Id";
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
                ModelState.AddModelError(string.Empty, $"Error loading workspace for deletion: {ex.Message}");
                return Page();
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM Workspaces WHERE WorkspaceID = @Id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", Workspace.WorkspaceID);
                        cmd.ExecuteNonQuery();
                    }
                }

                if (!string.IsNullOrEmpty(Workspace.BackgroundImage))
                {
                    string imagePath = Path.Combine(_env.WebRootPath, Workspace.BackgroundImage.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error deleting workspace: {ex.Message}");
                return Page();
            }

            return RedirectToPage("Index");
        }
    }
}