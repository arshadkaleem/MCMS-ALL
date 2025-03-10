using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCMS.Infrastructure.Models;

public partial class FacultyBook
{
    [Key]
    public int BookId { get; set; }

    public int FacultyId { get; set; }

    [StringLength(255)]
    public string Title { get; set; } = null!;

    [Column("ISBN")]
    [StringLength(50)]
    public string? Isbn { get; set; }

    [StringLength(255)]
    public string Publisher { get; set; } = null!;

    public int PublishedYear { get; set; }

    [StringLength(255)]
    public string? BookUrl { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("FacultyId")]
    [InverseProperty("FacultyBooks")]
    public virtual Faculty Faculty { get; set; } = null!;
}
