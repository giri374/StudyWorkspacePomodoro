using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;

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
    public string SelectedWorkspaceImage { get; set; } = "/img/BACKGROUND.png";
    public string SelectedMusicFile { get; set; } = "/music/default.mp3";

    public List<SelectListItem> WorkspaceList { get; set; } = new();
    public List<SelectListItem> MusicList { get; set; } = new();

    public string TimerDisplay { get; set; }
    public int TimerInSeconds { get; set; }
    public int PomodoroDurationMin { get; set; } = 25;
    public int ShortBreakDurationMin { get; set; } = 5;
    public int LongBreakDurationMin { get; set; } = 15;
    public bool TimerRunning { get; set; } = false;
    public string Mode { get; set; } = "Pomodoro";

    private void LoadPomodoroSettings()
    {
        string connStr = _configuration.GetConnectionString("DefaultConnection");
        using var conn = new SqlConnection(connStr);
        conn.Open();
        var cmd = new SqlCommand("SELECT TOP 1 PomodoroDuration, ShortBreakDuration, LongBreakDuration FROM PomodoroSettings", conn);
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            PomodoroDurationMin = Convert.ToInt32(reader["PomodoroDuration"]);
            ShortBreakDurationMin = Convert.ToInt32(reader["ShortBreakDuration"]);
            LongBreakDurationMin = Convert.ToInt32(reader["LongBreakDuration"]);
        }
    }

    public void OnGet()
    {
        LoadOptions();
        LoadPomodoroSettings();
        LoadSelectedWorkspace();
        LoadSelectedMusic();
        TimerInSeconds = PomodoroDurationMin * 60;
        TimerDisplay = $"{PomodoroDurationMin:D2}:00";
        TimerRunning = false;
    }

    public void OnPost(string action)
    {
        LoadOptions();
        LoadPomodoroSettings();
        LoadSelectedWorkspace();
        LoadSelectedMusic();

        switch (action)
        {
            case "StartPomodoro":
                TimerInSeconds = PomodoroDurationMin * 60;
                TimerDisplay = $"{PomodoroDurationMin:D2}:00";
                Mode = "Pomodoro";
                TimerRunning = true;
                break;
            case "ShortBreak":
                TimerInSeconds = ShortBreakDurationMin * 60;
                TimerDisplay = $"{ShortBreakDurationMin:D2}:00";
                Mode = "Short Break";
                TimerRunning = true;
                break;
            case "LongBreak":
                TimerInSeconds = LongBreakDurationMin * 60;
                TimerDisplay = $"{LongBreakDurationMin:D2}:00";
                Mode = "Long Break";
                TimerRunning = true;
                break;
            case "Reset":
                TimerInSeconds = PomodoroDurationMin * 60;
                TimerDisplay = $"{PomodoroDurationMin:D2}:00";
                Mode = "Pomodoro";
                TimerRunning = false;
                break;
        }
    }

    private void LoadOptions()
    {
        string connStr = _configuration.GetConnectionString("DefaultConnection");
        using var conn = new SqlConnection(connStr);
        conn.Open();

        // Clear existing lists
        WorkspaceList.Clear();
        MusicList.Clear();

        // Add default option for workspace
        WorkspaceList.Add(new SelectListItem
        {
            Value = "0",
            Text = "Select Workspace"
        });

        // Load workspaces
        using (var cmd = new SqlCommand("SELECT WorkspaceID, WorkspaceName FROM Workspaces ORDER BY WorkspaceName", conn))
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                WorkspaceList.Add(new SelectListItem
                {
                    Value = reader["WorkspaceID"].ToString(),
                    Text = reader["WorkspaceName"].ToString(),
                    Selected = SelectedWorkspaceId.ToString() == reader["WorkspaceID"].ToString()
                });
            }
        }

        // Add default option for music
        MusicList.Add(new SelectListItem
        {
            Value = "0",
            Text = "Select Music"
        });

        // Load music
        using (var cmd = new SqlCommand("SELECT MusicID, SongName FROM Music ORDER BY SongName", conn))
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                MusicList.Add(new SelectListItem
                {
                    Value = reader["MusicID"].ToString(),
                    Text = reader["SongName"].ToString(),
                    Selected = SelectedMusicId.ToString() == reader["MusicID"].ToString()
                });
            }
        }
    }

    private void LoadSelectedWorkspace()
    {
        if (SelectedWorkspaceId == 0) return;
        
        string connStr = _configuration.GetConnectionString("DefaultConnection");
        using var conn = new SqlConnection(connStr);
        conn.Open();
        var cmd = new SqlCommand("SELECT WorkspaceName, BackgroundImage FROM Workspaces WHERE WorkspaceID = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", SelectedWorkspaceId);
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            SelectedWorkspaceName = reader["WorkspaceName"].ToString() ?? "Default Workspace";
            var image = reader["BackgroundImage"].ToString();
            if (!string.IsNullOrEmpty(image))
            {
                // Handle different path formats
                if (image.StartsWith("/") || image.StartsWith("~"))
                {
                    SelectedWorkspaceImage = image.Replace("~", "");
                }
                else
                {
                    SelectedWorkspaceImage = "/" + image.TrimStart('/');
                }
            }
        }
    }

    private void LoadSelectedMusic()
    {
        if (SelectedMusicId == 0) return;
        
        string connStr = _configuration.GetConnectionString("DefaultConnection");
        using var conn = new SqlConnection(connStr);
        conn.Open();
        var cmd = new SqlCommand("SELECT MusicFile FROM Music WHERE MusicID = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", SelectedMusicId);
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            var musicFile = reader["MusicFile"].ToString();
            if (!string.IsNullOrEmpty(musicFile))
            {
                // Handle different path formats
                if (musicFile.StartsWith("/") || musicFile.StartsWith("~"))
                {
                    SelectedMusicFile = musicFile.Replace("~", "");
                }
                else
                {
                    SelectedMusicFile = "/" + musicFile.TrimStart('/');
                }
            }
        }
    }
}