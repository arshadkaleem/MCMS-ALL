using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCMS.Infrastructure.Models;

[Table("NAACCriteria")]
public partial class Naaccriterion
{
    [Key]
    public int CriteriaId { get; set; }

    [StringLength(255)]
    public string CriteriaName { get; set; } = null!;

    public string? Description { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Criteria")]
    public virtual ICollection<Naacmetric> Naacmetrics { get; set; } = new List<Naacmetric>();
}
