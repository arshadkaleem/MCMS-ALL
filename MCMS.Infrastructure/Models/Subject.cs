using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCMS.Infrastructure.Models;

[Index("CourseId", Name = "IX_Subjects_CourseId")]
[Index("DepartmentId", "Semester", Name = "IX_Subjects_DeptSem")]
[Index("SubjectName", Name = "IX_Subjects_Name")]
[Index("Slug", Name = "UQ__Subjects__BC7B5FB653E84ECE", IsUnique = true)]
public partial class Subject
{
    [Key]
    public int SubjectId { get; set; }

    [StringLength(255)]
    public string SubjectName { get; set; } = null!;

    [StringLength(255)]
    public string Slug { get; set; } = null!;

    public int CourseId { get; set; }

    public int DepartmentId { get; set; }

    public int Credits { get; set; }

    [StringLength(50)]
    public string Semester { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeletedAt { get; set; }

    [ForeignKey("CourseId")]
    [InverseProperty("Subjects")]
    public virtual Course Course { get; set; } = null!;

    [ForeignKey("DepartmentId")]
    [InverseProperty("Subjects")]
    public virtual Department Department { get; set; } = null!;

    [InverseProperty("Subject")]
    public virtual ICollection<FacultySubject> FacultySubjects { get; set; } = new List<FacultySubject>();

    [InverseProperty("Subject")]
    public virtual ICollection<Timetable> Timetables { get; set; } = new List<Timetable>();
}
