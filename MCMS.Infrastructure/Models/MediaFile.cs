using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCMS.Infrastructure.Models;

[Index("FileType", "Visibility", Name = "IX_MediaFiles_TypeVisibility")]
[Index("UploadedBy", Name = "IX_MediaFiles_UploadedBy")]
public partial class MediaFile
{
    [Key]
    public int MediaId { get; set; }

    [StringLength(255)]
    public string FileName { get; set; } = null!;

    [StringLength(500)]
    public string FilePath { get; set; } = null!;

    [StringLength(50)]
    public string FileType { get; set; } = null!;

    public long FileSize { get; set; }

    public string UploadedBy { get; set; } = null!;

    [StringLength(50)]
    public string Visibility { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    public bool IsDeleted { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeletedAt { get; set; }

    [InverseProperty("Media")]
    public virtual ICollection<MediaFileTag> MediaFileTags { get; set; } = new List<MediaFileTag>();

    [InverseProperty("Media")]
    public virtual ICollection<MediaRelation> MediaRelations { get; set; } = new List<MediaRelation>();
}
