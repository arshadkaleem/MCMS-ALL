using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MCMS.Web.Models
{
    // MCMS.Web/Models/AssignHODViewModel.cs
    public class AssignHODViewModel
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        [Display(Name = "Head of Department")]
        [Required(ErrorMessage = "Please select a faculty member as HOD")]
        public int? SelectedFacultyId { get; set; }

        // List of eligible faculty members for the dropdown
        public IEnumerable<SelectListItem> FacultyList { get; set; }
    }
}
