using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCMS.Infrastructure.Models;

[Index("RoomNumber", Name = "UQ__Classroo__AE10E07AA2470CB2", IsUnique = true)]
public partial class Classroom
{
    [Key]
    public int ClassroomId { get; set; }

    [Required(ErrorMessage = "Room Number is required")]
    [Display(Name = "Room Number")]
    [StringLength(50, ErrorMessage = "Room Number cannot exceed 50 characters")]
    public string RoomNumber { get; set; } = null!;

    [Required(ErrorMessage = "Capacity is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Capacity must be greater than 0")]
    public int Capacity { get; set; }

    [Required(ErrorMessage = "Room Type is required")]
    [Display(Name = "Room Type")]
    [StringLength(150, ErrorMessage = "Room Type cannot exceed 150 characters")]
    public string RoomType { get; set; } = null!;

    [Column(TypeName = "datetime")]
    [Display(Name = "Created At")]
    public DateTime? CreatedAt { get; set; }

    [StringLength(150, ErrorMessage = "Building name cannot exceed 150 characters")]
    public string? Building { get; set; }

    [Display(Name = "Facilities")]
    [StringLength(250, ErrorMessage = "Facilities description cannot exceed 250 characters")]
    public string? Facilities { get; set; }

    [InverseProperty("Classroom")]
    public virtual ICollection<Timetable> Timetables { get; set; } = new List<Timetable>();
}
