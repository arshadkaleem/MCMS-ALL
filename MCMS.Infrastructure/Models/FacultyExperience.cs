using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCMS.Infrastructure.Models;

[Table("FacultyExperience")]
public partial class FacultyExperience
{
    [Key]
    public int ExperienceId { get; set; }

    public int FacultyId { get; set; }

    [StringLength(255)]
    public string JobTitle { get; set; } = null!;

    [StringLength(255)]
    public string Organization { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string? Description { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("FacultyId")]
    [InverseProperty("FacultyExperiences")]
    public virtual Faculty Faculty { get; set; } = null!;
}
