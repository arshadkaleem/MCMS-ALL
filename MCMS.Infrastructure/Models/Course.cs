using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCMS.Infrastructure.Models;

[Index("CollegeType", Name = "IX_Courses_CollegeType")]
[Index("DepartmentId", Name = "IX_Courses_DepartmentId")]
[Index("Slug", Name = "UQ__Courses__BC7B5FB6C0D40108", IsUnique = true)]
public partial class Course
{
    [Key]
    public int CourseId { get; set; }

    [StringLength(150)]
    public string CourseName { get; set; } = null!;

    [StringLength(150)]
    public string Slug { get; set; } = null!;

    public int DepartmentId { get; set; }

    [StringLength(20)]
    public string CollegeType { get; set; } = null!;

    public int DurationYears { get; set; }

    public int? Credits { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeletedAt { get; set; }

    [ForeignKey("DepartmentId")]
    [InverseProperty("Courses")]
    public virtual Department Department { get; set; } = null!;

    [InverseProperty("Course")]
    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();

    [InverseProperty("Course")]
    public virtual ICollection<Timetable> Timetables { get; set; } = new List<Timetable>();
}
