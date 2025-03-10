using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCMS.Infrastructure.Models;

public partial class FacultyAward
{
    [Key]
    public int AwardId { get; set; }

    public int FacultyId { get; set; }

    [StringLength(255)]
    public string AwardTitle { get; set; } = null!;

    [StringLength(255)]
    public string AwardingBody { get; set; } = null!;

    public int AwardYear { get; set; }

    public string? Description { get; set; }

    [StringLength(255)]
    public string? CertificateUrl { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("FacultyId")]
    [InverseProperty("FacultyAwards")]
    public virtual Faculty Faculty { get; set; } = null!;
}
