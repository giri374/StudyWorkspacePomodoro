using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace StudyWorkspace.Pages;

public class FocusModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public FocusModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {

    }
}