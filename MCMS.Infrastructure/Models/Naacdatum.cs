using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCMS.Infrastructure.Models;

[Table("NAACData")]
[Index("AcademicYear", Name = "IX_NAACData_AcademicYear")]
[Index("DepartmentId", "MetricId", Name = "IX_NAACData_DeptMetric")]
public partial class Naacdatum
{
    [Key]
    public int DataId { get; set; }

    public int MetricId { get; set; }

    public int? DepartmentId { get; set; }

    public int AcademicYear { get; set; }

    public string Value { get; set; } = null!;

    [StringLength(255)]
    public string? SupportingDocumentUrl { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    public bool IsDeleted { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeletedAt { get; set; }

    [ForeignKey("DepartmentId")]
    [InverseProperty("Naacdata")]
    public virtual Department? Department { get; set; }

    [ForeignKey("MetricId")]
    [InverseProperty("Naacdata")]
    public virtual Naacmetric Metric { get; set; } = null!;
}
