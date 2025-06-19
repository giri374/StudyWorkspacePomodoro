using System;
using System.Data.SqlClient;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace StudyPage.Pages.WorkspaceManagement
{
    public class DeleteModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        [BindProperty]
        public Workspace Workspace { get; set; }
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
                    string sql = "SELECT WorkspaceID, WorkspaceName, BackgroundImage FROM Workspaces WHERE WorkspaceID = @id";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Workspace = new Workspace
                                {
                                    WorkspaceID = reader.GetInt32(reader.GetOrdinal("WorkspaceID")),
                                    WorkspaceName = reader.GetString(reader.GetOrdinal("WorkspaceName")),
                                    BackgroundImage = reader.IsDBNull(reader.GetOrdinal("BackgroundImage")) ? string.Empty : reader.GetString(reader.GetOrdinal("BackgroundImage"))
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
                    string backgroundImagePath = null;
                    string sqlSelect = "SELECT BackgroundImage FROM Workspaces WHERE WorkspaceID = @id";
                    using (var selectCommand = new SqlCommand(sqlSelect, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@id", id.Value);
                        var result = selectCommand.ExecuteScalar();
                        if (result != null)
                        {
                            backgroundImagePath = result.ToString();
                        }
                    }

                    // Step 2: Delete the record from the database.
                    string sqlDelete = "DELETE FROM Workspaces WHERE WorkspaceID = @id";
                    using (var deleteCommand = new SqlCommand(sqlDelete, connection))
                    {
                        deleteCommand.Parameters.AddWithValue("@id", id.Value);
                        deleteCommand.ExecuteNonQuery();
                    }

                    // Step 3: Delete the physical file from the wwwroot/uploads folder.
                    if (!string.IsNullOrEmpty(backgroundImagePath))
                    {
                        string fullPath = Path.Combine(_environment.WebRootPath, backgroundImagePath);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }
                }

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                // In case of an error during post, log it and show the page again.
                ErrorMessage = $"An error occurred during deletion: {ex.Message}";
                // Re-populate the Workspace property to display details on the page.
                OnGet(id);
                return Page();
            }
        }
    }
}