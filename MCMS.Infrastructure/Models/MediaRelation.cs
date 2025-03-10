using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCMS.Infrastructure.Models;

public partial class MediaRelation
{
    [Key]
    public int RelationId { get; set; }

    public int MediaId { get; set; }

    [StringLength(50)]
    public string RelatedEntity { get; set; } = null!;

    public int RelatedId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("MediaId")]
    [InverseProperty("MediaRelations")]
    public virtual MediaFile Media { get; set; } = null!;
}
