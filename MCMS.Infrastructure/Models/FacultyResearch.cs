using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCMS.Infrastructure.Models;

[Table("FacultyResearch")]
public partial class FacultyResearch
{
    [Key]
    public int ResearchId { get; set; }

    public int FacultyId { get; set; }

    [StringLength(255)]
    public string Title { get; set; } = null!;

    [StringLength(255)]
    public string JournalName { get; set; } = null!;

    [Column("ISSN")]
    [StringLength(50)]
    public string? Issn { get; set; }

    public int PublishedYear { get; set; }

    [Column("DOI")]
    [StringLength(100)]
    public string? Doi { get; set; }

    [StringLength(255)]
    public string? ResearchUrl { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("FacultyId")]
    [InverseProperty("FacultyResearches")]
    public virtual Faculty Faculty { get; set; } = null!;
}
