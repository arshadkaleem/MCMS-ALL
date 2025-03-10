using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCMS.Infrastructure.Models;

[Index("TagName", Name = "UQ__MediaTag__BDE0FD1DB07F600D", IsUnique = true)]
public partial class MediaTag
{
    [Key]
    public int TagId { get; set; }

    [StringLength(100)]
    public string TagName { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Tag")]
    public virtual ICollection<MediaFileTag> MediaFileTags { get; set; } = new List<MediaFileTag>();
}
