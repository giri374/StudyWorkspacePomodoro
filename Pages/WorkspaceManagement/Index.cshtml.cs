using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace StudyPage.Pages.WorkspaceManagement
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        
        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public List<Workspace> Workspaces = new List<Workspace>();
        
        public void OnGet()
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new ArgumentException("Connection string 'DefaultConnection' not found");
                }
                
                using var connection = new SqlConnection(connectionString);
                connection.Open();
                string query = "SELECT * FROM Workspaces";
                
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Workspaces = new List<Workspace>();
                        while (reader.Read())
                        {
                            Workspaces.Add(new Workspace
                            {
                                WorkspaceID = reader.GetInt32(reader.GetOrdinal("WorkspaceID")),
                                WorkspaceName = reader.IsDBNull(reader.GetOrdinal("WorkspaceName")) ? string.Empty : reader.GetString(reader.GetOrdinal("WorkspaceName")),
                                BackgroundImage = reader.IsDBNull(reader.GetOrdinal("BackgroundImage")) ? string.Empty : reader.GetString(reader.GetOrdinal("BackgroundImage"))
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error, show a message to the user)
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}