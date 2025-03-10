using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCMS.Infrastructure.Models;

[Table("Timetable")]
[Index("AcademicYear", "Semester", Name = "IX_Timetable_AcadYearSem")]
[Index("FacultyId", Name = "IX_Timetable_Faculty")]
public partial class Timetable
{
    [Key]
    public int TimetableId { get; set; }

    public int CourseId { get; set; }

    public int SubjectId { get; set; }

    public int FacultyId { get; set; }

    public int SlotId { get; set; }

    public int ClassroomId { get; set; }

    public int AcademicYear { get; set; }

    [StringLength(50)]
    public string Semester { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    public bool IsDeleted { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeletedAt { get; set; }

    [ForeignKey("ClassroomId")]
    [InverseProperty("Timetables")]
    public virtual Classroom Classroom { get; set; } = null!;

    [ForeignKey("CourseId")]
    [InverseProperty("Timetables")]
    public virtual Course Course { get; set; } = null!;

    [ForeignKey("FacultyId")]
    [InverseProperty("Timetables")]
    public virtual Faculty Faculty { get; set; } = null!;

    [ForeignKey("SlotId")]
    [InverseProperty("Timetables")]
    public virtual TimetableSlot Slot { get; set; } = null!;

    [ForeignKey("SubjectId")]
    [InverseProperty("Timetables")]
    public virtual Subject Subject { get; set; } = null!;
}
