using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCMS.Infrastructure.Models;

[Index("AcademicYear", "Semester", Name = "IX_FacultySubjects_AcadYear")]
public partial class FacultySubject
{
    [Key]
    public int FacultySubjectId { get; set; }

    public int FacultyId { get; set; }

    public int SubjectId { get; set; }

    public int AcademicYear { get; set; }

    [StringLength(50)]
    public string Semester { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    public bool IsDeleted { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeletedAt { get; set; }

    [ForeignKey("FacultyId")]
    [InverseProperty("FacultySubjects")]
    public virtual Faculty Faculty { get; set; } = null!;

    [ForeignKey("SubjectId")]
    [InverseProperty("FacultySubjects")]
    public virtual Subject Subject { get; set; } = null!;
}
