using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MCMS.Web.Models.Dtos
{
    public class FacultyCreateDto
    {
        // Required Faculty fields
        [Required]
        [Display(Name = "First Name")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Designation")]
        public string Post { get; set; }

        [Required]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Required]
        [Display(Name = "Joining Date")]
        [DataType(DataType.Date)]
        public DateTime JoiningDate { get; set; }

        [Display(Name = "Is Head of Department")]
        public bool IsHOD { get; set; }

        // Identity-related fields
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        // For dropdown in the view
        public IEnumerable<SelectListItem> Departments { get; set; }
    }
}
