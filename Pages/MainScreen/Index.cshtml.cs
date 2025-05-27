using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient ;

public class MainScreenModel : PageModel
{
    private readonly IConfiguration _configuration;

    public MainScreenModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [BindProperty] public int SelectedWorkspaceId { get; set; }
    [BindProperty] public int SelectedMusicId { get; set; }
    public string SelectedWorkspaceName { get; set; } = "Default Workspace";
    public string SelectedWorkspaceImage { get; set; } = "/images/default.jpg";

    public List<SelectListItem> WorkspaceList { get; set; } = new();
    public List<SelectListItem> MusicList { get; set; } = new();

    public string TimerDisplay { get; set; }
    public int TimerInSeconds { get; set; }// default
    public int PomodoroDurationMin { get; set; } = 25;
    public int ShortBreakDurationMin { get; set; } = 5;
    public string Mode { get; set; } = "Pomodoro";

private void LoadPomodoroSettings()
{
    string connStr = _configuration.GetConnectionString("DefaultConnection");

    using var conn = new SqlConnection(connStr);
    conn.Open();

    var cmd = new SqlCommand("SELECT TOP 1 PomodoroDuration, ShortBreakDuration FROM PomodoroSettings", conn);

    using var reader = cmd.ExecuteReader();
    if (reader.Read())
    {
        PomodoroDurationMin = Convert.ToInt32(reader["PomodoroDuration"]);
        ShortBreakDurationMin = Convert.ToInt32(reader["ShortBreakDuration"]);
    }
}
    public void OnGet()
    {
        LoadOptions();
        LoadPomodoroSettings();

        TimerInSeconds = PomodoroDurationMin * 60;
        TimerDisplay = $"{PomodoroDurationMin:D2}:00";
    }


    public void OnPost(string action)
    {
        LoadOptions();
        LoadPomodoroSettings();

    switch (action)
    {
        case "StartPomodoro":
            TimerInSeconds = PomodoroDurationMin * 60;
            TimerDisplay = $"{PomodoroDurationMin:D2}:00";
            Mode = "Pomodoro";
            break;
        case "ShortBreak":
            TimerInSeconds = ShortBreakDurationMin * 60;
            TimerDisplay = $"{ShortBreakDurationMin:D2}:00";
            Mode = "Short Break";
            break;
    }

        LoadSelectedWorkspace();
    }

    private void LoadOptions()
    {
        string connStr = _configuration.GetConnectionString("DefaultConnection");

        using var conn = new SqlConnection(connStr);
        conn.Open();

        // Load workspaces
        using (var cmd = new SqlCommand("SELECT WorkspaceID, WorkspaceName FROM Workspaces", conn))
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                WorkspaceList.Add(new SelectListItem
                {
                    Value = reader["WorkspaceID"].ToString(),
                    Text = reader["WorkspaceName"].ToString()
                });
            }
        }

        // Load music
        using (var cmd = new SqlCommand("SELECT MusicID, SongName FROM Music", conn))
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                MusicList.Add(new SelectListItem
                {
                    Value = reader["MusicID"].ToString(),
                    Text = reader["SongName"].ToString()
                });
            }
        }
    }

    private void LoadSelectedWorkspace()
    {
        string connStr = _configuration.GetConnectionString("DefaultConnection");

        using var conn = new SqlConnection(connStr);
        conn.Open();

        var cmd = new SqlCommand("SELECT WorkspaceName, BackgroundImage FROM Workspaces WHERE WorkspaceID = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", SelectedWorkspaceId);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            SelectedWorkspaceName = reader["WorkspaceName"].ToString();
            SelectedWorkspaceImage = reader["BackgroundImage"].ToString();
        }
    }
}
