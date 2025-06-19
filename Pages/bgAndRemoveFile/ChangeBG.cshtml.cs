using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace StudyPage.Pages.background
{
    public class ChangeBGModel : PageModel
    {
        public BGInfo BGInfo = new BGInfo();
        public void OnGet()
        {
           
        }

    
    }
}
