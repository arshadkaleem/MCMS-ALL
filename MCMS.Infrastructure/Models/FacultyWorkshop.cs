using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCMS.Infrastructure.Models;

public partial class FacultyWorkshop
{
    [Key]
    public int WorkshopId { get; set; }

    public int FacultyId { get; set; }

    [StringLength(255)]
    public string Title { get; set; } = null!;

    [StringLength(255)]
    public string Organizer { get; set; } = null!;

    public DateOnly Date { get; set; }

    [StringLength(255)]
    public string Location { get; set; } = null!;

    [StringLength(100)]
    public string? Role { get; set; }

    [StringLength(255)]
    public string? CertificateUrl { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("FacultyId")]
    [InverseProperty("FacultyWorkshops")]
    public virtual Faculty Faculty { get; set; } = null!;
}
