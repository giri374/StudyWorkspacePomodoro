using System;
using System.ComponentModel.DataAnnotations;

namespace StudyWorkspace.Models
{
    public class Workspace
    {
        public int WorkspaceID { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên workspace")]
        public string Name { get; set; }
        public string BackgroundImage { get; set; }
    }
}