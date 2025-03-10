using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCMS.Infrastructure.Models;

[Table("NAACMetrics")]
public partial class Naacmetric
{
    [Key]
    public int MetricId { get; set; }

    public int CriteriaId { get; set; }

    [StringLength(255)]
    public string MetricName { get; set; } = null!;

    public string? Description { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("CriteriaId")]
    [InverseProperty("Naacmetrics")]
    public virtual Naaccriterion Criteria { get; set; } = null!;

    [InverseProperty("Metric")]
    public virtual ICollection<Naacdatum> Naacdata { get; set; } = new List<Naacdatum>();
}
