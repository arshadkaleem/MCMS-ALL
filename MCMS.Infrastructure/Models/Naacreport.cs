using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCMS.Infrastructure.Models;

[Table("NAACReports")]
public partial class Naacreport
{
    [Key]
    public int ReportId { get; set; }

    [StringLength(50)]
    public string ReportType { get; set; } = null!;

    public int AcademicYear { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? GeneratedDate { get; set; }

    [StringLength(255)]
    public string? ReportFileUrl { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }
}
