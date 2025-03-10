using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCMS.Infrastructure.Models;

[Table("FacultyEducation")]
public partial class FacultyEducation
{
    [Key]
    public int EducationId { get; set; }

    public int FacultyId { get; set; }

    [StringLength(255)]
    public string Degree { get; set; } = null!;

    [StringLength(255)]
    public string FieldOfStudy { get; set; } = null!;

    [StringLength(255)]
    public string Institution { get; set; } = null!;

    public int YearOfCompletion { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("FacultyId")]
    [InverseProperty("FacultyEducations")]
    public virtual Faculty Faculty { get; set; } = null!;
}
