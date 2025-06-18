using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyWorkspace.Models;
using System.Data.SqlClient;
namespace StudyWorkspace.Pages.Workspaces
{
    public class IndexModel : PageModel
    {
        private readonly string _connectionString;

        public IndexModel(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Workspace> Workspaces { get; set; }

        public void OnGet()
        {
            Workspaces = new List<Workspace>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "SELECT WorkspaceID, WorkspaceName, BackgroundImage FROM Workspaces";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Workspaces.Add(new Workspace
                                {
                                    WorkspaceID = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    BackgroundImage = reader.IsDBNull(2) ? null : reader.GetString(2)
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log the exception or display a user-friendly message
                ModelState.AddModelError(string.Empty, $"Database connection failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Catch any other unexpected errors
                ModelState.AddModelError(string.Empty, $"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}