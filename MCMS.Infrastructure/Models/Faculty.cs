using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCMS.Infrastructure.Models;

[Table("Faculty")]
[Index("DepartmentId", Name = "IX_Faculty_DepartmentId")]
[Index("Email", Name = "IX_Faculty_Email")]
[Index("Email", Name = "UQ__Faculty__A9D10534FD2C9F9C", IsUnique = true)]
public partial class Faculty
{
    [Key]
    public int FacultyId { get; set; }

    [StringLength(100)]
    public string FirstName { get; set; } = null!;

    [StringLength(100)]
    public string LastName { get; set; } = null!;

    [StringLength(150)]
    public string Email { get; set; } = null!;

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [StringLength(100)]
    public string Post { get; set; } = null!;

    public int DepartmentId { get; set; }

    [Column("CVUrl")]
    [StringLength(255)]
    public string? Cvurl { get; set; }

    [StringLength(255)]
    public string? ProfilePictureUrl { get; set; }

    [StringLength(500)]
    public string? Address { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    [StringLength(100)]
    public string? State { get; set; }

    [StringLength(20)]
    public string? ZipCode { get; set; }

    public DateOnly JoiningDate { get; set; }

    [Column("IsHOD")]
    public bool? IsHod { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [StringLength(450)]
    public string? UserId { get; set; }

    public bool IsDeleted { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeletedAt { get; set; }

    [ForeignKey("DepartmentId")]
    [InverseProperty("Faculties")]
    public virtual Department Department { get; set; } = null!;

    [InverseProperty("Faculty")]
    public virtual ICollection<FacultyAward> FacultyAwards { get; set; } = new List<FacultyAward>();

    [InverseProperty("Faculty")]
    public virtual ICollection<FacultyBook> FacultyBooks { get; set; } = new List<FacultyBook>();

    [InverseProperty("Faculty")]
    public virtual ICollection<FacultyEducation> FacultyEducations { get; set; } = new List<FacultyEducation>();

    [InverseProperty("Faculty")]
    public virtual ICollection<FacultyExperience> FacultyExperiences { get; set; } = new List<FacultyExperience>();

    [InverseProperty("Faculty")]
    public virtual ICollection<FacultyResearch> FacultyResearches { get; set; } = new List<FacultyResearch>();

    [InverseProperty("Faculty")]
    public virtual ICollection<FacultySubject> FacultySubjects { get; set; } = new List<FacultySubject>();

    [InverseProperty("Faculty")]
    public virtual ICollection<FacultyWorkshop> FacultyWorkshops { get; set; } = new List<FacultyWorkshop>();

    [InverseProperty("Faculty")]
    public virtual ICollection<Timetable> Timetables { get; set; } = new List<Timetable>();
}
